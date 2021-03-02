using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

public interface IGameSaveSystem
{
    GameData gameData { get; }

    void SaveGobal<T1, T2>(string componentName, string value) where T1 : ISave<T2>;

    void SaveComponent<T1, T2>(string name, string value, string sceneName) where T1 : ISave<T2>;

    bool Load<T1, T2>(T2 component, SetValue setValue) where T1 : ISave<T2>;
}
