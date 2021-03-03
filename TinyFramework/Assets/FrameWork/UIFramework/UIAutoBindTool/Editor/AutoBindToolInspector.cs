using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.IO;
using LitJson;
using TFrameWork.UI;

public class BindData
{
    public string Name;
    public Component BindCom;
}

public class UISetting
{
    public string name;
    public string CodePath;
    public string NameSpace;
    public string ClassName;
}

[CustomEditor(typeof(UIPageBase))]
public class AutoBindToolInspector : Editor
{
    private string m_ClassName;
    private string m_Namespace;
    private string m_CodePath;
    private IAutoBindRuleHelper RuleHelper;
    private UIPageBase m_Target;
    private string[] s_AssemblyNames = { "Assembly-CSharp-Editor" };
    private string[] m_HelperTypeNames;
    private string m_HelperTypeName;
    private int m_HelperTypeNameIndex;
    private UISetting m_UISetting;

    public Dictionary<string, int> m_BindDataCount = new Dictionary<string, int>();
    public Dictionary<string, BindData> m_BindDatas = new Dictionary<string, BindData>();
    public List<BindData> m_BindDataList = new List<BindData>();
    private List<string> m_TempFiledNames = new List<string>();
    private List<string> m_TempComponentTypeNames = new List<string>();

    private void OnEnable()
    {
        m_Target = (UIPageBase)target;
        m_HelperTypeNames = GetTypeNames(typeof(IAutoBindRuleHelper), s_AssemblyNames);

        try
        {
            TextAsset UISettingDataJson = AssetDatabase.LoadAssetAtPath<TextAsset>(string.Format("Assets/FrameWork/UIFramework/UIAutoBindTool/Editor/UISetting/{0}.json", target.name));
            m_UISetting = JsonMapper.ToObject<UISetting>(UISettingDataJson.text);
        }
        catch
        {
            m_UISetting = new UISetting();
        }

        m_ClassName = m_UISetting.ClassName;
        m_CodePath = m_UISetting.CodePath;
        m_Namespace = m_UISetting.NameSpace;

    }

    public override void OnInspectorGUI()
    {
        DrawTopButton();
        base.OnInspectorGUI();

        DrawHelperSelect();

        DrawSetting();
    }

    /// <summary>
    /// 绘制顶部按钮
    /// </summary>
    private void DrawTopButton()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("生成绑定代码"))
        {
            m_BindDataCount.Clear();
            m_BindDatas.Clear();
            m_BindDataList.Clear();
            AutoBindComponent();
            GenAutoBindCode();
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// 自动绑定组件
    /// </summary>
    private void AutoBindComponent()
    {
        Transform[] childs = m_Target.gameObject.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in childs)
        {
            m_TempFiledNames.Clear();
            m_TempComponentTypeNames.Clear();

            if (RuleHelper.IsValidBind(child, m_TempFiledNames, m_TempComponentTypeNames))
            {
                for (int i = 0; i < m_TempFiledNames.Count; i++)
                {
                    Component com = child.GetComponent(m_TempComponentTypeNames[i]);
                    if (com == null)
                    {
                        Debug.LogError($"{child.name}上不存在{m_TempComponentTypeNames[i]}的组件");
                    }
                    else
                    {
                        BindData bindData = new BindData
                        {
                            Name = m_TempFiledNames[i],
                            BindCom = child.GetComponent(m_TempComponentTypeNames[i])
                        };
                        m_BindDataList.Add(bindData);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 绘制辅助器选择框
    /// </summary>
    private void DrawHelperSelect()
    {
        m_HelperTypeName = m_HelperTypeNames[0];

        if (RuleHelper != null)
        {
            m_HelperTypeName = RuleHelper.GetType().Name;

            for (int i = 0; i < m_HelperTypeNames.Length; i++)
            {
                if (m_HelperTypeName == m_HelperTypeNames[i])
                {
                    m_HelperTypeNameIndex = i;
                }
            }
        }
        else
        {
            IAutoBindRuleHelper helper = (IAutoBindRuleHelper)CreateHelperInstance(m_HelperTypeName, s_AssemblyNames);
            RuleHelper = helper;
        }

        int selectedIndex = EditorGUILayout.Popup("AutoBindRuleHelper", m_HelperTypeNameIndex, m_HelperTypeNames);
        if (selectedIndex != m_HelperTypeNameIndex)
        {
            m_HelperTypeNameIndex = selectedIndex;
            m_HelperTypeName = m_HelperTypeNames[selectedIndex];
            IAutoBindRuleHelper helper = (IAutoBindRuleHelper)CreateHelperInstance(m_HelperTypeName, s_AssemblyNames);
            RuleHelper = helper;
        }
    }

    /// <summary>
    /// 绘制设置项
    /// </summary>
    private void DrawSetting()
    {
        EditorGUILayout.BeginHorizontal();
        m_Namespace = EditorGUILayout.TextField(new GUIContent("命名空间："), m_Namespace);
        if (GUILayout.Button("恢复"))
        {
            m_Namespace = m_UISetting.NameSpace;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        m_ClassName = EditorGUILayout.TextField(new GUIContent("类名："), m_ClassName);
        if (GUILayout.Button("物体名"))
        {
            m_ClassName = m_Target.name;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("代码保存路径：");
        EditorGUILayout.LabelField(m_CodePath);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("选择路径"))
        {
            string temp = m_CodePath;
            m_CodePath = EditorUtility.OpenFolderPanel("选择代码保存路径", Application.dataPath, "");
            if (string.IsNullOrEmpty(m_CodePath))
            {
                m_CodePath = temp;
            }
        }
        if (GUILayout.Button("恢复"))
        {
            m_CodePath = m_UISetting.CodePath;
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("保存设置"))
        {
            m_UISetting.NameSpace = m_Namespace;
            m_UISetting.ClassName = m_ClassName;
            m_UISetting.CodePath = m_CodePath;
            SaveStting();
        }
    }

    /// <summary>
    /// 获取指定基类在指定程序集中的所有子类名称
    /// </summary>
    private string[] GetTypeNames(Type typeBase, string[] assemblyNames)
    {
        List<string> typeNames = new List<string>();
        foreach (string assemblyName in assemblyNames)
        {
            Assembly assembly = null;
            try
            {
                assembly = Assembly.Load(assemblyName);
            }
            catch
            {
                continue;
            }

            if (assembly == null)
            {
                continue;
            }

            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.IsClass && !type.IsAbstract && typeBase.IsAssignableFrom(type))
                {
                    typeNames.Add(type.FullName);
                }
            }
        }

        typeNames.Sort();
        return typeNames.ToArray();
    }

    /// <summary>
    /// 创建辅助器实例
    /// </summary>
    private object CreateHelperInstance(string helperTypeName, string[] assemblyNames)
    {
        foreach (string assemblyName in assemblyNames)
        {
            Assembly assembly = Assembly.Load(assemblyName);

            object instance = assembly.CreateInstance(helperTypeName);
            if (instance != null)
            {
                return instance;
            }
        }
        return null;
    }

    /// <summary>
    /// 生成自动绑定代码
    /// </summary>
    private void GenAutoBindCode()
    {
        GameObject go = m_Target.gameObject;

        string className = !string.IsNullOrEmpty(m_ClassName) ? m_ClassName : go.name;
        string codePath = !string.IsNullOrEmpty(m_CodePath) ? m_CodePath : m_UISetting.CodePath;

        if (!Directory.Exists(codePath))
        {
            Debug.LogError($"{go.name}的代码保存路径{codePath}无效");
        }

        using (StreamWriter sw = new StreamWriter($"{codePath}/{className}.cs"))
        {
            sw.WriteLine("using UnityEngine;");
            sw.WriteLine("using UnityEngine.UI;");
            sw.WriteLine("");
            sw.WriteLine("//自动生成于：" + DateTime.Now);

            if (!string.IsNullOrEmpty(m_Namespace))
            {
                //命名空间
                sw.WriteLine("namespace " + m_Namespace);
                sw.WriteLine("{");
            }

            //类名
            sw.WriteLine($"\tpublic partial class {className}:View");
            sw.WriteLine("\t{");

            //组件字段
            foreach (BindData data in m_BindDataList)
            {
                string name = data.Name;
                if (m_BindDataCount.ContainsKey(name))
                {
                    int count = m_BindDataCount[name];
                    count++;
                    m_BindDataCount[name] = count;
                    name = name + "_" + count;
                }
                else
                {
                    m_BindDataCount.Add(name, 0);
                }
                if (m_BindDatas.ContainsKey(name))
                {
                    Debug.Log("1111");
                }
                else
                {
                    Debug.Log(name);
                    m_BindDatas.Add(name, data);
                }

                sw.WriteLine($"\t\tpublic {data.BindCom.GetType().Name} {name} {{get; private set;}}");
            }
            sw.WriteLine("");

            sw.WriteLine("\t\tpublic void Awake()");
            sw.WriteLine("\t\t{");
            //获取autoBindTool上的Component
            foreach (KeyValuePair<string, BindData> item in m_BindDatas)
            {
                string filedName = $"{item.Key}";
                sw.WriteLine($"\t\t\t{filedName} = transform.GetComponetOnChildNode<{item.Value.BindCom.GetType().Name}>(\"{GetPath(item.Value.BindCom.transform)}\");");
            }

            sw.WriteLine("\t\t}");

            sw.WriteLine("\t}");

            if (!string.IsNullOrEmpty(m_Namespace))
            {
                sw.WriteLine("}");
            }
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("提示", "代码生成完毕", "OK");
    }

    private string GetPath(Transform item)
    {
        string path = "";
        List<string> paths = new List<string>();
        GetParent(item.parent, paths);
        for (int i = paths.Count - 1; i > -1; i--)
        {
            if (string.IsNullOrEmpty(path))
                path += paths[i];
            else
                path += ("/" + paths[i]);
        }
        if (string.IsNullOrEmpty(path))
            path += item.gameObject.name;
        else
            path += ("/" + item.gameObject.name);
        return path;
    }

    private void GetParent(Transform item, List<string> paths)
    {
        if (item.gameObject.name != target.name)
        {
            paths.Add(item.name);
            GetParent(item.parent, paths);
        }
    }

    public void SaveStting()
    {
        string path = Application.dataPath + string.Format("/FrameWork/UIFramework/UIAutoBindTool/Editor/UISetting/{0}.json", target.name);  //路径
        string setting = JsonMapper.ToJson(m_UISetting);
        File.WriteAllText(path, setting);

        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayDialog("UI自动绑定", "保存成功", "好的");
    }
}
