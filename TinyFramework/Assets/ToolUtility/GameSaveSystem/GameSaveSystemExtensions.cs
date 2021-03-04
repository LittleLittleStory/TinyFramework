using System.Collections;
using System.Collections.Generic;
using TFrameWork.Singleton;
using UnityEngine;

public static class GameSaveSystemExtensions
{
    private static GameSaveSystem gameSaveSystem
    {
        get
        {
            return SingletonManager.GetSingleton<GameSaveSystem>();
        }
    }

    /// <summary>
    /// 保存场景对象组件，只写进内存中的存档
    /// </summary>
    /// <typeparam name="T1">赋值操作对象类型。</typeparam>
    /// <typeparam name="T2">你希望保存组件对象类型
    /// 请务必保证该类型中的泛型，是你希望保存组件对象类型</typeparam>
    /// <param name="gameObject">gameObject</param>
    /// <param name="sceneName">场景名，不填为当前场景</param>
    public static void SaveComponent<T1, T2>(this GameObject gameObject, string sceneName = "")
        where T1 : ISave<T2>
    {
        if (gameObject.CheckEmpty())
            return;
        T2 component = gameObject.GetComponent<T2>();
        if (component.CheckEmpty())
            return;
        gameSaveSystem.SaveComponent<T1, T2>(gameObject, component, sceneName);
    }

    public static void SaveComponent<T1, T2>(this T2 component, string sceneName = "")
        where T1 : ISave<T2>
        where T2 : UnityEngine.Component
    {
        if (component.CheckEmpty())
            return;
        gameSaveSystem.SaveComponent<T1, T2>(component.gameObject, component, sceneName);
    }

    /// <summary>
    /// 保存全局数据，只读当前内存中的存档
    /// </summary>
    /// <typeparam name="T1">赋值操作对象类型。</typeparam>
    /// <typeparam name="T2">你希望保存组件对象类型
    /// 请务必保证该类型中的泛型，是你希望保存组件对象类型</typeparam>
    /// <param name="component">全局对象</param>
    public static void SaveGobal<T1, T2>(this T2 component)
        where T1 : ISave<T2>
    {
        if (component.CheckEmpty())
            return;
        gameSaveSystem.SaveGobal<T1, T2>(component);
    }

    /// <summary>
    ///  读取内存中的存档值
    /// </summary>
    /// <typeparam name="T1">赋值操作对象类型。</typeparam>
    /// <typeparam name="T2">你希望保存组件对象类型
    /// 请务必保证该类型中的泛型，是你希望保存组件对象类型</typeparam>
    /// <param name="gameObject">gameObject</param>
    /// <param name="sceneName">场景名，不填为当前场景</param>
    /// <returns></returns>
    public static bool LoadComponent<T1, T2>(this GameObject gameObject, string sceneName = "")
        where T1 : ISave<T2>
        where T2 : UnityEngine.Component
    {
        if (gameObject.CheckEmpty())
            return false;
        T2 component = gameObject.GetComponent<T2>();
        if (component.CheckEmpty())
            return false;

        SaveObject saveObject = GameSaveUtility.GetSaveObjectData(gameObject.name, sceneName);
        SetValue setValue = GameSaveUtility.GetComponentSetValue<T1, T2>(gameObject, saveObject);
        if (null == setValue)
            return false;
        bool result = gameSaveSystem.Load<T1, T2>(component, setValue);
        return result;
    }

    /// <summary>
    /// 读取内存中的全局数据
    /// </summary>
    /// <typeparam name="T1">赋值操作对象类型。</typeparam>
    /// <typeparam name="T2">你希望保存组件对象类型
    /// 请务必保证该类型中的泛型，是你希望保存组件对象类型</typeparam>
    /// <param name="component">全局对象</param>
    /// <returns></returns>
    public static bool LoadGobal<T1, T2>(this T2 component)
        where T1 : ISave<T2>
    {
        if (component.CheckEmpty())
            return false;
        string componentName = typeof(T2).Name;
        GobalData gobalData = GameSaveUtility.GetGobalObjectData(componentName);
        SetValue setValue = GameSaveUtility.GetGobalSetValue<T1, T2>(gobalData);
        if (null == setValue)
            return false;
        bool result = gameSaveSystem.Load<T1, T2>(component, setValue);
        return result;
    }
}
