using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FuzzyKnowledgeBase_V2._0.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            string s = System.Environment.GetEnvironmentVariable("Test");
            ViewData["TestConvert"] = Double.Parse("5,4");;
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
                HttpContext.Response.Cookies["FileName"].Value = fileName;
            }
            return RedirectToAction("Index");
        }
        public string Test(string z)
        {
            return z;
        }
    }
}
