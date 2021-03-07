using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace DataFileExporter
{
    class DataOverFlowException : System.Exception { }

    class Program
    {
        static Dictionary<string, string> s_argsValueSetting = new Dictionary<string, string>();
        static Dictionary<string, bool> s_argsSetting = new Dictionary<string, bool>();
        static Encoding s_binStrEncoding = Encoding.Unicode;

        static string s_lastSheetName = null;
        static string s_lastColName = null;//供错误信息使用
        static int s_lastRow = -1;//供错误信息使用
        static string s_enumString = "";
        static string s_constString = "";

        static Microsoft.Office.Interop.Excel.Application s_xlsApp = null;//new Application();
        static Workbook s_xlsWorkbook = null;

        //static StreamWriter s_FileWriter = null;
        static string s_md5fileName = null;
        static int s_keyType = 1;

        static string[] enum_str_macro = null;
        static string[] enum_str_macro_name = null;
        static string[] enum_title = null;

        public static int testByte(int num)
        {
            if (num > 255)
            {
                throw new DataOverFlowException();
            }

            return num;
        }

        public static int testShort(int num)
        {
            if (num > 65535)
            {
                throw new DataOverFlowException();
            }

            return num;
        }

        static void Main(string[] args)
        {
            enum_str_macro = null;
            enum_str_macro_name = null;
            enum_title = null;
            string outPutString = "";

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-start"))
                {
                    outPutString = "using System.IO;\nusing System.Collections.Generic;\nusing System.Text;\nusing TFramework.GameDataConfig.Enums;\nusing TFramework.GameDataConfig.Utility;\n\nnamespace TFramework.GameDataConfig.Configs\n{\n";
                    outPutString += "   public abstract class GameData\n    {\n";
                    outPutString += "       protected Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();\n";

                    outPutString += "       public GameData(BinaryReader reader, List<GameDataValue> keyTypes)\n";
                    outPutString += "       {\n";
                    outPutString += "           for (int i = 0; i < keyTypes.Count; i++)\n";
                    outPutString += "           {\n";
                    outPutString += "               GameDataValue gameDataValue = keyTypes[i];\n";
                    outPutString += "               switch ((DataType)gameDataValue.KeyType)\n";
                    outPutString += "               {\n";
                    outPutString += "                   case DataType.int16:\n";
                    outPutString += "                       keyValuePairs[gameDataValue.KeyName] = reader.ReadInt16();\n";
                    outPutString += "                       break;\n";
                    outPutString += "                   case DataType.int64:\n";
                    outPutString += "                       keyValuePairs[gameDataValue.KeyName] = reader.ReadInt64();\n";
                    outPutString += "                       break;\n";
                    outPutString += "                   case DataType.int32:\n";
                    outPutString += "                   case DataType.ID:\n";
                    outPutString += "                       keyValuePairs[gameDataValue.KeyName] = reader.ReadInt32();\n";
                    outPutString += "                       break;\n";
                    outPutString += "                   case DataType.int8:\n";
                    outPutString += "                       keyValuePairs[gameDataValue.KeyName] = reader.ReadByte();\n";
                    outPutString += "                       break;\n";
                    outPutString += "                   case DataType.Float:\n";
                    outPutString += "                       keyValuePairs[gameDataValue.KeyName] = reader.ReadSingle();\n";
                    outPutString += "                       break;\n";
                    outPutString += "                   case DataType.str:\n";
                    outPutString += "                       keyValuePairs[gameDataValue.KeyName] = new string(reader.ReadChars(reader.ReadInt16()));\n";
                    outPutString += "                       break;\n";
                    outPutString += "                   case DataType.int32Array:\n";
                    outPutString += "                       string strInt32Array = new string(reader.ReadChars(reader.ReadInt16()));\n";
                    outPutString += "                       keyValuePairs[gameDataValue.KeyName] = StringUtility.StringToIntArray(strInt32Array);\n";
                    outPutString += "                       break;\n";
                    outPutString += "                   case DataType.int32ArrayArray:\n";
                    outPutString += "                       string strInt32ArrayArray = new string(reader.ReadChars(reader.ReadInt16()));\n";
                    outPutString += "                       keyValuePairs[gameDataValue.KeyName] = StringUtility.StringToIntArrayArray(strInt32ArrayArray);\n";
                    outPutString += "                       break;\n";
                    outPutString += "                   case DataType.utf8str:\n";
                    outPutString += "                       var utf8str = reader.ReadBytes(reader.ReadInt16());\n";
                    outPutString += "                       keyValuePairs[gameDataValue.KeyName] = Encoding.Default.GetString(utf8str);\n";
                    outPutString += "                       break;\n";
                    outPutString += "                   default:\n";
                    outPutString += "                       break;\n";
                    outPutString += "               }\n";
                    outPutString += "           }\n";
                    outPutString += "       }\n";
                    outPutString += "       public virtual int makeIntKey() { return 0; }\n";
                    outPutString += "       public virtual string makeStrKey() { return \"\"; }\n";
                    outPutString += "       public virtual void setMaxIntKey(ref int maxID) {  }\n";
                    outPutString += "       public virtual void setMaxIntKeyDic(Dictionary<int, int> maxIDDic) {  }\n";
                    outPutString += "   }\n\n";

                    s_enumString = "namespace TFramework.GameDataConfig.Enums\n  {\n";
                    s_enumString += "   public enum EnConfig\n    {\n";

                    s_constString = "namespace TFramework.GameDataConfig.Const\n  {";
                    s_constString += "    public static class CongfigConst\n    {\n";
                    s_constString += "        public static string[] ConfigName =\n    {\n";

                    WriteToFile(outPutString, false);
                    return;
                }

                if (args[i].Equals("-end"))
                {
                    outPutString += "      \n}";
                    s_enumString += "       Count\n";
                    s_enumString += "      }\n}";
                    s_constString += "      };\n    }\n}";


                    WriteToFile(outPutString);
                    return;
                }

                if (args[i].Equals("-src") ||
                    args[i].Equals("-dataDes") ||
                    args[i].Equals("-output"))
                {
                    s_argsValueSetting.Add(args[i], Path.GetFullPath(args[i + 1]));
                    i++;
                }

                if (args[i].Equals("-keyType"))
                {
                    string keyType = args[i + 1];
                    if (keyType == "StrID")
                    {
                        s_keyType = 0;
                    }
                    else if (keyType == "OneID")
                    {
                        s_keyType = 1;
                    }
                    else if (keyType == "TwoID")
                    {
                        s_keyType = 2;
                    }
                    else if (keyType == "ThreeID")
                    {
                        s_keyType = 3;
                    }
                    else if (keyType == "TextID")
                    {
                        s_keyType = 4;
                    }
                }

                if (args[i].Equals("-md5file"))
                {
                    s_md5fileName = args[i + 1];
                }

                if (args[i].Equals("-prefix"))
                {
                    s_argsValueSetting.Add(args[i], args[i + 1]);
                    i++;
                }

                if (args[i].Equals("-dataName"))
                {
                    s_argsValueSetting.Add(args[i], args[i + 1]);
                    i++;
                }

                if (args[i].Equals("-structname"))
                {
                    s_argsValueSetting.Add(args[i], args[i + 1]);
                    i++;
                }

                if (args[i].Equals("-encoding"))
                {
                    if (args[i + 1].Equals("UnicodeBigUnmarked"))
                    {
                        s_binStrEncoding = Encoding.BigEndianUnicode;
                    }
                    else if (args[i + 1].Equals("UTF-8"))
                    {
                        s_binStrEncoding = Encoding.UTF8;
                    }
                    s_argsValueSetting.Add(args[i], args[i + 1]);
                    i++;
                }

                if (args[i].Equals("-txt"))
                {
                    s_argsValueSetting.Add(args[i], args[i + 1]);
                    i++;
                }

                if (args[i].Equals("-params") ||
                    args[i].Equals("-offset") ||
                    args[i].Equals("-id") ||
                    args[i].Equals("-id_dataType") ||
                    args[i].Equals("-cppheader") ||
                    args[i].Equals("-mergesheet") ||
                    args[i].Equals("-row_offset") ||
                    args[i].Equals("-all"))
                {
                    s_argsSetting.Add(args[i], true);
                }
            }

            if (!s_argsValueSetting.ContainsKey("-txt"))
            {
                s_argsValueSetting.Add("-txt", Path.GetFileNameWithoutExtension(s_argsValueSetting["-src"]) + ".txt");
            }

           
            //try
            {
                DataDescParser ddp = new DataDescParser(s_argsValueSetting["-dataDes"]);
                //string headStr = "struct Data_Invalid: public std::runtime_error{ Data_Invalid(const std::string& s):std::runtime_error(s){}};\n";
                /*string headStr = "#pragma once\n";
                headStr += "#include <string>\n";
                headStr += "#include \"cocos2d.h\"\n";
                headStr += "#include \"Archivesystem_i.h\"\n\n";*/

                /*string outputName = s_argsValueSetting["-output"];

                string cppStr = "#include \"" + Path.GetFileNameWithoutExtension(outputName) + ".h\"\n\n";
                
                int dotPos = outputName.LastIndexOf(".");
                if (dotPos != -1)
                {
                    outputName = outputName.Substring(0, outputName.LastIndexOf("."));
                }
                if (!Directory.Exists(Path.GetDirectoryName(outputName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(outputName));
                }*/

                if (s_argsValueSetting["-src"].EndsWith(".csv"))
                {
                    ExportCSV(ddp, ref outPutString);
                }
                else
                {
                    Export(ddp);
                }

               
                /*ExportCode(ddp, ref outPutString, ref cppStr);
                StreamWriter sw = new StreamWriter(outputName + ".h");
                sw.Write(headStr);
                sw.Close();
                sw = new StreamWriter(outputName + ".cpp");
                sw.Write(cppStr);
                sw.Close();
                if (s_md5fileName != null && File.Exists(s_argsValueSetting["-output"]))
                {//outputName
                    FileMode openWay = FileMode.Truncate;
                    if (!File.Exists(s_md5fileName))
                    {
                        openWay = FileMode.CreateNew;
                    }
                    FileStream fs = new FileStream(s_md5fileName, openWay);
                    BinaryWriter bw = new BinaryWriter(fs, Encoding.ASCII);
                    bw.Write(GetMd5Hash(s_argsValueSetting["-output"]).ToCharArray());
                    bw.Close();
                    fs.Close();
                }*/
            }

            //outPutString += "}";

            WriteToFile(outPutString);
//             catch (Exception errExport)
//             {
//                 ErrLog(errExport.StackTrace + "\n", ConsoleColor.Red);
//                 string errInfo = "Error Sheet:";
//                 if (s_lastSheetName != null)
//                 {
//                     errInfo += s_lastSheetName + "\nError Row:";
//                 }
//                 if (s_lastRow != -1)
//                 {
//                     errInfo += s_lastRow + "\nError Col:";
//                 }
//                 if (s_lastColName != null)
//                 {
//                     errInfo += s_lastColName + "\n";
//                 }
//                 ErrLog(errInfo + "\n", ConsoleColor.Red);
//             }
           

            if (s_xlsWorkbook != null) s_xlsWorkbook.Close(false);
        }

        static void WriteToFile(string outPutString, bool isAppend = true)
        {
            StreamWriter sw = new StreamWriter("export_code/GameData.cs", isAppend);
            sw.Write(outPutString);
            sw.Close();

            StreamWriter en = new StreamWriter("export_code/GameDataEnum.cs", isAppend);
            en.Write(s_enumString);
            en.Close();

            StreamWriter con = new StreamWriter("export_code/GameDataConst.cs", isAppend);
            con.Write(s_constString);
            con.Close();
        }

        static void ErrLog(string errStr, ConsoleColor errColor)
        {
            ConsoleColor tempColor = Console.ForegroundColor;
            Console.ForegroundColor = errColor;
            Console.Write(errStr);
            Console.ForegroundColor = tempColor;
        }

        static void ExportCSV(DataDescParser dataDesc, ref string outputString)
        {
            KeyValuePair<string, SheetDataDesc> kvpSheet = dataDesc.m_sheetDataDesc.First();
            string txt_export = "";
            string define_export = "";
            string srcFileName = s_argsValueSetting["-src"];
            string srcSheetName = Path.GetFileNameWithoutExtension(srcFileName);//sheet名称在csv中跟文件名一样
            StreamReader fileReader = null;
            try
            {
                fileReader = new StreamReader(srcFileName, Encoding.Default);
            }
            catch (System.Exception exx)
            {
                ErrLog("Cannot find " + srcFileName + " file!\n" , ConsoleColor.Red);
            }
            string exportFileAddr = s_argsValueSetting["-output"];
            FileMode openWay = FileMode.Truncate;
            if (!File.Exists(exportFileAddr))
            {
                openWay = FileMode.CreateNew;
            }
            FileStream fs = new FileStream(s_argsValueSetting["-output"], openWay);
            BinaryWriter bw = new BinaryWriter(fs, s_binStrEncoding);
            string[][] ls = CSVUtil.SplitCSV(fileReader.ReadToEnd());
            fileReader.Close();
            Dictionary<string, int> titleDict = new Dictionary<string, int>();
            for (int i = 0; i < ls[0].Length; i++)
            {
                if (ls[0][i] != null && !ls[0][i].Equals(""))
                {
                    titleDict.Add(ls[0][i], i);
                }
            }
            SheetDataDesc sdd = dataDesc.GetSheetDataDesc(srcSheetName);            
            
            if (sdd == null)
            {
                ErrLog("xml config file doesnt contain \"" + srcSheetName + "\" sheet key!\n", ConsoleColor.Red);
                return;
            }
            int[] title_offsets = new int[sdd.m_typeDesc.Count];
            string[] title_name = new string[sdd.m_typeDesc.Count];
            DataType[] title_type = new DataType[sdd.m_typeDesc.Count];
            enum_title = new string[sdd.m_typeDesc.Count];
            enum_str_macro = new string[sdd.m_typeDesc.Count];
            enum_str_macro_name = new string[sdd.m_typeDesc.Count];
            int count = 0;
            foreach (KeyValuePair<string, DataType> kvpDataDesc in sdd.m_typeDesc)
            {
                try
                {
                    title_offsets[count] = titleDict[kvpDataDesc.Key];
                }
                catch (Exception eeeeeeee)
                {
                    ErrLog("Open your PENIS eyes! Cannot find " + kvpDataDesc.Key + "!!!", ConsoleColor.Red);
                }
                
                title_name[count] = kvpDataDesc.Key;
                title_type[count] = kvpDataDesc.Value;
                count++;
            }

            // 导出描述
            bw.Write(Convert.ToInt16(count));
            for (int index = 0; index < count; index ++)
            {
                bw.Write(Convert.ToInt16(s_binStrEncoding.GetByteCount(title_name[index])));
                char[] chars = title_name[index].ToCharArray();
                bw.Write(chars);

                int nType = (int)title_type[index];
                bw.Write(Convert.ToInt16(nType));
            }

            bw.Write(Convert.ToInt16(s_keyType));

            string[][] enum_str = new string[title_offsets.Length][];
            int enumOff = -1;
            for (int i = 2; i < ls.Length; i++)
            {
                s_lastRow = i;
                int LineID = -1;
                int LineMacroID = -1;
                string LineMD5Val = "0";
                string LineMD5File = null;
                string LineDes = null;
                string LineMacro = null;
                //int j = 0;
                //foreach (KeyValuePair<string, DataType> kvpDataDesc in sdd.m_typeDesc)
                for (int j = 0; j < title_offsets.Length; j++ )
                {
                    //                     if (i == 2)
                    //                     {
                    //                         title_offsets[j] = titleDict[kvpDataDesc.Key];
                    //                     }
                    s_lastColName = title_name[j];// kvpDataDesc.Key;
                    string cellVal = ls[i][title_offsets[j]];
                    string valStr;
                    if (s_lastColName.Equals(sdd.m_keyTitleName))
                    {
                        if (cellVal.Equals("")) cellVal = "0";
                        try
                        {
                            LineMacroID = Convert.ToInt32(cellVal);
                        }
                        catch (Exception ead) { }                        
                    }
                    if (s_lastColName.Equals(sdd.m_MD5FileColName))
                    {
                        //if (cellVal.Equals("")) cellVal = "0";
                        try
                        {
                            LineMD5File = cellVal;
                            string filePathName = sdd.m_MD5FilePath + @"\" + LineMD5File;
                            if (File.Exists(filePathName))
                            {
                                LineMD5Val = GetMd5Hash(filePathName);
                            }
                            else
                            {
                                LineMD5Val = "0";
                            }
                        }
                        catch (Exception ead1) { } 
                    }

                    switch (title_type[j])
                    {
                        case DataType.int8:
                            {
                                if (cellVal.Equals("")) cellVal = "0";
                                int value = Convert.ToInt32(cellVal);

                                int writevalue;
                                try
                                {
                                    writevalue = testByte(value);
                                    if (value < 0)
                                    {
                                        bw.Write(Convert.ToSByte(value));
                                    }
                                    else
                                    {
                                        bw.Write(Convert.ToByte(value));
                                    }
                                }
                                catch (DataOverFlowException)
                                {
                                    MessageBox.Show(s_lastColName + " " + cellVal + " This has a value not Byte!!!!");
                                }
                            }
                            break;

                        case DataType.int16:
                            {
                                if (cellVal.Equals("")) cellVal = "0";
                                int valint = Convert.ToInt32(cellVal);

                                int writevalue1;
                                try
                                {
                                    writevalue1 = testShort(valint);
                                    bw.Write((short)(writevalue1));
                                }
                                catch (DataOverFlowException)
                                {
                                    MessageBox.Show(s_lastColName + " " + cellVal + " This has a value not Short!!!!");
                                }
                            }
                            break;

                        case DataType.ID:
                        case DataType.int32:
                            {
                                if (cellVal.Equals("")) cellVal = "0";
                                bw.Write(Convert.ToInt32(cellVal));
                            }
                            break;
                        case DataType.int64:
                            {
                                if (cellVal.Equals("")) cellVal = "0";
                                bw.Write(Convert.ToInt64(cellVal));
                            }
                            break;
                        case DataType.Float:
                            {
                                if (cellVal.Equals("")) cellVal = "0.0";
                                bw.Write(Convert.ToSingle(cellVal));
                            }
                            break;
                        case DataType.str:
                        case DataType.utf8str:
                        case DataType.int32Array:
                        case DataType.int32ArrayArray:
                            {
                                valStr = cellVal;
                                if (valStr == null || valStr.Equals(""))
                                {
                                    bw.Write((short)(0));
                                }
                                else
                                {
                                    if (valStr.Contains("\\n"))
                                    {
                                        valStr = valStr.Replace("\\n", "\n");
                                    }
                                    //bw.Write(Convert.ToInt16(s_binStrEncoding.GetByteCount(valStr) / 2));           
                                    bw.Write(Convert.ToInt16(s_binStrEncoding.GetByteCount(valStr)));
                                    char[] chars = valStr.ToCharArray();
                                    bw.Write(chars);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    //j++;
                }
                if (LineID != -1 && LineDes != null)
                {
                    txt_export += LineID + " = \"" + LineDes + "\"\n";
                }
                if (LineMacroID != -1 && LineMacro != null)
                {
                    if (define_export.Equals(""))
                    {
                        define_export += "#pragma once\n\n";
                    }
                    define_export += "#define\t" + (srcSheetName + "_" + LineMacro.Replace(' ', '_')).ToUpper() + "\t\t\t(" + LineMacroID + ")\n";
                }               
            }
            for (int j = 0; j < title_offsets.Length; j++)
            {
                if (enum_str_macro[j] != null)
                {
                    enum_str_macro[j] += "};\n";
                }
            }
            bw.Close();
            fs.Close();
            if (!txt_export.Equals(""))
            {
                string filepath = Path.GetDirectoryName(s_argsValueSetting["-output"]) + "\\" + s_argsValueSetting["-txt"];
                filepath.Trim();
                StreamWriter sw = new StreamWriter(filepath);
                sw.Write(txt_export);
                sw.Close();
            }
            if (!define_export.Equals(""))
            {
                string definename = s_argsValueSetting["-dataName"] + "Define.h";
                StreamWriter sw = new StreamWriter(Path.GetDirectoryName(s_argsValueSetting["-output"]) + "\\" + definename);
                sw.Write(define_export);
                sw.Close();
            }

            ExportCSCode(srcSheetName, title_name, title_type, ref outputString);
        }

        static string GetMd5Hash(string content, Encoding code)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider oMD5Hasher =
                new System.Security.Cryptography.MD5CryptoServiceProvider();
            //注意:这里选择的编码不同 所计算出来的MD5值也不同例如
            Byte[] aHashTable = oMD5Hasher.ComputeHash(code.GetBytes(content));
            return System.BitConverter.ToString(aHashTable).Replace("-", "").ToLower();
        }

        static string GetMd5Hash(string pathName)
        {
            string strResult = "";
            string strHashData = "";
            byte[] arrbytHashValue;

            System.IO.FileStream oFileStream = null;

            System.Security.Cryptography.MD5CryptoServiceProvider oMD5Hasher =
                new System.Security.Cryptography.MD5CryptoServiceProvider();

            try
            {
                oFileStream = new System.IO.FileStream(pathName, System.IO.FileMode.Open,
                                                       System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);

                //oMD5Hasher.ComputeHash()
                arrbytHashValue = oMD5Hasher.ComputeHash(oFileStream); //计算指定Stream 对象的哈希值

                oFileStream.Close();

                //由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”

                strHashData = System.BitConverter.ToString(arrbytHashValue);

                //替换-
                strHashData = strHashData.Replace("-", "");

                strResult = strHashData.ToLower();
            }

            catch (System.Exception ex)
            {
                ErrLog(ex.Message, ConsoleColor.Red);                
            }
            return strResult;
        }

        static void Export(DataDescParser dataDesc)
        {
            string txt_export = "";
            string define_export = "";
            s_xlsWorkbook = GetWorkbook(s_argsValueSetting["-src"]);

            foreach (Worksheet worksheet in s_xlsWorkbook.Sheets)
            {
                s_lastSheetName = worksheet.Name;

                SheetDataDesc sdd = dataDesc.GetSheetDataDesc(worksheet.Name);
                if (sdd == null)
                {
                    continue;
                }


                Dictionary<string, int> titleDict = new Dictionary<string, int>();
                for (int i = 1; i < worksheet.UsedRange.Columns.Count + 1; i++)
                {
                    string cellname = worksheet.Cells[2, i].Value;
                    if (cellname != null && !cellname.Equals(""))
                    {
                        titleDict.Add(worksheet.Cells[2, i].Value, i);
                    }
                }
                
                FileStream fs = new FileStream(s_argsValueSetting["-output"] + "_" + worksheet.Name + ".bin", FileMode.CreateNew);
                BinaryWriter bw = new BinaryWriter(fs, s_binStrEncoding);

                int[] title_offsets = new int[sdd.m_typeDesc.Count];
                string[] title_name = new string[sdd.m_typeDesc.Count];
                DataType[] title_type = new DataType[sdd.m_typeDesc.Count];
                int count = 0;
                foreach (KeyValuePair<string, DataType> kvpDataDesc in sdd.m_typeDesc)
                {
                    title_offsets[count] = titleDict[kvpDataDesc.Key];
                    title_name[count] = kvpDataDesc.Key;
                    title_type[count] = kvpDataDesc.Value;
                    count++;
                }
                //int[] title_offsets = new int[sdd.m_typeDesc.Count];

                // 导出描述
                bw.Write(Convert.ToInt16(count));
                for (int index = 0; index < count; index++)
                {
                    bw.Write(Convert.ToInt16(s_binStrEncoding.GetByteCount(title_name[index])));
                    char[] chars = title_name[index].ToCharArray();
                    bw.Write(chars);

                    int nType = (int)title_type[index];
                    bw.Write(Convert.ToInt16(nType));
                }

                for (int i = 3; i < worksheet.UsedRange.Rows.Count + 1; i++)
                {
                    s_lastRow = i;
                    int LineID = -1;
                    int LineMacroID = -1;
                    string LineDes = null;
                    string LineMacro = null;
                    string LineMD5Val = "0";
                    string LineMD5File = null;
                    //int j = 0;
                    //foreach (KeyValuePair<string, DataType> kvpDataDesc in sdd.m_typeDesc)
                    for (int j = 0; j < title_offsets.Length; j++ )
                    {
//                         if (i == 2)
//                         {
//                             title_offsets[j] = titleDict[kvpDataDesc.Key];
//                         }
                        s_lastColName = title_name[j];// kvpDataDesc.Key;                     
                        string valStr;
                        string cellVal = "";
                        if (worksheet.Cells[i, title_offsets[j]].Value != null)
                        {
                            cellVal = worksheet.Cells[i, title_offsets[j]].Value.ToString();
                        }
                        
                        if (s_lastColName.Equals(sdd.m_keyTitleName))
                        {
                            if (cellVal.Equals("")) cellVal = "0";
                            try
                            {
                                LineMacroID = Convert.ToInt32(cellVal);
                            }
                            catch (Exception ead) { }   
                        }
                        if (s_lastColName.Equals(sdd.m_MD5FileColName))
                        {
                            //if (cellVal.Equals("")) cellVal = "0";
                            try
                            {
                                LineMD5File = cellVal;
                                string filePathName = sdd.m_MD5FilePath + @"\" + LineMD5File;
                                if (File.Exists(filePathName))
                                {
                                    LineMD5Val = GetMd5Hash(filePathName);
                                }
                                else
                                {
                                    LineMD5Val = "0";
                                }
                            }
                            catch (Exception ead1) { }
                        }
                        switch (title_type[j])
                        {
                            case DataType.int8:
                                {
                                    if (cellVal.Equals("")) cellVal = "0";
                                    int value = Convert.ToInt32(cellVal);

                                    int writevalue;
                                    try
                                    {
                                        writevalue = testByte(value);
                                        if (value < 0)
                                        {
                                            bw.Write(Convert.ToSByte(value));
                                        }
                                        else
                                        {
                                            bw.Write(Convert.ToByte(value));
                                        }
                                    }
                                    catch (DataOverFlowException)
                                    {
                                        MessageBox.Show(s_lastColName + " " + cellVal + " This has a value not Byte!!!!");
                                    }
                                }
                                break;

                            case DataType.int16:
                                {
                                    if (cellVal.Equals("")) cellVal = "0";
                                    int valint = Convert.ToInt32(cellVal);

                                    int writevalue1;
                                    try
                                    {
                                        writevalue1 = testShort(valint);
                                        bw.Write((short)(writevalue1));
                                    }
                                    catch (DataOverFlowException)
                                    {
                                        MessageBox.Show(s_lastColName + " " + cellVal + " This has a value not Short!!!!");
                                    }
                                }
                                break;

                            case DataType.ID:
                            case DataType.int32:
                                {
                                    if (cellVal.Equals("")) cellVal = "0";
                                    bw.Write(Convert.ToInt32(cellVal));
                                }
                                break;
                            case DataType.int64:
                                {
                                    if (cellVal.Equals("")) cellVal = "0";
                                    bw.Write(Convert.ToInt64(cellVal));
                                }
                                break;
                            case DataType.Float:
                                {
                                    if (cellVal.Equals("")) cellVal = "0.0";
                                    bw.Write(Convert.ToSingle(cellVal));
                                }
                                break;
                            case DataType.str:
                                {
                                    valStr = cellVal;
                                    if (valStr == null || valStr.Equals(""))
                                    {
                                        bw.Write((short)(0));
                                    }
                                    else
                                    {
                                        if (valStr.Contains("\\n"))
                                        {
                                            valStr = valStr.Replace("\\n", "\n");
                                        }
                                        //bw.Write(Convert.ToInt16(s_binStrEncoding.GetByteCount(valStr) / 2));           
                                        bw.Write(Convert.ToInt16(s_binStrEncoding.GetByteCount(valStr)));
                                        char[] chars = valStr.ToCharArray();
                                        bw.Write(chars);
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                        //j++;
                    }
                    if (LineID != -1 && LineDes != null)
                    {
                        txt_export += LineID + " = \"" + LineDes + "\"\n";
                    }
                    if (LineMacroID != -1 && LineMacro != null)
                    {
                        if (define_export.Equals(""))
                        {
                            define_export += "#pragma once\n\n";
                        }
                        define_export += "#define\t" + (s_lastSheetName + "_" + LineMacro.Replace(' ', '_')).ToUpper() + "\t\t\t(" + LineMacroID + ")\n";
                    }
                }

                bw.Close();
                fs.Close();
            }
         
            if (!txt_export.Equals(""))
            {
                StreamWriter sw = new StreamWriter(Path.GetDirectoryName(s_argsValueSetting["-output"]) + "\\" + s_argsValueSetting["-txt"]);
                sw.Write(txt_export);
                sw.Close();
            }
            if (!define_export.Equals(""))
            {
                string definename = s_argsValueSetting["-dataName"] + "Define.h";
                StreamWriter sw = new StreamWriter(Path.GetDirectoryName(s_argsValueSetting["-output"]) + "\\" + definename);
                sw.Write(define_export);
                sw.Close();
            }
        }

        static void ExportCode(DataDescParser dataDesc, ref string headStr, ref string cppStr)
        {
            KeyValuePair<string, SheetDataDesc> kvpSheet = dataDesc.m_sheetDataDesc.First();
            string dataname = s_argsValueSetting["-dataName"];
            if (s_argsValueSetting.ContainsKey("-structname"))
            {
                dataname = s_argsValueSetting["-structname"];
            }

            for(int i = 0; i < enum_str_macro.Length; i++)
            {
                if(enum_str_macro[i] != null)
                {
                    headStr += enum_str_macro[i] + "\n";
                }
            }

            headStr += "class numeric" + dataname + " : public cocos2d::Ref" + "\n{\npublic:\n";
            headStr += "    numeric" + dataname + "();\n";
            headStr += "    virtual ~numeric" + dataname + "();\n\n";
            headStr += "    CREATE_FUNC(" + "numeric" + dataname + ");\n\n";
            headStr += "    void Decode(IArchive* pArchive);\n\n";

            string constructStr = "numeric" + dataname + "::" + "numeric" + dataname + "()\n{\n";
            string deconstructStr = "numeric" + dataname + "::" + "~" + "numeric" + dataname + "()\n{\n";

            cppStr += "void numeric" + dataname + "::Decode(IArchive* pArchive)\n{\n    short tempLen = 0;\n";

            int off = 0;

            foreach (KeyValuePair<string, string> kvpDataDesc in kvpSheet.Value.m_nameDesc)
            {
                switch (kvpSheet.Value.m_typeDesc[kvpDataDesc.Key])
                {
                    case DataType.int16:               
                        headStr += "    short ";
                        cppStr += "    pArchive->Read(" + kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + ");\n";
                        headStr += kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + ";\n";

                        headStr += "    short ";
                        headStr += kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + "_get(){return " + kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + ";}\n";
                        break;
                    case DataType.int32:
                    case DataType.ID:
                        headStr += "    int ";
                        cppStr += "    pArchive->Read(" + kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + ");\n";
                        headStr += kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + ";\n";

                        headStr += "    int ";
                        headStr += kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + "_get(){return " + kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + ";}\n";
                        break;
                    case DataType.int8:
                        headStr += "    char ";
                        cppStr += "    pArchive->Read(" + kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + ");\n";
                        headStr += kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + ";\n";

                        headStr += "    char ";
                        headStr += kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + "_get(){return " + kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + ";}\n";
                        break;
                    case DataType.str:
                        headStr += "    std::string* ";
                        cppStr += "    tempLen = 0;\n";
                        cppStr += "    pArchive->Read(tempLen);\n";
                        cppStr += "    " + kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + " = new std::string();\n";
                        cppStr += "    char tempChar" + kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + ";\n";
                        cppStr += "    for(int i = 0; i < tempLen; i++)\n    {\n";
                        cppStr += "        pArchive->Read(tempChar" + kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + ");\n";
                        cppStr += "        " + kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + "->append(1, tempChar" + kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + ");\n    }\n";

                        constructStr += "    " + kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + " = NULL;\n";
                        deconstructStr += "    if(" + kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + ")\n    {\n";
                        deconstructStr += "        delete " + kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + ";\n";
                        deconstructStr += "        " + kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + " = NULL;\n";
                        deconstructStr += "    }\n";
                        headStr += kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + ";\n";

                        headStr += "    const char* ";
                        headStr += kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + "_get(){return " + kvpSheet.Value.m_nameDesc[kvpDataDesc.Key] + "->c_str();}\n";          
                        break;
                    default:
                        break;
                }
                off++;
            }

            cppStr += "}\n\n";
            constructStr += "}\n\n";
            deconstructStr += "}\n\n";
            cppStr += constructStr + deconstructStr;

            headStr += "};\n\n";

            return;
        }

        static void ExportCSCode(string dataname, string[] titleName, DataType[] tileType, ref string outStr)
        {
            dataname += "Data";

            s_enumString += "       " + dataname + ",\n";
            s_constString += "       \"" + dataname + "\",\n";
            outStr += " public class " + dataname + " : GameData" + "\n{\n";

            string constructStr = "\n     public " + dataname + "(BinaryReader reader, List<GameDataValue> keyTypes) : base(reader, keyTypes)\n        {\n";
            
            for (int i = 0; i < tileType.Length; i++ )
            {
                switch (tileType[i])
                {
                    case DataType.int16:
                        outStr += "     public readonly short " + titleName[i] + ";\n";
                        constructStr += "         if (keyValuePairs.ContainsKey(\"" + titleName[i] + "\"))\n";
                        constructStr += "           " + titleName[i] + " = (int)keyValuePairs[\"" + titleName[i]  + "\"];\n";
                        break;
                    case DataType.int64:
                        outStr += "     public readonly long " + titleName[i] + ";\n";
                        constructStr += "         if (keyValuePairs.ContainsKey(\"" + titleName[i] + "\"))\n";
                        constructStr += "           " + titleName[i] + " = (long)keyValuePairs[\"" + titleName[i] + "\"];\n";
                        break;
                    case DataType.int32:
                    case DataType.ID:
                        outStr += "     public readonly int " + titleName[i] + ";\n";
                        constructStr += "         if (keyValuePairs.ContainsKey(\"" + titleName[i] + "\"))\n";
                        constructStr += "           " + titleName[i] + " = (int)keyValuePairs[\"" + titleName[i] + "\"];\n";
                        break;
                    case DataType.int8:
                        outStr += "     public readonly byte " + titleName[i] + ";\n";
                        constructStr += "         if (keyValuePairs.ContainsKey(\"" + titleName[i] + "\"))\n";
                        constructStr += "           " + titleName[i] + " = (byte)keyValuePairs[\"" + titleName[i] + "\"];\n";
                        break;
                    case DataType.Float:
                        outStr += "     public readonly float " + titleName[i] + ";\n";
                        constructStr += "         if (keyValuePairs.ContainsKey(\"" + titleName[i] + "\"))\n";
                        constructStr += "           " + titleName[i] + " = (float)keyValuePairs[\"" + titleName[i] + "\"];\n";
                        break;
                    case DataType.str:
                        outStr += "     public readonly string " + titleName[i] + ";\n";
                        constructStr += "         if (keyValuePairs.ContainsKey(\"" + titleName[i] + "\"))\n";
                        constructStr += "           " + titleName[i] + " = (string)keyValuePairs[\"" + titleName[i] + "\"];\n";
                        break;
                    case DataType.int32Array:
                        outStr += "     public readonly int[] " + titleName[i] + ";\n";
                        constructStr += "         if (keyValuePairs.ContainsKey(\"" + titleName[i] + "\"))\n";
                        constructStr += "           " + titleName[i] + " = (int[])keyValuePairs[\"" + titleName[i] + "\"];\n";
                        break;
                    case DataType.int32ArrayArray:
                        outStr += "     public readonly int[][] " + titleName[i] + ";\n";
                        constructStr += "         if (keyValuePairs.ContainsKey(\"" + titleName[i] + "\"))\n";
                        constructStr += "           " + titleName[i] + " = (int[][])keyValuePairs[\"" + titleName[i] + "\"];\n";
                        break;
                    case DataType.utf8str:
                        outStr += "     public readonly string " + titleName[i] + ";\n";
                        constructStr += "         if (keyValuePairs.ContainsKey(\"" + titleName[i] + "\"))\n";
                        constructStr += "           " + titleName[i] + " = (string)keyValuePairs[\"" + titleName[i] + "\"];\n";
                        break;
                    default:
                        break;
                }
            }

            constructStr += "\n         keyValuePairs.Clear();\n";

            constructStr += "       }\n\n";
            outStr += constructStr;

            if (s_keyType == 0)
            {
                outStr += "         public override string makeStrKey() {return ID;}\n";
            }
            else if (s_keyType == 1)
            {
                outStr += "         public override int makeIntKey() {return ID;}\n";
            }
            else if (s_keyType == 2)
            {
                outStr += "         public override string makeStrKey() {return StringUtility.MakeStrKey(ID, ID1);}\n";
            }
            else if (s_keyType == 3)
            {
                outStr += "         public override string makeStrKey() {return StringUtility.MakeStrKey(ID, ID1, ID2);}\n";
            }
            else if (s_keyType == 4)
            {
                outStr += "         public override string makeStrKey() {return StringUtility.MakeTextKey(ID, ID1);}\n";
            }

            outStr += "\n";

            if (s_keyType == 1)
            {
                outStr += "         public override void setMaxIntKey(ref int maxID) { if (ID > maxID) { maxID = ID; }}\n";
            }
            else if (s_keyType == 2)
            {
                outStr += "         public override void setMaxIntKeyDic(Dictionary<int, int> maxIDDic) { if (maxIDDic.ContainsKey(ID)) { if (ID1 > maxIDDic[ID]) { maxIDDic[ID] = ID1; }} else { maxIDDic[ID] = ID1; }}\n";
            }

            outStr += " };\n\n";

            return;
        }

        static string GetStringValue(string key)
        {
            string retval = null;
            try
            {
                retval = s_argsValueSetting[key];
            }
            catch (System.Exception ex)
            {
            }
            return retval;
        }

        static bool GetSetting(string key)
        {
            bool retval = false;
            try
            {
                retval = s_argsSetting[key];
            }
            catch (System.Exception ex)
            {
            }
            return retval;
        }

        public static int AddFlag(int value, int flag)
        {
            return (flag | value);
        }

        public static int RemoveFlag(int value, int flag)
        {
            return ((~flag) & (value));
        }

        public static bool CheckFlag(int value, int flag)
        {
            return ((flag & value) != 0);
        }

        public static int SetFlag(int value, int flag, bool enable)
        {
            if (enable)
            {
                return (AddFlag(value, flag));
            }
            else
            {
                return (RemoveFlag(value, flag));
            }
        }

        public static int wr_byte(byte[] array, int offset, int b)
        {
            array[offset] = (byte)((sbyte)b);
            return 1;
        }

        public static int wr_2valuesIn3bytes(byte[] array, int offset, int value1, int value2)
        {
            array[offset] = (byte)(value1 & 0xff);
            array[offset + 1] = (byte)((value1 & 0xf00 >> 4) | (value2 & 0xf00 >> 8));
            array[offset + 2] = (byte)(value2 & 0xff);
            return 3;
        }

        public static int wr_short(byte[] array, int offset, int s)
        {
            array[offset] = (byte)((Int16)s & 0xff);
            array[offset + 1] = (byte)(((Int16)s & 0xff00) >> 8);
            return 2;
        }

        public static int wr_int(byte[] array, int offset, int i)
        {
            array[offset] = (byte)(i & 0xff);
            array[offset + 1] = (byte)((i & 0xff00) >> 8);
            array[offset + 2] = (byte)((i & 0xff0000) >> 16);
            array[offset + 3] = (byte)((i & 0xff000000) >> 24);
            return 4;
        }

        public static int wr_int_3bytes(byte[] array, int offset, int i)
        {
            array[offset] = (byte)(i & 0xff);
            array[offset + 1] = (byte)((i & 0xff00) >> 8);
            array[offset + 2] = (byte)((i & 0xff0000) >> 16);
            return 3;
        }

        /*******************************About File******************************/
        public static string getFullPathFrom(string FromPath, string RelatedPath)
        {
            string org_path = Directory.GetCurrentDirectory();
            if (Directory.Exists(FromPath))
            {
                Directory.SetCurrentDirectory(FromPath);
            }
            if (File.Exists(FromPath))
            {
                Directory.SetCurrentDirectory(Path.GetDirectoryName(FromPath));
            }
            string retval = "";
            retval = Path.GetFullPath(RelatedPath);
            Directory.SetCurrentDirectory(org_path);
            return retval;
        }

        public static string getRelatedPathFrom(string FromPath, string RealPath)
        {
            if (Path.IsPathRooted(RealPath))
            {
                string a = FromPath.ToLower();
                if (File.Exists(FromPath))
                {
                    try
                    {
                        a = Path.GetDirectoryName(FromPath).ToLower();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                string b = Path.GetFullPath(RealPath).ToLower();
                int i = 0;
                for (; i < Math.Min(a.Length, b.Length); i++)
                {
                    if (a[i] != b[i]) break;
                }

                string cur = Regex.Replace(a.Substring(i - 1), @"\\?[a-zA-Z]+:?", @"..\");
                if (!a.Substring(i - 1).Contains("\\"))
                {
                    cur = "";
                }
                if (!cur.Contains("..\\")) cur = ".\\" + cur;
                return (cur + b.Substring(i)).Replace(@"\\", @"\");
            }
            else
            {
                return RealPath;
            }
        }

        static Workbook GetWorkbook(string xlsName)
        {
            if (s_xlsApp == null)
            {
                try
                {
                    s_xlsApp = new Microsoft.Office.Interop.Excel.Application();
                }
                catch (Exception e)
                {
                    ErrLog("Did you forget installing Office? error:" + e,ConsoleColor.Red);
                }                
            }
            Workbook wbook = s_xlsApp.Workbooks.Open(xlsName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            return wbook;
        }
    }
}
