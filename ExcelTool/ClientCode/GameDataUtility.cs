using UnityEngine;
using TinyJoy.SSG.Game.Project.Enums;
using TinyJoy.SSG.Game.Configs;
using System.Collections.Generic;
using UnityEngine.UI;

namespace TinyJoy.SSG.Game.Project.Utility
{
    public static class GameDataUtility
    {
        private static GameDataStore[] gameDataList = new GameDataStore[(int)EnConfig.Count];
        private static Dictionary<int, List<int>> filterSubTypeDic = new Dictionary<int, List<int>>();

        private static string text_lan = "cn";
        public static string GetLanguage()
        {
            return text_lan;
        }

        public static string GetTextDataName()
        {
            return "Text_" + text_lan;
        }

        public static void SetLanguage(string lan)
        {
            text_lan = lan;
        }

        public static void InitConfig(int enConfig, short keyType, List<GameData> dataList)
        {
            gameDataList[enConfig] = new GameDataStore((EnConfig)enConfig, keyType, dataList);
        }

        public static void InitFilterList()
        {
            var datas = GetGameDataList<FilterListData>(EnConfig.FilterListData);
            foreach (var gameData in datas)
            {
                var data = gameData as FilterListData;
                var id = data.ID;
                if (id == FilterType.IsOwner)
                {
                    continue;
                }
                var value = data.ID1;
                if (false == filterSubTypeDic.ContainsKey(id))
                {
                    filterSubTypeDic.Add(id, new List<int>());
                }
                filterSubTypeDic[id].Add(value);
            }
        }

        public static List<int> GetFilterSubList(int typeId)
        {
            List<int> list;
            filterSubTypeDic.TryGetValue(typeId, out list);
            return list;
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

        public static string GetText(string textID)
        {
            return "";
            // if (gameDataList[(int)EnConfig.TextData] == null)
            // {
            //     return StringUtility.StrEmpty;
            // }
            //
            // var gameData = gameDataList[(int)EnConfig.TextData].GetGameData<TextData>(textID);
            //
            // if (gameData == null)
            // {
            //     return StringUtility.StrEmpty;
            // }
            //
            // return gameData.Text;
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