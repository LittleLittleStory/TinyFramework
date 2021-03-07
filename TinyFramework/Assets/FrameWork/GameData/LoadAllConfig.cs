using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using TFramework.GameDataConfig.Configs;

namespace TFramework.GameDataConfig.Utility
{
    public class LoadAllConfigA
    {
        #region 加载配置表
        public void LoadAllConfig()
        {
            int configCount = (int)EnConfig.Count;
            TextAsset[] configDatas = new TextAsset[configCount];
            for (int i = 0; i < configCount; i++)
            {
                configDatas[i] = LoadConfig(i);
            }

            for (int i = 0; i < configDatas.Length; ++i)
            {
                string configDataName = CongfigConst.ConfigName[i];
                string dataClassName = "TinyJoy.SSG.Game.Configs." + configDataName;

                if (null == configDatas[i])
                {
                    Debug.Log("LoadAllConfig: ......" + dataClassName + " 数据读出为空！！！");
                    continue;
                }

                if (i >= CongfigConst.ConfigName.Length)
                {
                    Debug.Log("LoadAllConfig: ...... 当前index 超过了ConfigName的长度！！！");
                    continue;
                }
                LoadConfigData(configDatas[i].bytes, dataClassName, i);
            }
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

        private TextAsset LoadConfig(int enConfig)
        {

            string configDataName = CongfigConst.ConfigName[enConfig];
            string resFileName = configDataName + ".bytes";
            return  Resources.Load<TextAsset>(resFileName);
        }
        #endregion
    }
}
