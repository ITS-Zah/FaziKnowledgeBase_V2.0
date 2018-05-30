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
using FaziKnowledgeBase_V2._0.Models.Chart;
using g3;

namespace FaziKnowledgeBase_V2._0.Controllers
{
    [RoutePrefix("Chart")]
    public class ChartController : ApiController
    {
        [HttpGet, Route("GetFkb")]
        public FuzzyKnowledgeBase GetFkb()
        {
            return СonclusionController.FKB;
        }
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
        public Chart GetVievDefuzzication()
        {
            List<Rule> rules = Accumulation.AccumulationStart(СonclusionController.FKB);
            Term res = Defuzzication.DefuzzicationStart(rules);
            Chart chart = new Chart();
            chart.NameChart = rules[0].Cоnsequens.NameLP;
            foreach (var item in СonclusionController.FKB.ListVar[СonclusionController.FKB.ListVar.Count - 1].terms)
            {
                chart.SimplyLines.Add(new Line(new List<Point>() { new Point(item.a, 0), new Point(item.b, 1), new Point(item.c, 0) }, item.Name));

            }
            for (int i = 0; i < chart.SimplyLines.Count; i++)
            {
                if (i == 0)
                {
                    chart.SimplyLines[0].Points[0].Y = 1;
                }
                if (i == chart.SimplyLines.Count - 1)
                {
                    chart.SimplyLines[chart.SimplyLines.Count - 1].Points[2].Y = 1;
                }
            }
            chart.points.Add(new Point(res.NumericValue, res.ZnachFp));
            foreach (var item in СonclusionController.FKB.ListVar[СonclusionController.FKB.ListVar.Count - 1].terms)
            {
                chart.BoldLines.Add(new Line(new List<Point>() { new Point(item.a, 0), CulclPoint(new Point(item.a,0), new Point(item.b, 1), new Point(0,item.NumericValue), new Point(1, item.NumericValue)) , CulclPoint(new Point(item.b, 1), new Point(item.c, 0), new Point(0, item.NumericValue), new Point(1, item.NumericValue)), new Point(item.c, 0) }, item.Name));
            }
            for (int i = 0; i < chart.BoldLines.Count; i++)
            {
                if (i == 0)
                {
                    chart.BoldLines[0].Points[0].Y = 1;
                }
                if (i == chart.BoldLines.Count - 1)
                {
                    chart.BoldLines[chart.BoldLines.Count - 1].Points[2].Y = 1;
                }
            }
            return chart;
        }
        public Point CulclPoint(Point point1_1 , Point point1_2, Point point2_1, Point point2_2)
        {
            Line2d line2D = new Line2d(new Vector2d(point1_1.X, point1_1.Y), new Vector2d(point1_2.X - point1_1.X, point1_2.Y - point1_1.Y));
            Line2d line2D2 = new Line2d(new Vector2d(point2_1.X, point2_1.Y), new Vector2d(point2_1.X - point2_2.X, point2_1.Y - point2_2.Y));
            Vector2d vector = line2D.IntersectionPoint(ref line2D2);
            Point res = new Point(vector.x, vector.y);
            return res;
        }
    }
}
