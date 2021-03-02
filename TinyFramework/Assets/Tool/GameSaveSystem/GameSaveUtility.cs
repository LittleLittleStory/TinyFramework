using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameSaveUtility
{
    private static GameSaveSystem gameSaveSystem
    {
        get
        {
            return GameSaveSystem.gameSaveSystem;
        }
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    public static void SaveGame()
    {
        ToolUtility.SaveJson(gameSaveSystem.GameData, "GameData");
    }

    public static GobalData AddGobalObjectData(string name)
    {
        if (gameSaveSystem.GameData.GobalDatas.ContainsKey(name))
        {
            Debug.LogError(string.Format("存档已存在{0}对应的物体", name));
            return null;
        }
        else
        {
            GobalData gobalData = new GobalData(name);
            gameSaveSystem.GameData.GobalDatas.Add(name, gobalData);
            return gobalData;
        }
    }

    public static GobalData GetGobalObjectData(string name)
    {
        if (gameSaveSystem.GameData.GobalDatas.ContainsKey(name))
            return gameSaveSystem.GameData.GobalDatas[name];
        else
            return null;
    }

    public static SetValue GetGobalSetValue<T1, T2>(GobalData gobalData)
    where T1 : ISave<T2>
    {
        string ISaveName = typeof(T1).Name;
        string componentName = typeof(T2).Name;
        if (null == gobalData)
        {
            Debug.LogError(string.Format("存档未找到{0}全局数据", componentName));
            return null;
        }
        if (false == gobalData.SetValues.ContainsKey(componentName))
        {
            Debug.LogError(string.Format("存档未找到{0}对应的组件类型", componentName));
            return null;
        }
        Dictionary<string, SetValue> setValues = gobalData.SetValues[componentName];
        if (false == setValues.ContainsKey(ISaveName))
        {
            Debug.LogError(string.Format("存档未找到{0}对应的赋值操作类型", ISaveName));
            return null;
        }
        return setValues[ISaveName];
    }

    public static SceneData AddSceneData(string sceneName)
    {
        if (gameSaveSystem.GameData.SceneDatas.ContainsKey(sceneName))
        {
            Debug.LogError(string.Format("存档已存在{0}对应的场景", sceneName));
            return null;
        }
        else
        {
            SceneData sceneData = new SceneData(sceneName);
            gameSaveSystem.GameData.SceneDatas.Add(sceneName, sceneData);
            return sceneData;
        }
    }

    public static SceneData GetSceneData(string sceneName)
    {
        if (gameSaveSystem.GameData.SceneDatas.ContainsKey(sceneName))
        {
            return gameSaveSystem.GameData.SceneDatas[sceneName];
        }
        else
        {
            return null;
        }
    }

    public static SaveObject AddSaveObjectData(string name, string sceneName)
    {
        SceneData sceneData = GetSceneData(sceneName);
        if (null == sceneData)
            AddSceneData(sceneName);
        if (sceneData.SaveObjects.ContainsKey(name))
        {
            Debug.LogError(string.Format("存档已存在{0}对应的物体", name));
            return null;
        }
        else
        {
            SaveObject saveObject = new SaveObject(name);
            sceneData.SaveObjects.Add(name, saveObject);
            return saveObject;
        }
    }

    public static SaveObject GetSaveObjectData(string name, string sceneName = "")
    {
        sceneName = string.IsNullOrEmpty(sceneName) == true ? SceneManager.GetActiveScene().name : sceneName;
        SceneData sceneData = GetSceneData(sceneName);
        if (null == sceneData)
        {
            Debug.LogError(string.Format("存档未找到{0}对应的场景", sceneName));
            return null;
        }
        if (sceneData.SaveObjects.ContainsKey(name))
            return sceneData.SaveObjects[name];
        else
            return null;
    }

    public static SetValue GetComponentSetValue<T1, T2>(GameObject gameObject, SaveObject saveObject)
    where T1 : ISave<T2>
    {
        string ISaveName = typeof(T1).Name;
        string componentName = typeof(T2).Name;
        if (null == saveObject)
        {
            Debug.LogError(string.Format("存档未找到{0}对应的物体", gameObject.name));
            return null;
        }
        if (false == saveObject.SetValues.ContainsKey(componentName))
        {
            Debug.LogError(string.Format("存档未找到{0}对应的组件类型", componentName));
            return null;
        }
        Dictionary<string, SetValue> setValues = saveObject.SetValues[componentName];
        if (false == setValues.ContainsKey(ISaveName))
        {
            Debug.LogError(string.Format("存档未找到{0}对应的赋值操作类型", ISaveName));
            return null;
        }
        return setValues[ISaveName];
    }
}
