using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;

namespace EntrepriseApplicationServer
{
    public class ParseStream
    {
        public static List<string> ParseStreamToString(StreamReader s)
        {
            List<string> resultParsing = new List<string>();
            bool isInParenthesis = false;

            string temp = s.ReadToEnd();
            string jsonItem = "";
            foreach (var c in temp)
            {
                if (c == '"')
                    isInParenthesis = !isInParenthesis;
                if (!isInParenthesis && c == '}')
                {
                    jsonItem = jsonItem.Insert(jsonItem.Length, new string(c, 1));
                    resultParsing.Add(String.Copy(jsonItem));
                    //MessageBox.Show("In ParseStream[" + jsonItem + "]");
                    jsonItem = "";
                }
                else
                {
                    jsonItem = jsonItem.Insert(jsonItem.Length, new string(c, 1));
                }
            }
            return resultParsing;
        }

        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
