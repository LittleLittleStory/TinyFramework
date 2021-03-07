using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataFileExporter
{
    class CSVUtil
    {
        private CSVUtil() 
        { 
        } 

        /// <summary> 
        /// 分割 CVS 文件内容为一个二维数组。 
        /// </summary> 
        /// <param name="src">CVS 文件内容字符串</param> 
        /// <returns>二维数组。String[line count][column count]</returns> 
        public static String[][] SplitCSV(String src) { 
        // 如果输入为空，返回 0 长度字符串数组 
            if (src==null || src.Length == 0) return new String[0][]{}; 
            String st=""; 
            System.Collections.ArrayList lines = new System.Collections.ArrayList(); // 行集合。其元素为行 
            System.Collections.ArrayList cells = new System.Collections.ArrayList(); // 单元格集合。其元素为一个单元格 
            bool beginWithQuote = false; 
            int maxColumns = 0; 
            // 遍历字符串的字符 
            for (int i=0;i<src.Length;i++){ 
                char ch = src[i]; 

                                                                                                                                                                                                                    #region CR 或者 LF 
            //A record separator may consist of a line feed (ASCII/LF=0x0A), 
            //or a carriage return and line feed pair (ASCII/CRLF=0x0D 0x0A). 
            // 这里我不明白CR为什么不作为separator呢，在Mac OS上好像是用CR的吧。 
            // 这里我“容错”一下，CRLF、LFCR、CR、LF都作为separator 
            if (ch == '\r') { 
                #region CR 
                if (beginWithQuote) { 
                    st += ch; 
                } 
                else { 
                    if(i+1 < src.Length && src[i+1] == '\n') { // 如果紧接的是LF，那么直接把LF吃掉 
                        i++; 
                    } 

                    //line = new String[cells.Count]; 
                    //System.Array.Copy (cells.ToArray(typeof(String)), line, line.Length); 
                    //lines.Add(line); // 把上一行放到行集合中去 

                    cells.Add(st); 
                    st = ""; 
                    beginWithQuote = false; 

                    maxColumns = (cells.Count > maxColumns ? cells.Count : maxColumns); 
                    if(!cells[0].ToString().StartsWith("##")) lines.Add(cells); 
                    st = ""; 
                    cells = new System.Collections.ArrayList(); 
                } 
                #endregion CR 
            } 
            else if (ch == '\n') { 
                #region LF 
                if (beginWithQuote) { 
                    st += ch; 
                } 
                else { 
                    if(i+1 < src.Length && src[i+1] == '\r') { // 如果紧接的是LF，那么直接把LF吃掉 
                        i++; 
                    } 

                    //line = new String[cells.Count]; 
                    //System.Array.Copy (cells.ToArray(typeof(String)), line, line.Length); 
                    //lines.Add(line); // 把上一行放到行集合中去 

                    cells.Add(st); 
                    st = ""; 
                    beginWithQuote = false; 

                    maxColumns = (cells.Count > maxColumns ? cells.Count : maxColumns);
                    if (!cells[0].ToString().StartsWith("##")) lines.Add(cells); 
                    st = ""; 
                    cells = new System.Collections.ArrayList(); 
                } 
                #endregion LF 
            } 
            #endregion CR 或者 LF 
                else if (ch == '\"'){ // 双引号 
                    #region 双引号 
                    if (beginWithQuote){ 
                        i++; 
                        if (i>=src.Length){ 
                            cells.Add(st); 
                            st=""; 
                            beginWithQuote=false; 
                        } 
                        else{ 
                            ch=src[i]; 
                            if (ch == '\"'){ 
                                st += ch; 
                            } 
                            else if (ch == ','){ 
                                cells.Add(st);
                                st = ""; 
                                beginWithQuote = false; 
                            }
                            else if (ch == '\r' || ch == '\n')
                            {
                                //st = ""; 
                                beginWithQuote = false; 
                                //do nothing
                            }
                            else{ 
                                throw new Exception("Single double-quote char mustn't exist in filed "+(cells.Count+1)+" while it is begined with quote\nchar at:"+i); 
                            } 
                        } 
                    } 
                    else if (st.Length==0){ 
                        beginWithQuote = true; 
                    } 
                    else{ 
                        throw new Exception("Quote cannot exist in a filed which doesn't begin with quote!\nfield:"+(cells.Count+1)); 
                    } 
                    #endregion 双引号 
                } 
                else if (ch==','){ 
                    #region 逗号 
                    if (beginWithQuote){ 
                        st += ch; 
                    } 
                    else{ 
                        cells.Add(st); 
                        st = ""; 
                        beginWithQuote = false; 
                    } 
                    #endregion 逗号 
                } 
                else{ 
                    #region 其它字符 
                    st += ch; 
                    #endregion 其它字符 
                } 

            } 
            if (st.Length != 0){ 
                if (beginWithQuote){ 
                    throw new Exception("last field is begin with but not end with double quote"); 
                } 
                else{ 
                    cells.Add(st); 
                    maxColumns = (cells.Count > maxColumns ? cells.Count : maxColumns); 
                    lines.Add(cells); 
                } 
            } 

            String[][] ret = new String[lines.Count][]; 
            for (int i = 0; i < ret.Length; i++) { 
                cells = (System.Collections.ArrayList) lines[i]; 
                ret[i] = new String[maxColumns]; 
                for (int j = 0; j < maxColumns; j++) { 
                    ret[i][j] = cells[j].ToString(); 
                } 
            } 
            //System.Array.Copy(lines.ToArray(typeof(String[])), ret, ret.Length); 
            return ret; 
        } 
    }
}
