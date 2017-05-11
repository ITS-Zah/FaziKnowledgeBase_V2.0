using FaziKnowledgeBase_V2._0.FKB.Helper;
using FuzzyKnowledgeBase_V2._0.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Mvc;

namespace FaziKnowledgeBase_V2._0.Controllers
{
    public class EditFKBController : Controller
    {
        public static FuzzyKnowledgeBase FKB;
        public static string fileName;
        public static string NameLv;
        public static string NameTerm;
        // GET: EditFKB
        public ActionResult Index()
        {
            List<string> ListFiles = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(System.Environment.GetEnvironmentVariable("PathFkbFiles"));
            // Для извлечения имени файла используется цикл foreach и свойство files.name
            foreach (FileInfo files in dir.GetFiles())
            {
                ListFiles.Add(files.Name);
            }
            return View(ListFiles);
        }
        public ActionResult Edit(string FileName)
        {
            HttpContext.Response.Cookies["FileName"].Value = FileName;
            fileName = FileName;
            return View();
        }
        public ActionResult EditLvIndex()
        {
            using (FileStream fs = new FileStream(System.Environment.GetEnvironmentVariable("PathFkbFiles") + fileName, FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(FuzzyKnowledgeBase));
                FKB = (FuzzyKnowledgeBase)jsonFormatter.ReadObject(fs);
            }
            return View(FKB);
        }
        public void EditLV(params string[] valueLV)
        {
            for (int i = 0; i < FKB.ListVar.Count; i++)
            {
                if (FKB.ListVar[i].Name != valueLV[i])
                {
                    FKBHelper.EditLinguisticVariable(FKB, FKB.ListVar[i].Name, valueLV[i]);
                }
            }
            FKBHelper.Save_BNZ(FKB, System.Environment.GetEnvironmentVariable("PathFkbFiles") + fileName);
            // return View();
        }
        public ActionResult EditTermsIndex()
        {
            using (FileStream fs = new FileStream(System.Environment.GetEnvironmentVariable("PathFkbFiles") + fileName, FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(FuzzyKnowledgeBase));
                FKB = (FuzzyKnowledgeBase)jsonFormatter.ReadObject(fs);
            }
            return View(FKB);
        }

        public void EditTerm(string nameLv, params string[] valueTerm)
        {
            foreach (var linguisticVariable in FKB.ListVar)//заміна у спику лінгвістичних мінних
            {
                if (linguisticVariable.Name == nameLv)
                {
                    for (int i = 0; i < linguisticVariable.terms.Count; i++)
                    {
                        if (linguisticVariable.terms[i].Name != valueTerm[i])
                        {
                            FKBHelper.EditTerm(FKB, linguisticVariable.Name, linguisticVariable.terms[i].Name, valueTerm[i]);
                        }
                    }

                }
            }
            FKBHelper.Save_BNZ(FKB, System.Environment.GetEnvironmentVariable("PathFkbFiles") + fileName);
        }
        public ActionResult EditFpTermsIndex()
        {
            List<string> NameAllLV = new List<string>();
            using (FileStream fs = new FileStream(System.Environment.GetEnvironmentVariable("PathFkbFiles") + HttpContext.Request.Cookies["FileName"].Value, FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(FuzzyKnowledgeBase));
                FKB = (FuzzyKnowledgeBase)jsonFormatter.ReadObject(fs);
                string LvConclusions = FKB.ListOfRule[0].Cоnsequens.NameLP;
                ViewData["NameLv"] = LvConclusions;
                for (int i = 0; i < FKB.ListVar.Count; i++)
                {
                    NameAllLV.Add(FKB.ListVar[i].Name);
                }
            }
            return View(NameAllLV);
        }
        public ActionResult VievTermWithFp(string nameLv)
        {
            NameLv = nameLv;
            ViewData["NameLv"] = nameLv;
            foreach (var Var in FKB.ListVar)
            {
                if(Var.Name == nameLv)
                {
                    return View(Var.terms);
                }
            }
            return View();
        }
        public ActionResult EditFpIndex(string nameTerm)
        {
            NameTerm = nameTerm;
            foreach (var Var in FKB.ListVar)
            {
                if(Var.Name == NameLv)
                {
                    foreach(var term in Var.terms)
                    {
                        if(term.Name == NameTerm)
                        {
                            return View(term);
                        }
                    }
                }
            }
            return View();
        }
        public ActionResult EditFp( string a, string b, string c)
        {
            ViewData["NameLv"] = NameLv;
            FKBHelper.EditFpTerm(FKB, NameLv, NameTerm, a, b, c);
            FKBHelper.Save_BNZ(FKB, System.Environment.GetEnvironmentVariable("PathFkbFiles") + fileName);
            return View();
        }
    }
}