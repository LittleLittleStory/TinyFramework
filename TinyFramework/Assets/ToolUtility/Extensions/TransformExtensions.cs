using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    /// <summary>
    /// 获取对应名字的子节点上的对应类型组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="transform">父节点 transform</param>
    /// <param name="nodeName">子节点名字</param>
    /// <returns>返回对应组件，没有找到返回null</returns>
    public static T GetComponetOnChildNode<T>(this Transform transform, string nodeName)
    {
        if (null == transform)
        {
#if UNITY_EDITOR
            string stackInfo = new System.Diagnostics.StackTrace(true).ToString();
            Debug.LogError(string.Format("{0}获取失败，路径{1}", typeof(T).ToString(), nodeName));
#endif
            return default(T);
        }

        var nodeTrans = transform.Find(nodeName);
        if (null == nodeTrans)
        {
#if UNITY_EDITOR
            string stackInfo = new System.Diagnostics.StackTrace(true).ToString();
            Debug.LogError(string.Format("{0}获取失败，路径{1}", typeof(T).ToString(), nodeName));
#endif
            return default(T);
        }

        T result = nodeTrans.GetComponent<T>();
        if (null == result)
        {
#if UNITY_EDITOR
            string stackInfo = new System.Diagnostics.StackTrace(true).ToString();
            Debug.LogError(string.Format("{0}获取失败，路径{1}", typeof(T).ToString(), nodeName));
#endif
            return default(T);
        }
        return result;
    }
}
