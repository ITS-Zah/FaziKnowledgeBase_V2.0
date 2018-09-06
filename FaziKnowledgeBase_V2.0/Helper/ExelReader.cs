using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;
using FuzzyKnowledgeBase_V2._0.Models;
using FaziKnowledgeBase_V2._0.FKB.DataStructures;
using FaziKnowledgeBase_V2._0.Models.C_Means_Clustering;                    

namespace FaziKnowledgeBase_V2._0.Helper
{
    public class ExelReader
    {
        public static int countColumnData = 0, counterFoRowDataFromFile = 0, NumbersOfZonesOneLP = 0, RecommendCountOfMaxClusterCount = 0, ClusterCount;
        public static List<string> NameOfLinguisticVariables = new List<string>();
        public static double[,] ElementsMatrix;
        public double[,] MatrixOfTheElements;
        public  List<MultiDimensionalVector> ElementsMulti = new List<MultiDimensionalVector>();
        public static double[,] FindMinMaxValueTerm;
        public static double[,] ValueIntervalTerm;
        public static double[,] ValueTermVithoutRepeat;                                                                             
        public static int ostanovkaLP = 0;
        public static int ostanovkaTM = 0;
        public static List<string> NameOfTerms = new List<string>();
        public static List<int> WeightOfTerms = new List<int>();
        public const int NUMBER_OF_CLUSTERS = 3; 

        public void ReadFromXLS(string path) // Function for reading data from the file .xls
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
        public static void ProcessedDataFromFile(List<Cluster> Clusters)  // найти термы, возможные их значения 
        {
           

            int CheckerCount;  // Логическая проверка
            if (ClusterCount > countColumnData)
            {
                CheckerCount = ClusterCount;
            }
            else
            {
                CheckerCount = countColumnData;
            }
            FindMinMaxValueTerm = new double[CheckerCount, 2];  /// ClusterCount -- countColumnData что большее 
            ValueIntervalTerm = new double[CheckerCount, NumbersOfZonesOneLP];  /// countColumnData --- ClusterCount

            //NameOfTermsByWords = new string[counterFoRowDataFromFile, countColumnData];
            //NameOfTermsByWordsWhithoutRepeat = new string[counterFoRowDataFromFile, countColumnData];
            ValueTermVithoutRepeat = new double[CheckerCount, countColumnData];
            int column = 1;
            double MaxValTerm = Clusters.ElementAt(0).Centroid.ElementAt(0), MinValTerm = Clusters.ElementAt(0).Centroid.ElementAt(0);
            //Clusters.ElementAt(i).Centroid.ElementAt(j)
            for (column = 0; column < countColumnData; column++) // найти мин и макс элемент одного ЛП (сравнения значений по колонкам)
            {
                MaxValTerm = Clusters.ElementAt(0).Centroid.ElementAt(column);
                MinValTerm = Clusters.ElementAt(0).Centroid.ElementAt(column);
                for (int rown = 0; rown < ClusterCount; rown++)
                {
                    if (Clusters.ElementAt(rown).Centroid.ElementAt(column) > MaxValTerm)
                    {
                        MaxValTerm = Clusters.ElementAt(rown).Centroid.ElementAt(column);
                    }
                    if (Clusters.ElementAt(rown).Centroid.ElementAt(column) < MinValTerm)
                    {
                        MinValTerm = Clusters.ElementAt(rown).Centroid.ElementAt(column);
                    }
                }
                FindMinMaxValueTerm[column, 0] = MinValTerm;
                FindMinMaxValueTerm[column, 1] = MaxValTerm;
            }

            double interval = 0;
            for (int i = 0; i < CheckerCount; i++)  // разбиение на промежутки значений. Нахождения возможных зон для значений термов одной ЛП 
            {
                interval = (FindMinMaxValueTerm[i, 1] - FindMinMaxValueTerm[i, 0]) / (NumbersOfZonesOneLP - 1);
                ValueIntervalTerm[i, 0] = FindMinMaxValueTerm[i, 0];
                ValueIntervalTerm[i, NumbersOfZonesOneLP - 1] = FindMinMaxValueTerm[i, 1];
                for (int j = 1; j < NumbersOfZonesOneLP; j++)
                {
                    ValueIntervalTerm[i, j] = ValueIntervalTerm[i, j - 1] + interval;
                }
            }

            for (column = 0; column < countColumnData; column++)  // Запись возможных значений (числами) термов у ЛП. Для вывода.
            {
                ValueTermVithoutRepeat[0, column] = Clusters.ElementAt(0).Centroid.ElementAt(column);
                int countElements = 1;
                for (int rown = 1; rown < ClusterCount; rown++)
                {
                    for (int j = 0; j < countElements; j++)
                    {
                        if (Clusters.ElementAt(rown).Centroid.ElementAt(column) == ValueTermVithoutRepeat[j, column])
                        {
                            break;
                        }
                        if (Clusters.ElementAt(rown).Centroid.ElementAt(column) != ValueTermVithoutRepeat[j, column] && j + 1 == countElements)
                        {
                            ValueTermVithoutRepeat[j + 1, column] = Clusters.ElementAt(rown).Centroid.ElementAt(column);
                            countElements++;
                        }
                    }
                }
            }
        }
       
        public static void FindingPoints (double aGaus, double sigmGaus, int ClusterCount, int countColumnDataNow, int countColumnData, FuzzyKnowledgeBase FKB)
        {
            double A = aGaus - 3*sigmGaus;
            double B = aGaus;
            double C = aGaus + 3*sigmGaus;
            if(countColumnDataNow + 1 == countColumnData)
            {
                FKB.ListOfRule[ClusterCount].Cоnsequens.a = A;
                FKB.ListOfRule[ClusterCount].Cоnsequens.b = B;
                FKB.ListOfRule[ClusterCount].Cоnsequens.c = C;
            }
            else
            {
                FKB.ListOfRule[ClusterCount].Antecedents.ElementAt(countColumnDataNow).a = A;
                FKB.ListOfRule[ClusterCount].Antecedents.ElementAt(countColumnDataNow).b = B;
                FKB.ListOfRule[ClusterCount].Antecedents.ElementAt(countColumnDataNow).c = C;
            }
        }

        public void ReadingFromXlsFile(string path)
        {
            HSSFWorkbook hssfwb;

            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read)) 
            {
                hssfwb = new HSSFWorkbook(file);
            }
            ISheet sheet = hssfwb.GetSheet("FirstList");
            List<string> Elements = new List<string>();
            List<ClusterPoint> points = new List<ClusterPoint>();
            
            int j = 0;
            for (int Row = 1; sheet.GetRow(Row) != null; Row++) 
            {
                for (int Col = 1; sheet.GetRow(Row).GetCell(Col) != null; Col++)
                {
                    Elements.Add(string.Format("{0: 0.0}", sheet.GetRow(Row).GetCell(Col)));
                    List<double> result = Elements.Select(x => double.Parse(x)).ToList();
                    MatrixOfTheElements[Row,Col] = result.ElementAt(j);
                    points.Add(new ClusterPoint(Row, Col, result.ElementAt(j)));
                    j++;
                }
            }

            for (int Row = 1; sheet.GetRow(Row).GetCell(0) != null; Row++)  // подсчет количества строк в файле
            {
                counterFoRowDataFromFile++;
            }

            for (int column = 1; sheet.GetRow(0).GetCell(column) != null; column++) // подсчет количества колонок в файле, а также запись названия ЛП
            {
                
                countColumnData ++;
            }
            List<ClusterCentroid> centroids = new List<ClusterCentroid>();
            Random random = new Random();
            for (int i = 0; i < NUMBER_OF_CLUSTERS; i++)
            {
                int randomNumber1 = random.Next(countColumnData);
                int randomNumber2 = random.Next(counterFoRowDataFromFile);
                centroids.Add(new ClusterCentroid(randomNumber1, randomNumber2, MatrixOfTheElements[randomNumber2, randomNumber1]));
            }
        }
        


    }
}