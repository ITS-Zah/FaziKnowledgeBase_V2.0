using FuzzyKnowledgeBase_V2._0.ActionsFKB;
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
    public class CMDController : Controller
    {
        // GET: CMD
        public ActionResult Index()
        {
            return View();
        }
        public string ExelReader(params string[] valueLv)
        {
            string resultall = "";
            using (FileStream fs = new FileStream(System.Environment.GetEnvironmentVariable("PathFkbFiles") + "BNZ.txt", FileMode.Open))
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(FuzzyKnowledgeBase));
                FuzzyKnowledgeBase FKB = (FuzzyKnowledgeBase)jsonFormatter.ReadObject(fs);
                Phasing.PhasingLv(FKB, valueLv);
                Agregation.AgregationStart(FKB);
                List<Rule> result = Accumulation.AccumulationStart(FKB);
                Term t = Defuzzication.DefuzzicationStart(result);
                resultall = t.b.ToString();
            }           
            return resultall;
        }
    }
}