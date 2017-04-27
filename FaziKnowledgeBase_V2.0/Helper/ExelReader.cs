using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;
using FuzzyKnowledgeBase_V2._0.Models;
using FaziKnowledgeBase_V2._0.FKB.DataStructures;

namespace FaziKnowledgeBase_V2._0.Helper
{
    public class ExelReader
    {
        public static int countColumnData = 0, counterFoRowDataFromFile = 0, ClusterCount;
        public static List<string> NameOfLinguisticVariables = new List<string>();
        public static double[,] ElementsMatrix;
        public static List<MultiDimensionalVector> ElementsMulti = new List<MultiDimensionalVector>();
        public static void ReadFromXLS(string path) // Function for reading data from the file .xls
        {
            HSSFWorkbook hssfwb;

            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))  // Data2
            {
                hssfwb = new HSSFWorkbook(file);
            }
            ISheet sheet = hssfwb.GetSheet("FirstList");
            List<string> Elements = new List<string>();
            int column = 1, Row = 1;
            countColumnData = 0;
            Elements.Clear();

            for (column = 1; sheet.GetRow(0).GetCell(column) != null; column++) // подсчет количества колонок в файле, а также запись названия ЛП
            {
                List<Term> t = new List<Term>();
                NameOfLinguisticVariables.Add(string.Format("{0: 0.0}", sheet.GetRow(0).GetCell(column)));
                LinguisticVariable LP = new LinguisticVariable(new Guid(), string.Format("{0: 0.0}", sheet.GetRow(0).GetCell(column)), t, 0, 1);
                //FKB.ListVar.Add(LP);
                countColumnData += 1;
            }

            if (counterFoRowDataFromFile == 0)
            {
                for (Row = 1; sheet.GetRow(Row) != null && sheet.GetRow(Row).GetCell(0) != null; Row++)  // подсчет количества строк в файле
                {
                    counterFoRowDataFromFile++;
                }
            }
            column = 1;
            ElementsMatrix = new double[counterFoRowDataFromFile, countColumnData];
            for (int row = 1; row <= counterFoRowDataFromFile; row++)  // запись построчно с файла данных в список ElementsMulti -MultiDimensionalVector-
            {
                MultiDimensionalVector h = new MultiDimensionalVector();
                while (sheet.GetRow(row).GetCell(column) != null)
                {
                    Elements.Add(string.Format("{0: 0.0}", sheet.GetRow(row).GetCell(column)));
                    column += 1;
                }
                List<double> result = Elements.Select(x => double.Parse(x)).ToList();
                int integer = 0;
                foreach (double x in result)
                {
                    var newVector = x;
                    h.Add(newVector);
                    ElementsMatrix[row - 1, integer] = x;
                    integer++;
                }
                ElementsMulti.Add(h);
                column = 1;
                Elements.Clear();
            }
            ClusterCount = (counterFoRowDataFromFile / 2) + 3;
            if (ClusterCount > 10)
            {
                ClusterCount = 7;
            }
        }
    }
}