using System.Collections.Generic;
using TinyJoy.SSG.Game.Project.Enums;
using TinyJoy.SSG.Game.Project.Utility;

namespace TinyJoy.SSG.Game.Configs
{
    public class GameDataValue
    {
        public string KeyName;
        public int KeyType;
    }

    public enum DataType
    {
        int8 = 0,
        int16,
        int32,
        str,
        ID,
        int64,
        Float,
        int32Array,
        int32ArrayArray,
        utf8str,
        unsupported,
    }

    public class GameDataStore
    {
        private EnConfigKeyType keyType;
        private Dictionary<int, GameData> iDataDic;
        private Dictionary<string, GameData> sDataDic;

        public readonly List<GameData> DataList;
        public readonly int DataCount;

        public readonly EnConfig configType;

        public int maxIDValue;
        public Dictionary<int, int> maxIDValueDic;

        public GameDataStore(EnConfig configType, short keyType, List<GameData> dataList)
        {
            this.configType = configType;
            this.keyType = (EnConfigKeyType)keyType;
            DataList = dataList;
            DataCount = dataList.Count;

            maxIDValue = 0;
            maxIDValueDic = new Dictionary<int, int>();

            InitDic();
        }

        private void InitDic()
        {
            if (DataList == null || DataCount == 0)
            {
                return;
            }
            if (keyType == EnConfigKeyType.OneID)
            {
                iDataDic = new Dictionary<int, GameData>(DataCount);
                foreach (GameData data in DataList)
                {
                    iDataDic[data.makeIntKey()] = data;
                    data.setMaxIntKey(ref maxIDValue);
                }
            }
            else
            {
                sDataDic = new Dictionary<string, GameData>(DataCount);
                foreach (GameData data in DataList)
                {
                    sDataDic[data.makeStrKey()] = data;
                    if (keyType == EnConfigKeyType.TwoID)
                    {
                        data.setMaxIntKeyDic(maxIDValueDic);
                    }
                }
            }
        }

        public T GetGameData<T>(int ID) where T : GameData
        {
            if (keyType != EnConfigKeyType.OneID)
            {
                return null;
            }

            if (iDataDic == null || iDataDic.ContainsKey(ID) == false)
            {
#if UNITY_EDITOR
                if (ID > 0)
                {
                    UnityEngine.Debug.LogError(string.Format("{0}不存在{1}", typeof(T).ToString(), ID));
                }      
#endif
                return null;
            }

            return (T)iDataDic[ID];
        }

        public int GetGameDataMaxKey()
        {
            return maxIDValue;
        }

        public int GetGameDataMaxKey(int ID)
        {
            if (keyType != EnConfigKeyType.TwoID)
            {
                return 0;
            }

            if (maxIDValueDic.ContainsKey(ID))
            {
                return maxIDValueDic[ID];
            }
            else
            {
                return 0;
            }
        }

        public T GetGameData<T>(int ID, int ID1) where T : GameData
        {
            if (keyType != EnConfigKeyType.TwoID)
            {
                return null;
            }

            string strID = StringUtility.MakeStrKey(ID, ID1);

            if (sDataDic == null || sDataDic.ContainsKey(strID) == false)
            {
                return null;
            }

            return (T)sDataDic[strID];
        }

        public T GetGameData<T>(int ID, int ID1, int ID2) where T : GameData
        {
            if (keyType != EnConfigKeyType.ThreeID)
            {
                return null;
            }

            string strID = StringUtility.MakeStrKey(ID, ID1, ID2);

            if (sDataDic == null || sDataDic.ContainsKey(strID) == false)
            {
                return null;
            }

            return (T)sDataDic[strID];
        }

        public T GetGameData<T>(string ID) where T : GameData
        {
            if (keyType != EnConfigKeyType.StrID && keyType != EnConfigKeyType.TextID)
            {
                return null;
            }

            if (sDataDic == null || sDataDic.ContainsKey(ID) == false)
            {
                return null;
            }

            return (T)sDataDic[ID];
        }
    }
}