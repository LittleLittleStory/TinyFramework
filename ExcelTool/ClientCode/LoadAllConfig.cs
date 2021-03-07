using UnityEngine;
using TinyJoy.SSG.Game.Project.Enums;
using TinyJoy.SSG.Game.Configs;
using System.Collections.Generic;
using UnityEngine.UI;

namespace TinyJoy.SSG.Game.Project.Utility
{
    public static class LoadAllConfig
    {
        #region 加载配置表
        public async UniTaskVoid LoadAllConfig()
        {
            int configCount = (int)EnConfig.Count;
            UniTask<AsyncOperationHandle<TextAsset>>[] handles = new UniTask<AsyncOperationHandle<TextAsset>>[configCount];
            for (int i = 0; i < configCount; i++)
            {
                handles[i] = LoadConfig(i);
            }

            var configDatas = await UniTask.WhenAll(handles);
            for (int i = 0; i < configDatas.Length; ++i)
            {
                if (configDatas[i].Status == AsyncOperationStatus.Succeeded)
                {
                    if (i >= CongfigConst.ConfigName.Length)
                    {
                        DebugLogger.Debug("LoadAllConfig: ...... 当前index 超过了ConfigName的长度！！！");
                        return;
                    }

                    string configDataName = CongfigConst.ConfigName[i];
                    string dataClassName = "TinyJoy.SSG.Game.Configs." + configDataName;

                    if (null == configDatas[i].Result)
                    {
                        DebugLogger.Debug("LoadAllConfig: ......" + dataClassName + " 数据读出为空！！！");
                        return;
                    }

                    LoadConfigData(configDatas[i].Result.bytes, dataClassName, i);
                }

            }
            for (int i = 0; i < configDatas.Length; ++i)
            {
                if (configDatas[i].IsValid())
                {
                    resourceLoader.UnloadAsset(configDatas[i]);
                }
            }

            await furnitureManager.LoadFurnitureDataAsync();
            GameDataUtility.InitFilterList();
            CollectionService.LoadCollectionData();
            playerService.CountItemBagRes();
            PlayerAccountDataContainer.LoadAccount();
            loadStep.Value = EnGameLoadStep.LoadAtlas;
        }

        private void LoadConfigData(byte[] data, string dataClassName, int enConfig)
        {
            using (var memoryStream = new MemoryStream(data))
            {
                using (var binaryReader = new BinaryReader(memoryStream, Encoding.ASCII))
                {
                    List<GameDataValue> keyTypes = new List<GameDataValue>();
                    int count = binaryReader.ReadInt16();

                    if (count == 0)
                    {
                        return;
                    }

                    for (int i = 0; i < count; i++)
                    {
                        GameDataValue gameDataValue = new GameDataValue();
                        gameDataValue.KeyName = new string(binaryReader.ReadChars(binaryReader.ReadInt16()));
                        gameDataValue.KeyType = binaryReader.ReadInt16();
                        keyTypes.Add(gameDataValue);
                    }

                    //定义参数类型数组  
                    Type[] tps = new Type[2];
                    tps[0] = typeof(BinaryReader);
                    tps[1] = typeof(List<GameDataValue>);

                    //定义参数数组  
                    object[] obj = new object[2];
                    obj[0] = (object)binaryReader;
                    obj[1] = (object)keyTypes;

                    short keyType = (short)EnConfigKeyType.OneID;
                    keyType = binaryReader.ReadInt16();

                    List<GameData> dataList = new List<GameData>();
                    var type = Type.GetType(dataClassName);// 通过类名获取同名类

                    if (type != null)
                    {
                        while (binaryReader.PeekChar() != -1)
                        {
                            var gameData = type.GetConstructor(tps).Invoke(obj) as GameData;
                            dataList.Add(gameData);
                        }

                        GameDataUtility.InitConfig(enConfig, keyType, dataList);
                    }
                }
            }
        }

        private async UniTask<AsyncOperationHandle<TextAsset>> LoadConfig(int enConfig)
        {

            string configDataName = CongfigConst.ConfigName[enConfig];
            string resFileName = configDataName + ".bytes";

            // if (enConfig == (int)EnConfig.TextData)
            // {
            //     resFileName = GameDataUtility.GetTextDataName() + ".bytes";
            // }

            return await resourceLoader.LoadAssetAsync<TextAsset>(resFileName);

            //if (resource.Status == AsyncOperationStatus.Succeeded)
            //{
            //    using (var memoryStream = new MemoryStream(resource.Result.bytes))
            //    {
            //        using (var binaryReader = new BinaryReader(memoryStream, Encoding.ASCII))
            //        {
            //            //定义参数类型数组  
            //            Type[] tps = new Type[1];
            //            tps[0] = typeof(BinaryReader);

            //            //定义参数数组  
            //            object[] obj = new object[1];
            //            obj[0] = (object)binaryReader;

            //            short keyType = (short)EnConfigKeyType.OneID;

            //            if (enConfig == (int)EnConfig.TextData)
            //            {
            //                keyType = (short)EnConfigKeyType.TextID;
            //            }
            //            else
            //            {
            //                keyType = binaryReader.ReadInt16(); 
            //            }
            //            List<GameData> dataList = new List<GameData>();
            //            var type = Type.GetType(dataClassName);// 通过类名获取同名类

            //            if (type != null)
            //            {
            //                while (binaryReader.PeekChar() != -1)
            //                {
            //                    var gameData = type.GetConstructor(tps).Invoke(obj) as GameData;

            //                    dataList.Add(gameData);
            //                }

            //                GameDataUtility.InitConfig(enConfig, keyType, dataList);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    throw new ResourceException($"load asset failed!!!", configDataName);
            //}

            //if (resource.IsValid())
            //{
            //    resourceLoader.UnloadAsset(resource);
            //}
        }
        #endregion
    }
}
}