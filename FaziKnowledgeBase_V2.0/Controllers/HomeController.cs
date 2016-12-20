using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                //string FKB = WebApplication1.FuzzyLogicBase.FuzzyKnowledgeBase.runFuzzy("Стан екології", Parser("Повітря:Грунти"), Parser("0,6:0,5"), "xls", @"C:\Metagraph_docs\Молоко.xls");
            }
            return RedirectToAction("Index");
        }
    }
}
