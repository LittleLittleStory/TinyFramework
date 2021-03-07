using TFramework.GameDataConfig.Enums;
using System.Collections.Generic;
using TFramework.GameDataConfig.Configs;

namespace TFramework.GameDataConfig.Utility
{
    public static class GameDataUtility
    {
        private static GameDataStore[] gameDataList = new GameDataStore[(int)EnConfig.Count];

        public static void InitConfig(int enConfig, short keyType, List<GameData> dataList)
        {
            gameDataList[enConfig] = new GameDataStore((EnConfig)enConfig, keyType, dataList);
        }

        /// <summary>
        /// 判断是否加载过这个Config
        /// </summary>
        /// <param name="enConfig">ConfigID</param>
        /// <returns>false 没有加载过</returns>
        public static bool HasConfig(int enConfig)
        {
            if (enConfig < 0
                || enConfig >= gameDataList.Length
                )
            {
                return false;
            }

            return gameDataList[enConfig] != null;
        }

        public static int GetGameDataCount<T>(EnConfig enConfig) where T : GameData
        {
            if (enConfig < 0 || enConfig >= EnConfig.Count || gameDataList[(int)enConfig] == null)
            {
                return 0;
            }

            return gameDataList[(int)enConfig].DataCount;
        }

        public static List<GameData> GetGameDataList<T>(EnConfig enConfig) where T : GameData
        {
            if (enConfig < 0 || enConfig >= EnConfig.Count || gameDataList[(int)enConfig] == null)
            {
                return null;
            }

            return gameDataList[(int)enConfig].DataList;
        }

        public static T GetGameData<T>(EnConfig enConfig, int ID) where T : GameData
        {
            if (enConfig < 0 || enConfig >= EnConfig.Count || gameDataList[(int)enConfig] == null)
            {
                return null;
            }

            return gameDataList[(int)enConfig].GetGameData<T>(ID);
        }

        public static T GetGameData<T>(EnConfig enConfig, int ID, int ID1) where T : GameData
        {
            if (enConfig < 0 || enConfig >= EnConfig.Count || gameDataList[(int)enConfig] == null)
            {
                return null;
            }

            return gameDataList[(int)enConfig].GetGameData<T>(ID, ID1);
        }

        public static T GetGameData<T>(EnConfig enConfig, int ID, int ID1, int ID2) where T : GameData
        {
            if (enConfig < 0 || enConfig >= EnConfig.Count || gameDataList[(int)enConfig] == null)
            {
                return null;
            }

            return gameDataList[(int)enConfig].GetGameData<T>(ID, ID1, ID2);
        }

        public static T GetGameData<T>(EnConfig enConfig, string ID) where T : GameData
        {
            if (enConfig < 0 || enConfig >= EnConfig.Count || gameDataList[(int)enConfig] == null)
            {
                return null;
            }

            return gameDataList[(int)enConfig].GetGameData<T>(ID);
        }

        public  static int GetGameDataMaxKey(EnConfig enConfig)
        {
            if (enConfig < 0 || enConfig >= EnConfig.Count || gameDataList[(int)enConfig] == null)
            {
                return 0;
            }

            return gameDataList[(int)enConfig].GetGameDataMaxKey();
        }

        public static int GetGameDataMaxKey(EnConfig enConfig, int firstKey)
        {
            if (enConfig < 0 || enConfig >= EnConfig.Count || gameDataList[(int)enConfig] == null)
            {
                return 0;
            }

            return gameDataList[(int)enConfig].GetGameDataMaxKey(firstKey);
        }
    }
}