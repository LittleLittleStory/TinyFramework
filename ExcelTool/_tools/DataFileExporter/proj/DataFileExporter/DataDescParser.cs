using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace DataFileExporter
{
    enum DataType
    {
        int8 = 0,
        int16 ,
        int32 ,
        str,
        ID,
        int64,
        Float,
        int32Array,
        int32ArrayArray,
        utf8str,
        unsupported,
    }

    class LinkData
    {
        public string m_path;
        public string m_key;
        public string m_value;
        public DataType m_dataType;
    }

    class SheetDataDesc
    {
        public Dictionary<string, string> m_nameDesc = null;
        public Dictionary<string, DataType> m_typeDesc = null;
        public Dictionary<string, LinkData> m_linkDesc = null;
        public string m_sheetName = null;
        public string m_keyTitleName = null;
        public string m_MD5ColName = null;//MD5码列名
        public string m_MD5FilePath = null;
        public string m_MD5FileColName = null;//MD5码文件名
        public SheetDataDesc()
        {
            m_nameDesc = new Dictionary<string, string>();
            m_typeDesc = new Dictionary<string, DataType>();
        }
    }

    class DataDescParser
    {
        private XmlDocument m_xmlFile = null;
        private bool m_isClientData = false;
        //public string m_DataDescName = null;
        public Dictionary<string, SheetDataDesc> m_sheetDataDesc = new Dictionary<string, SheetDataDesc>();

        public static Dictionary<string, DataType> m_dicDataType = null;// new Dictionary<string, DataType>();
        

        //public Dictionary<string, string> m_nameDesc = new Dictionary<string, string>();
        //public Dictionary<string, DataType> m_typeDesc = new Dictionary<string, DataType>();

        public DataDescParser(string filepath)
        {
            if (m_dicDataType == null)
            {
                m_dicDataType = new Dictionary<string, DataType>();
                m_dicDataType["int"] = DataType.int32;
                m_dicDataType["short"] = DataType.int16;
                m_dicDataType["id"] = DataType.ID;
                m_dicDataType["byte"] = DataType.int8;
                m_dicDataType["string"] = DataType.str;
                m_dicDataType["int64"] = DataType.int64;
                m_dicDataType["float"] = DataType.Float;
                m_dicDataType["int32array"] = DataType.int32Array;
                m_dicDataType["int32arrayarray"] = DataType.int32ArrayArray;
                m_dicDataType["utf8string"] = DataType.utf8str;
            }
            

            m_xmlFile = new XmlDocument();
            m_xmlFile.Load(filepath);

            XmlNode rootnode = m_xmlFile.SelectSingleNode("client");
            if (rootnode != null)
            {
                IsClient = true;
            }
            else
            {
                rootnode = m_xmlFile.SelectSingleNode("server");
            }            
            foreach (XmlNode xmlnode in rootnode.ChildNodes)
            {
                if (xmlnode.Name.Equals("Version")) continue;
                SheetDataDesc sheetData = new SheetDataDesc();
                sheetData.m_sheetName = xmlnode.Name;
                m_sheetDataDesc.Add(xmlnode.Name, sheetData);                
                foreach (XmlNode dataNode in xmlnode.ChildNodes)
                {
                    string attrTypeVal = dataNode.Attributes["DataType"].Value;
                    string attrNameVal = dataNode.Attributes["excelName"].Value;
                    try
                    {
                        if (dataNode.Attributes["keyID"].Value.Equals("true"))
                        {
                            sheetData.m_keyTitleName = attrNameVal;
                        }                        
                    }
                    catch (System.Exception exasdf)
                    {
                    }
                    try
                    {
                        sheetData.m_MD5FilePath = dataNode.Attributes["MD5Path"].Value;
                        sheetData.m_MD5FilePath = Path.GetDirectoryName(filepath) + @"\" + sheetData.m_MD5FilePath;
                        sheetData.m_MD5ColName = attrNameVal;
                    }
                    catch (System.Exception ex12)
                    {                    	
                    }
                    try
                    {
                        if (dataNode.Attributes["MD5FileName"].Value.Equals("true"))
                        {
                            sheetData.m_MD5FileColName = attrNameVal; 
                        }   
                    }
                    catch (System.Exception ex13)
                    {                        
                    }
                    attrTypeVal = attrTypeVal.ToLower();
                    //sheetData.m_nameDesc.Add(attrNameVal, dataNode.Attributes["xmlName"].Value);

                    bool isSupported = false;
                    foreach (KeyValuePair<string, DataType> pair in m_dicDataType)
                    {
                        if (attrTypeVal.Equals(pair.Key))
                        {
                            isSupported = true;
                            sheetData.m_typeDesc.Add(attrNameVal, pair.Value);
                        }
                    }
                    if (attrTypeVal.Equals("link"))
                    {
//                         sheetData.m_typeDesc.Add(attrNameVal, DataType.link);
//                         sheetData.m_linkDesc = new Dictionary<string, LinkData>();
//                         LinkData linkD = new LinkData();
//                         linkD.m_path = dataNode.Attributes["LinkTable"].Value;
//                         linkD.m_
//                         //linkD.m_dataType = 
//                         sheetData.m_linkDesc.Add(attrNameVal, linkD);

                        //[sheetData.m_nameDesc] = new LinkData();
                        //dataNode.Attributes["LinkTable"].Value;
                    }
                    if (!isSupported)
                    {
                        sheetData.m_typeDesc.Add(dataNode.Attributes["excelName"].Value, DataType.unsupported);
                    }                        
                }
            }
        }

        public bool IsClient
        {
            get { return m_isClientData; }
            set { m_isClientData = value; }
        }

        public SheetDataDesc GetSheetDataDesc(string name)
        {
            SheetDataDesc retval = null;
            try
            {
                retval = m_sheetDataDesc[name];
            }
            catch (System.Exception ex)
            {            	
            }
            return retval;
        }
    }
}
