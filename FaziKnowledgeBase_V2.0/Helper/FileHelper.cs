using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaziKnowledgeBase_V2._0.Helper
{
    public class FileHelper
    {
        public static string CheckFileFormat(string fileName)
        {
            string FileFormat = "";
            for (int i = 0; i < fileName.Length; i++)
            {
                if (fileName[i] == '.')
                {
                    for (int j = i + 1; j < fileName.Length; j++)
                    {
                        FileFormat += fileName[j];
                    }
                    break;
                }
            }
            return FileFormat;
        }
    }
}