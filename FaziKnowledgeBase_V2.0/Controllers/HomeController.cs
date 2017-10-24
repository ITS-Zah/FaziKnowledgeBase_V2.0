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

namespace FuzzyKnowledgeBase_V2._0.Controllers
{
    public class HomeController : Controller
    {

        private double epsilon = 0.05;

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
                    // read xls data file
                    ExelReader.ReadFromXLS(upload.FileName);
                    K_means k = new K_means(ExelReader.ElementsMulti, null, ExelReader.ClusterCount, ExelReader.ElementsMatrix);
                    
                    // clustering
                    k.Clustering(ExelReader.ClusterCount, epsilon);

                    // find rules 
                    k.FindRulesModelTypeMamdani(ExelReader.NameOfLinguisticVariables, ExelReader.ValueIntervalTerm, ExelReader.NameOfTerms, 
                        ExelReader.countColumnData, ExelReader.NumbersOfZonesOneLP, ExelReader.counterFoRowDataFromFile, "Трикутна", ExelReader.WeightOfTerms, FKB);

                    // find function for each term
                    k.GausFunction(ExelReader.countColumnData, FKB);

                    FKBHelper.WithRullToVar(FKB);
                    FKBHelper.Save_BNZ(FKB, Server.MapPath("~/Files/BNZauto.txt"));
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
