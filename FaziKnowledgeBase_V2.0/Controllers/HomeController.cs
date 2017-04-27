using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FaziKnowledgeBase_V2._0.Helper;
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
            if (upload != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(Server.MapPath("~/Files/" + fileName));
                HttpContext.Response.Cookies["FileName"].Value = fileName;
                FileFormat = FileHelper.CheckFileFormat(fileName);
                if(FileFormat == "txt")
                {
                    return RedirectToAction("ReadyForms","Сonclusion",new {FileName = fileName });
                }
                else if (FileFormat == "xls")
                {

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
