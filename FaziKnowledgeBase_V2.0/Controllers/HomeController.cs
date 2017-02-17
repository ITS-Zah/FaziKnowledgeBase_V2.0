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
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (upload != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(Server.MapPath("~/Files/" + fileName));
                using(FileStream fs = new FileStream("~/Files/" + fileName, FileMode.OpenOrCreate))
                {
                    DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(FuzzyKnowledgeBase));
                    FuzzyKnowledgeBase FKB = (FuzzyKnowledgeBase)jsonFormatter.ReadObject(fs);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
