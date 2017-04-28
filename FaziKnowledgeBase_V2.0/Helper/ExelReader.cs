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
        public static int countColumnData = 0, counterFoRowDataFromFile = 0, NumbersOfZonesOneLP = 0, RecommendCountOfMaxClusterCount = 0, ClusterCount;
        public static List<string> NameOfLinguisticVariables = new List<string>();
        public static double[,] ElementsMatrix;
        public static List<MultiDimensionalVector> ElementsMulti = new List<MultiDimensionalVector>();
        public static double[,] FindMinMaxValueTerm;
        public static double[,] ValueIntervalTerm;
        public static double[,] ValueTermVithoutRepeat;
        public static int ostanovkaLP = 0;
        public static int ostanovkaTM = 0;
        public static List<string> NameOfTerms = new List<string>();
        public static List<int> WeightOfTerms = new List<int>();
        static HSSFWorkbook wb;
        static HSSFSheet sh;
        static double s;
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
        public static void ProcessedDataFromFile(List<Cluster> Clusters)  // найти термы, возможные их значения 
        {
            GiveNameToTerms(ClusterCount, counterFoRowDataFromFile);  // выбор количества названий термов

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
        public static void GiveNameToTerms(int ClusterCount, int counterFoRowDataFromFile)  // функция для определения просранства имен термов, а также количества зон (возможных значений термов) одной ЛП
        {
            /*if(ClusterCount <= 4 && ClusterCount > 0)
            {
                NameOfTerms.Add("якість низька");
                NameOfTerms.Add("якість середня");
                NameOfTerms.Add("якість висока");
                WeightOfTerms.Add(1);
                WeightOfTerms.Add(3);
                WeightOfTerms.Add(5);
                NumbersOfZonesOneLP = 3;
           }
            else if(ClusterCount > 4 && ClusterCount <=6)
            {*/
            /*NameOfTerms.Add("очень маленькая");
            NameOfTerms.Add("маленькая");
            NameOfTerms.Add("средняя");
            NameOfTerms.Add("большая");
            NameOfTerms.Add("очень большая");
            WeightOfTerms.Add(0);
            WeightOfTerms.Add(1);
            WeightOfTerms.Add(3);
            WeightOfTerms.Add(4);
            WeightOfTerms.Add(5);
            NumbersOfZonesOneLP = 5;*/
            /*}
             else if (ClusterCount >= 7 && ClusterCount <= (counterFoRowDataFromFile / 2) + 3) 
             {*/
            NameOfTerms.Add("low");
            NameOfTerms.Add("middle");
            NameOfTerms.Add("high");
            NameOfTerms.Add("very high");
            WeightOfTerms.Add(0);
            WeightOfTerms.Add(1);
            WeightOfTerms.Add(2);
            WeightOfTerms.Add(3);
            NumbersOfZonesOneLP = 4;
            /*}*/
        }
        public static double Y(double p, int CheckerOfPart, double aGaus, double sigmGaus) // Нахождения координаты точки а
        {
            if (p == 0) // посмотреть детально
            {
                return 0;
            }
            else if (CheckerOfPart == 0)
            {
                return (aGaus - sigmGaus * (Math.Sqrt(-Math.Log(p)))) * p;
            }
            else if (CheckerOfPart == 1)
            {
                return (aGaus + sigmGaus * (Math.Sqrt(-Math.Log(p)))) * p;
            }
            else if (CheckerOfPart == 2)
            {
                return aGaus - sigmGaus * (Math.Sqrt(-Math.Log(p)));
            }
            else if (CheckerOfPart == 3)
            {
                return aGaus + sigmGaus * (Math.Sqrt(-Math.Log(p)));
            }
            else
                return 0;
        }
        public static double Y(double p, int CheckerOfPart, int a, double aGaus, double sigmGaus)// Нахождения координаты точки b
        {
            if (p == 0)
            {
                return 0;
            }
            else if (CheckerOfPart == 0)
            {
                return aGaus - sigmGaus * (Math.Sqrt(-Math.Log(p)));
            }
            else if (CheckerOfPart == 1)
            {
                return aGaus + sigmGaus * (Math.Sqrt(-Math.Log(p)));
            }
            else if (CheckerOfPart == 2)
            {
                return (aGaus - sigmGaus * (Math.Sqrt(-Math.Log(p)))) * p;
            }
            else if (CheckerOfPart == 3)
            {
                return (aGaus + sigmGaus * (Math.Sqrt(-Math.Log(p)))) * p;
            }
            else
                return 0;
        }
        public static double Y(double p, int CheckerOfPart, int a, int d, double aGaus, double sigmGaus) // Нахождения координаты точки c
        {
            if (p == 0)
            {
                return 0;
            }
            else if (CheckerOfPart == 0)
            {
                return aGaus - sigmGaus * (Math.Sqrt(-Math.Log(p)));
            }
            else if (CheckerOfPart == 1)
            {
                return aGaus + sigmGaus * (Math.Sqrt(-Math.Log(p)));
            }
            else if (CheckerOfPart == 2)
            {
                return (aGaus - sigmGaus * (Math.Sqrt(-Math.Log(p)))) * p;
            }
            else if (CheckerOfPart == 3)
            {
                return (aGaus + sigmGaus * (Math.Sqrt(-Math.Log(p)))) * p;
            }
            else
                return 0;
        }
        public static void SimpsonsMethodFindingIntegrall(double aGaus, double sigmGaus, int ClusterCount, int countColumnDataNow, int countColumnData, FuzzyKnowledgeBase FKB)//интегрирует любые данные (для точек фп)
        {
            double a = 0, b = 1, eps = 0.0001, result = 0; //Нижний и верхний пределы интегрирования (a, b), погрешность (eps).
            double I = eps + 1, I1 = 0;//I-предыдущее вычисленное значение интеграла, I1-новое, с большим N.

            //Запись новой точки (  a  )
            for (int i = 0; i < 4; i++)
            {
                a = 0; b = 1; eps = 0.0001; //Нижний и верхний пределы интегрирования (a, b), погрешность (eps).
                I = eps + 1; I1 = 0;//I-предыдущее вычисленное значение интеграла, I1-новое, с большим N.
                for (int N = 2; (N <= 4) || (Math.Abs(I1 - I) > eps); N *= 2)
                {
                    double h, sum2 = 0, sum4 = 0, sum = 0;
                    h = (b - a) / (2 * N);//Шаг интегрирования.
                    for (int ind = 1; ind <= 2 * N - 1; ind += 2)
                    {
                        sum4 += Y(a + h * ind, i, aGaus, sigmGaus);//Значения с нечётными индексами, которые нужно умножить на 4.
                        sum2 += Y(a + h * (ind + 1), i, aGaus, sigmGaus);//Значения с чётными индексами, которые нужно умножить на 2.
                    }
                    sum = Y(a, i, aGaus, sigmGaus) + 4 * sum4 + 2 * sum2 - Y(b, i, aGaus, sigmGaus);//Отнимаем значение f(b) так как ранее прибавили его дважды. 
                    I = I1;
                    I1 = (h / 3) * sum;
                }
                if (i < 1)
                    result = 3 * I1;
                else if (i == 1)
                    result += 3 * I1;
                else if (i > 1 && i <= 3)
                    result -= I1;
            }
            if (countColumnDataNow + 1 == countColumnData)
                FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Cоnsequens.a = result;
            else
                FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Antecedents[countColumnDataNow].a = result;

            /*//Запись новой точки (  b  )
            result = 0;
            for (int i = 0; i < 4; i++)
            {
                a = 0; b = 1; eps = 0.0001; //Нижний и верхний пределы интегрирования (a, b), погрешность (eps).
                I = eps + 1; I1 = 0;//I-предыдущее вычисленное значение интеграла, I1-новое, с большим N.
                for (int N = 2; (N <= 4) || (Math.Abs(I1 - I) > eps); N *= 2)
                {
                    double h, sum2 = 0, sum4 = 0, sum = 0;
                    h = (b - a) / (2 * N);//Шаг интегрирования.
                    for (int ind = 1; ind <= 2 * N - 1; ind += 2)
                    {
                        sum4 += Y(a + h * ind, i, i, aGaus, sigmGaus);//Значения с нечётными индексами, которые нужно умножить на 4.
                        sum2 += Y(a + h * (ind + 1), i, i, aGaus, sigmGaus);//Значения с чётными индексами, которые нужно умножить на 2.
                    }
                    sum = Y(a, i, i, aGaus, sigmGaus) + 4 * sum4 + 2 * sum2 - Y(b, i, i, aGaus, sigmGaus);//Отнимаем значение f(b) так как ранее прибавили его дважды. 
                    I = I1;
                    I1 = (h / 3) * sum;
                }
                if (i == 0)
                    result = (7 / 2) * I1;
                else if (i == 1)
                    result += (1 / 2) * I1;
                else if (i == 2)
                    result -= (9 / 2) * I1;
                else if (i == 3)
                    result -= (3 / 2) * I1;
            }
            if (countColumnDataNow + 1 == countColumnData) //часть для точки b
                FKB.ListOfRule[FKB.ListOfRule.Count() - WebApplication1.FKB.Program.ClusterCount + ClusterCount].Cоnsequens.a = result;
            else
                FKB.ListOfRule[FKB.ListOfRule.Count() - WebApplication1.FKB.Program.ClusterCount + ClusterCount].Antecedents[countColumnDataNow].a = result;*/

            //Запись новой точки (  c  )
            result = 0;
            for (int i = 0; i < 4; i++)
            {
                a = 0; b = 1; eps = 0.0001; //Нижний и верхний пределы интегрирования (a, b), погрешность (eps).
                I = eps + 1; I1 = 0;//I-предыдущее вычисленное значение интеграла, I1-новое, с большим N.
                for (int N = 2; (N <= 4) || (Math.Abs(I1 - I) > eps); N *= 2)
                {
                    double h, sum2 = 0, sum4 = 0, sum = 0;
                    h = (b - a) / (2 * N);//Шаг интегрирования.
                    for (int ind = 1; ind <= 2 * N - 1; ind += 2)
                    {
                        sum4 += Y(a + h * ind, i, i, i, aGaus, sigmGaus);//Значения с нечётными индексами, которые нужно умножить на 4.
                        sum2 += Y(a + h * (ind + 1), i, i, i, aGaus, sigmGaus);//Значения с чётными индексами, которые нужно умножить на 2.
                    }
                    sum = Y(a, i, i, i, aGaus, sigmGaus) + 4 * sum4 + 2 * sum2 - Y(b, i, i, i, aGaus, sigmGaus);//Отнимаем значение f(b) так как ранее прибавили его дважды. 
                    I = I1;
                    I1 = (h / 3) * sum;
                }
                if (i == 0)
                    result = (1 / 2) * I1;
                else if (i == 1)
                    result += (7 / 2) * I1;
                else if (i == 2)
                    result -= (3 / 2) * I1;
                else if (i == 3)
                    result -= (9 / 2) * I1;
            }
            if (countColumnDataNow + 1 == countColumnData) //часть для точки b
                FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Cоnsequens.c = result;
            else
                FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Antecedents[countColumnDataNow].c = result;

            double tempTerm = 0;
            if (countColumnDataNow + 1 == countColumnData) //часть для точки b
                FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Cоnsequens.b = (FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Cоnsequens.a + FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Cоnsequens.c) / 2;
            else
                FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Antecedents[countColumnDataNow].b = (FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Antecedents[countColumnDataNow].a + FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Antecedents[countColumnDataNow].c) / 2;

            if (countColumnDataNow + 1 == countColumnData)
            {
                if (FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Cоnsequens.a > FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Cоnsequens.c)
                {
                    tempTerm = FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Cоnsequens.c;
                    FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Cоnsequens.c = FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Cоnsequens.a;
                    FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Cоnsequens.a = tempTerm;
                }
            }
            else
            {
                if (FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Antecedents[countColumnDataNow].a > FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Antecedents[countColumnDataNow].c)
                {
                    tempTerm = FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Antecedents[countColumnDataNow].c;
                    FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Antecedents[countColumnDataNow].c = FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Antecedents[countColumnDataNow].a;
                    FKB.ListOfRule[FKB.ListOfRule.Count() - ExelReader.ClusterCount + ClusterCount].Antecedents[countColumnDataNow].a = tempTerm;
                }
            }
        }
    }
}