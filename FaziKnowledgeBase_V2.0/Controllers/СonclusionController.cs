using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using FuzzyKnowledgeBase_V2._0.Models;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using FuzzyKnowledgeBase_V2._0.ActionsFKB;

namespace FuzzyKnowledgeBase_V2._0.Controllers
{
    public class СonclusionController : Controller
    {
        public static FuzzyKnowledgeBase FKB;
        // GET: Сonclusion
        public ActionResult Index()
        {
            List<string> ListFiles = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(/*@"C:\MetaDoc\"*/Server.MapPath("~/Files/"));
            // Для извлечения имени файла используется цикл foreach и свойство files.name
            foreach (FileInfo files in dir.GetFiles())
            {
                ListFiles.Add(files.Name);
            }
            //using (FileStream fs = new FileStream(Server.MapPath("~/Files/BNZ.txt"), FileMode.OpenOrCreate))
            //{
            //    DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(FuzzyKnowledgeBase));
            //    FuzzyKnowledgeBase FKB = (FuzzyKnowledgeBase)jsonFormatter.ReadObject(fs);
            //}
            return View(ListFiles);
        }
        public ActionResult ReadyForms(string FileName)
        {
            HttpContext.Response.Cookies["FileName"].Value = FileName;
            List<string> ParametrsLp = new List<string>();
            using (FileStream fs = new FileStream(/*@"C:\MetaDoc\"*/ Server.MapPath("~/Files/") + FileName, FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(FuzzyKnowledgeBase));
                FKB = (FuzzyKnowledgeBase)jsonFormatter.ReadObject(fs);
                string LvConclusions = FKB.ListOfRule[0].Cоnsequens.NameLP;
                ViewData["NameLv"] = LvConclusions;               
                for(int i =0; i < FKB.ListVar.Count; i++)
                {
                    if(FKB.ListVar[i].Name != LvConclusions)
                    {
                        ParametrsLp.Add(FKB.ListVar[i].Name);
                    }
                }
            }
            return View(ParametrsLp);
        } 
        public ActionResult GetConclusion(params string [] valueLv)
        {
           
            Phasing.StartPhasing(FKB, valueLv);
            Agregation.AgregationStart(FKB);
            Term result = Accumulation.AccumulationStart(FKB);
            Defuzzication.DefuzzicationStart(result);
            return View(result);
        }

    }
}