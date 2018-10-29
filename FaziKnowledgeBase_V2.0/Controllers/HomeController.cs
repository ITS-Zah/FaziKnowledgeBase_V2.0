using System;
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
using AI.Fuzzy.Library;

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
                    double[] matWaiting = new double[alg.Clusters.Count];
                    double[] sigma = new double[alg.Clusters.Count];
                    while (true)
                    {
                        alg.J = alg.CalculateObjectiveFunction();
                        alg.CalculateClusterCentroids();
                        alg.Step();
                        double Jnew = alg.CalculateObjectiveFunction();
                        if (Math.Abs(alg.J - Jnew) < epsilon)
                        {
                            for (int i = 0; i < alg.Clusters.Count; i++)
                            {
                                matWaiting[i] = alg.Clusters[i].Value;
                            }
                            Array.Sort(matWaiting);
                            break;
                        }
                    }
                    for (int i = 0; i < matWaiting.Length; i++)
                    {
                        if (i == matWaiting.Length - 1)
                        {
                            break;
                        }
                        else
                        {
                            sigma[i] = matWaiting[i + 1] - matWaiting[i];
                        }
                        if(i == matWaiting.Length - 2)
                        {
                            sigma[i + 1] = sigma[i];
                        }
                        
                    }
                    MamdaniFuzzySystem qOS = new MamdaniFuzzySystem();
                    FuzzyVariable callDrop = new FuzzyVariable("Call Drop", 0.0, 1.0);
                    callDrop.Terms.Add(new FuzzyTerm("poor", new TriangularMembershipFunction(-0.33, 0.0, 0.33)));
                    callDrop.Terms.Add(new FuzzyTerm("good", new TriangularMembershipFunction(0.0, 0.33, 0.66)));
                    callDrop.Terms.Add(new FuzzyTerm("excellent", new TriangularMembershipFunction(0.33, 0.66, 0.99)));
                    qOS.Input.Add(callDrop);

                    FuzzyVariable voiceRAB = new FuzzyVariable("Voice RAB", 0.0, 1.0);
                    voiceRAB.Terms.Add(new FuzzyTerm("poor1", new TriangularMembershipFunction(0.0, 0.1, 0.25)));
                    voiceRAB.Terms.Add(new FuzzyTerm("good1", new TriangularMembershipFunction(0.1, 0.25, 0.5)));
                    voiceRAB.Terms.Add(new FuzzyTerm("excellent1", new TriangularMembershipFunction(0.5, 0.75, 1.1)));
                    qOS.Input.Add(voiceRAB);

                    FuzzyVariable voiceSoft = new FuzzyVariable("Voice Soft", 0.0, 1.0);
                    voiceSoft.Terms.Add(new FuzzyTerm("poor2", new TriangularMembershipFunction(0.0, 0.2, 0.3)));
                    voiceSoft.Terms.Add(new FuzzyTerm("good2", new TriangularMembershipFunction(0.2, 0.45, 0.6)));
                    voiceSoft.Terms.Add(new FuzzyTerm("excellent2", new TriangularMembershipFunction(0.5, 0.8, 1.2)));
                    qOS.Input.Add(voiceSoft);

                    FuzzyVariable output_qOS = new FuzzyVariable("qOS", 0.0, 1.0);

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
