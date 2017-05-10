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
        public static string  fileName;
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
            for(int i = 0; i < FKB.ListVar.Count; i++)
            {
                if (FKB.ListVar[i].Name != valueLV[i])
                {
                    FKBHelper.EditLinguisticVariable(FKB, FKB.ListVar[i].Name, valueLV[i]);
                }             
            }
            FKBHelper.Save_BNZ(FKB, @"C:\MetaDoc\BNZauto.txt");
           // return View();
        }
    }
}