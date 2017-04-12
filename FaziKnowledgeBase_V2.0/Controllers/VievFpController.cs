using FuzzyKnowledgeBase_V2._0.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Mvc;
using SimpleChart = System.Web.Helpers;
namespace FaziKnowledgeBase_V2._0.Controllers
{
    public class VievFpController : Controller
    {
        // GET: VievAllFp
        public static FuzzyKnowledgeBase FKB;
        public ActionResult Index()
        {
            List<string> NameAllLV = new List<string>();
            using (FileStream fs = new FileStream(System.Environment.GetEnvironmentVariable("PathFkbFiles") + HttpContext.Request.Cookies["FileName"].Value, FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(FuzzyKnowledgeBase));
                FKB = (FuzzyKnowledgeBase)jsonFormatter.ReadObject(fs);
                string LvConclusions = FKB.ListOfRule[0].Cоnsequens.NameLP;
                ViewData["NameLv"] = LvConclusions;
                for (int i = 0; i < FKB.ListVar.Count; i++)
                {
                    NameAllLV.Add(FKB.ListVar[i].Name);
                }
            }
            return View(NameAllLV);

        }
        public ActionResult VievFpOneLv(string nameLv)
        {
            ViewData["NameLv"] = nameLv;
            return PartialView();
        }
        
        public ActionResult CreateChart(string nameLv)
        {
            const string Blue = "<Chart BackColor=\"#D3DFF0\" BackGradientStyle=\"TopBottom\" BackSecondaryColor=\"White\" BorderColor=\"26, 59, 105\" BorderlineDashStyle=\"Solid\" BorderWidth=\"15\" Palette=\"BrightPastel\">\r\n    <ChartAreas>\r\n        <ChartArea Name=\"Default\" _Template_=\"All\" BackColor=\"64, 165, 191, 228\" BackGradientStyle=\"TopBottom\" BackSecondaryColor=\"White\" BorderColor=\"64, 64, 64, 64\" BorderDashStyle=\"Solid\" ShadowColor=\"Transparent\" /> \r\n    </ChartAreas>\r\n    <Legends>\r\n        <Legend _Template_=\"All\" BackColor=\"Transparent\" Font=\"Trebuchet MS, 8.25pt, style=Bold\" IsTextAutoFit=\"False\" /> \r\n    </Legends>\r\n    <BorderSkin SkinStyle=\"Emboss\" /> \r\n  </Chart>";     
            var chart = new SimpleChart.Chart(width: 800, height: 300,theme:Blue).AddTitle(("Membership function: " + nameLv)).AddLegend();
            
            for (int i =0; i < FKB.ListVar.Count; i++)
            {
                if(FKB.ListVar[i].Name == nameLv)
                {
                    chart.AddSeries(
                                name: FKB.ListVar[i].terms[0].Name,
                                chartType: "Line",
                                xValue: new[] { FKB.ListVar[i].terms[0].a, FKB.ListVar[i].terms[0].b, FKB.ListVar[i].terms[0].c },
                                yValues: new[] { 1, 1, 0 });
                    

                    for (int j = 1; j < FKB.ListVar[i].terms.Count - 1; j++)
                    {
                       
                        chart.AddSeries(
                        name: FKB.ListVar[i].terms[j].Name,
                        chartType: "Line",
                        xValue: new[] { FKB.ListVar[i].terms[j].a, FKB.ListVar[i].terms[j].b, FKB.ListVar[i].terms[j].c },
                        yValues: new[] { 0, 1, 0 });

                    }
                    chart.AddSeries(
                               name: FKB.ListVar[i].terms[FKB.ListVar[i].terms.Count - 1].Name,
                               chartType: "Line",
                               xValue: new[] { FKB.ListVar[i].terms[FKB.ListVar[i].terms.Count - 1].a, FKB.ListVar[i].terms[FKB.ListVar[i].terms.Count - 1].b, FKB.ListVar[i].terms[FKB.ListVar[i].terms.Count - 1].c },
                               yValues: new[] { 0, 1, 1 })
                               .Write();
                    break;
                }
            }
            return null;
        }
    }
}