using FuzzyKnowledgeBase_V2._0.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Web.Http;
using Newtonsoft.Json;
using System.Web;
using FuzzyKnowledgeBase_V2._0.Controllers;
using FuzzyKnowledgeBase_V2._0.ActionsFKB;

namespace FaziKnowledgeBase_V2._0.Controllers
{
    [RoutePrefix("Chart")]
    public class ChartController : ApiController
    {
        [HttpGet, Route("GetListLV")]
        public List<LinguisticVariable> GetListLV()
        {
            return СonclusionController.FKB.ListVar;
        }
        [HttpPost, Route("StartPhasing")]
        public FuzzyKnowledgeBase StartPhasing([FromBody]string[] parametres)
        {
            Phasing.PhasingLv(СonclusionController.FKB, parametres);
            return СonclusionController.FKB;
        }
        [HttpGet, Route("GetVievAgregation")]
        public List<string> GetVievAgregation()
        {
            Agregation.AgregationStart(СonclusionController.FKB);
            List<string> result = new List<string>();
            foreach (Rule item in СonclusionController.FKB.ListOfRule)
            {
                string s = $"IF {item.Antecedents[0].NameLP} = {item.Antecedents[0].Name} ";
                for (int i = 1; i < item.Antecedents.Count; i++)
                {
                    s += $"AND {item.Antecedents[i].Name} = {item.Cоnsequens.Name}";
                }
                s += $" THEN {item.Cоnsequens.NameLP} = {item.Cоnsequens.Name} ( Minimal value {item.MinZnach})";
                result.Add(s);
            }
            return result;
        }
        [HttpGet, Route("GetVievAccumulation")]
        public List<string> GetVievAccumulation()
        {
            List<Rule> rules = Accumulation.AccumulationStart(СonclusionController.FKB);
            List<string> result = new List<string>();
            foreach (Rule item in rules)
            {
                string s = $"IF {item.Antecedents[0].NameLP} = {item.Antecedents[0].Name} ";
                for (int i = 1; i < item.Antecedents.Count; i++)
                {
                    s += $"AND {item.Antecedents[i].Name} = {item.Cоnsequens.Name}";
                }
                s += $" THEN {item.Cоnsequens.NameLP} = {item.Cоnsequens.Name} ( Minimal value {item.MinZnach})";
                result.Add(s);
            }
            return result;
        }
        [HttpGet, Route("GetVievDefuzzication")]
        public FuzzyKnowledgeBase GetVievDefuzzication()
        {
            List<Rule> rules = Accumulation.AccumulationStart(СonclusionController.FKB);
            Term res = Defuzzication.DefuzzicationStart(rules);
            return СonclusionController.FKB;
        }
        [HttpGet, Route("GetFKB")]
        public FuzzyKnowledgeBase GetFKB()
        {
            return СonclusionController.FKB;
        }
    }
}
