using System.IO;
using System.Collections.Generic;
using System.Text;
using TFramework.GameDataConfig.Enums;
using TFramework.GameDataConfig.Utility;

namespace TFramework.GameDataConfig.Configs
{
   public abstract class GameData
    {
       protected Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
       public GameData(BinaryReader reader, List<GameDataValue> keyTypes)
       {
           for (int i = 0; i < keyTypes.Count; i++)
           {
               GameDataValue gameDataValue = keyTypes[i];
               switch ((DataType)gameDataValue.KeyType)
               {
                   case DataType.int16:
                       keyValuePairs[gameDataValue.KeyName] = reader.ReadInt16();
                       break;
                   case DataType.int64:
                       keyValuePairs[gameDataValue.KeyName] = reader.ReadInt64();
                       break;
                   case DataType.int32:
                   case DataType.ID:
                       keyValuePairs[gameDataValue.KeyName] = reader.ReadInt32();
                       break;
                   case DataType.int8:
                       keyValuePairs[gameDataValue.KeyName] = reader.ReadByte();
                       break;
                   case DataType.Float:
                       keyValuePairs[gameDataValue.KeyName] = reader.ReadSingle();
                       break;
                   case DataType.str:
                       keyValuePairs[gameDataValue.KeyName] = new string(reader.ReadChars(reader.ReadInt16()));
                       break;
                   case DataType.int32Array:
                       string strInt32Array = new string(reader.ReadChars(reader.ReadInt16()));
                       keyValuePairs[gameDataValue.KeyName] = StringUtility.StringToIntArray(strInt32Array);
                       break;
                   case DataType.int32ArrayArray:
                       string strInt32ArrayArray = new string(reader.ReadChars(reader.ReadInt16()));
                       keyValuePairs[gameDataValue.KeyName] = StringUtility.StringToIntArrayArray(strInt32ArrayArray);
                       break;
                   case DataType.utf8str:
                       var utf8str = reader.ReadBytes(reader.ReadInt16());
                       keyValuePairs[gameDataValue.KeyName] = Encoding.Default.GetString(utf8str);
                       break;
                   default:
                       break;
               }
           }
       }
       public virtual int makeIntKey() { return 0; }
       public virtual string makeStrKey() { return ""; }
       public virtual void setMaxIntKey(ref int maxID) {  }
       public virtual void setMaxIntKeyDic(Dictionary<int, int> maxIDDic) {  }
   }

 public class AllianceManageData : GameData
{
     public readonly int ID;
     public readonly int master;
     public readonly int vice_master;
     public readonly int elite_person;
     public readonly int normal_person;

     public AllianceManageData(BinaryReader reader, List<GameDataValue> keyTypes) : base(reader, keyTypes)
        {
         if (keyValuePairs.ContainsKey("ID"))
           ID = (int)keyValuePairs["ID"];
         if (keyValuePairs.ContainsKey("master"))
           master = (int)keyValuePairs["master"];
         if (keyValuePairs.ContainsKey("vice_master"))
           vice_master = (int)keyValuePairs["vice_master"];
         if (keyValuePairs.ContainsKey("elite_person"))
           elite_person = (int)keyValuePairs["elite_person"];
         if (keyValuePairs.ContainsKey("normal_person"))
           normal_person = (int)keyValuePairs["normal_person"];

         keyValuePairs.Clear();
       }

         public override int makeIntKey() {return ID;}

         public override void setMaxIntKey(ref int maxID) { if (ID > maxID) { maxID = ID; }}
 };

      
}