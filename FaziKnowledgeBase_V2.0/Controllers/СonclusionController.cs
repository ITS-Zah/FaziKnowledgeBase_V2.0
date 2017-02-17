using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using FaziKnowledgeBase_V2._0.Models;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace FaziKnowledgeBase_V2._0.Controllers
{
    public class СonclusionController : Controller
    {
        // GET: Сonclusion
        public ActionResult Index()
        {
            List<string> ListFiles = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/Files/"));
            // Для извлечения имени файла используется цикл foreach и свойство files.name
            foreach (FileInfo files in dir.GetFiles())
            {
                ListFiles.Add(files.Name);
            }
            ViewBag.ListFiles = ListFiles;
            //using (FileStream fs = new FileStream(Server.MapPath("~/Files/BNZ.txt"), FileMode.OpenOrCreate))
            //{
            //    DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(FuzzyKnowledgeBase));
            //    FuzzyKnowledgeBase FKB = (FuzzyKnowledgeBase)jsonFormatter.ReadObject(fs);
            //}
            return View();
        }
        public ActionResult ReadyForms(string FileName)
        {
            string s = "dfgf";
            s += "sdf";
            return View();
        } 
    }
}