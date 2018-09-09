﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FaziKnowledgeBase_V2._0.Helper;
using FaziKnowledgeBase_V2._0.FKB.FormingFKB;
using FuzzyKnowledgeBase_V2._0.Models;
using FaziKnowledgeBase_V2._0.FKB.Helper;
using FaziKnowledgeBase_V2._0.Models.C_Means_Clustering;

namespace FuzzyKnowledgeBase_V2._0.Controllers
{
    public class HomeController : Controller
    {
        
        [HttpGet]
        public ActionResult Index()
        {
            string s = System.Environment.GetEnvironmentVariable("Test");
           
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            
            string FileFormat = "";
            ExelReader exelreader = new ExelReader();
            if (upload != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                // сохраняем файл в папку Files в проекте
                FuzzyKnowledgeBase FKB = new FuzzyKnowledgeBase();
                FKB.ListOfRule.Clear();
                FKB.ListVar.Clear();
                upload.SaveAs(Server.MapPath("~/Files/" + fileName));
                HttpContext.Response.Cookies["FileName"].Value = fileName;
                FileFormat = FileHelper.CheckFileFormat(fileName);
                if(FileFormat == "txt")
                {
                    return RedirectToAction("ReadyForms","Сonclusion",new {FileName = fileName });
                }
                else if (FileFormat == "xls")
                {
                    //exelreader.ReadFromXLS(Environment.GetEnvironmentVariable("PathFkbFiles") + fileName);
                    double epsilon = 0.05;
                    exelreader.ReadingFromXlsFile(fileName);
                    //K_means k = new K_means(exelreader.ElementsMulti, null, ExelReader.ClusterCount, ExelReader.ElementsMatrix);
                    FCM alg = new FCM(exelreader.points, exelreader.centroids, 2, exelreader.NumberOfTheRows, exelreader.NumberOfTheColums, exelreader.NumberOfTheColums);
                    while (true)
                    {
                        alg.J = alg.CalculateObjectiveFunction();
                        alg.CalculateClusterCentroids();
                        alg.Step();
                        double Jnew = alg.CalculateObjectiveFunction();
                        if (Math.Abs(alg.J - Jnew) < epsilon)
                        {
                            double[,] asad = alg.U;
                            break;
                        }
                    }
                   // double epsilon = 0.05;
                    //k.Clustering(ExelReader.ClusterCount, epsilon);
                    //k.FindRulesModelTypeMamdani(ExelReader.NameOfLinguisticVariables, ExelReader.ValueIntervalTerm, ExelReader.NameOfTerms, ExelReader.countColumnData, ExelReader.NumbersOfZonesOneLP, ExelReader.counterFoRowDataFromFile, "Трикутна", ExelReader.WeightOfTerms, FKB);
                    //k.GausFunction(ExelReader.countColumnData, FKB);
                    FKBHelper.WithRullToVar(FKB);
                    FKBHelper.Save_BNZ(FKB, System.Environment.GetEnvironmentVariable("PathFkbFiles")+ "BNZauto.txt");
                    return RedirectToAction("ReadyForms", "Сonclusion", new { FileName = "BNZauto.txt" });
                }
            }
            return RedirectToAction("Index");
        }
        public string Test(string z)
        {
            return z;
        }
    }
}
