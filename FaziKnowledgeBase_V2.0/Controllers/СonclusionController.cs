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
            using (FileStream fs = new FileStream("~/Files/" + "BNZ.txt", FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(FuzzyKnowledgeBase));
                FuzzyKnowledgeBase FKB = (FuzzyKnowledgeBase)jsonFormatter.ReadObject(fs);
            }
            return View();
        }
    }
}