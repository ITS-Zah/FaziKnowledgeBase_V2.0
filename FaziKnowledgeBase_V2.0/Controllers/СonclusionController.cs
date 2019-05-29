using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using FuzzyKnowledgeBase_V2._0.Models;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using FuzzyKnowledgeBase_V2._0.ActionsFKB;
using SimpleChart = System.Web.Helpers;
using Newtonsoft.Json;
namespace FuzzyKnowledgeBase_V2._0.Controllers
{
    public class СonclusionController : Controller
    {
        public static FuzzyKnowledgeBase FKB;
        public static List<Rule> result ;
        // GET: Сonclusion
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
        public ActionResult ReadyForms(string FileName)
        {
            HttpContext.Response.Cookies["FileName"].Value = FileName;
            List<LinguisticVariable> ParametrsLp = new List<LinguisticVariable>();

            using (var fs = new StreamReader(System.Environment.GetEnvironmentVariable("PathFkbFiles") + FileName))
            {
                var file = fs.ReadToEnd();
                FKB = JsonConvert.DeserializeObject<FuzzyKnowledgeBase>(file);
                string LvConclusions = FKB.ListOfRule[0].Cоnsequens.NameLP;
                ViewData["NameLv"] = LvConclusions;
                for (int i = 0; i < FKB.ListVar.Count; i++)
                {
                    if (FKB.ListVar[i].Name != LvConclusions)
                    {
                        ParametrsLp.Add(FKB.ListVar[i]);
                    }
                }

            }

            //using (FileStream fs = new FileStream(System.Environment.GetEnvironmentVariable("PathFkbFiles") + FileName, FileMode.Open))
            //{
            //    DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(FuzzyKnowledgeBase));
            //    FKB = (FuzzyKnowledgeBase)jsonFormatter.ReadObject(fs);
            //    string LvConclusions = FKB.ListOfRule[0].Cоnsequens.NameLP;
            //    ViewData["NameLv"] = LvConclusions;               
            //    for(int i =0; i < FKB.ListVar.Count; i++)
            //    {
            //        if(FKB.ListVar[i].Name != LvConclusions)
            //        {
            //            ParametrsLp.Add(FKB.ListVar[i]);
            //        }
            //    }
            //}

            return View(ParametrsLp);
        } 
        public ActionResult GetConclusion(string action,string file,params string [] valueLv)
        {
            Random rnd = new Random();

            if (action == "RedyForms")
            {
                Phasing.PhasingLv(FKB, valueLv);
                Agregation.AgregationStart(FKB);
                List<Rule> result = Accumulation.AccumulationStart(FKB);
                Term res = Defuzzication.DefuzzicationStart(result);
                double sum = 0;
                foreach (var item in valueLv)
                {
                    sum = sum + Convert.ToDouble(item);
                }
                //res.NumericValue = (sum / valueLv.Count()) - rnd.Next(1, 1000)/1000;
                //res.NumericValue = Math.Round(res.NumericValue, 4);
                if (res.NumericValue < 0.2)
                {
                    res.Name = "low";
                }
                else if(res.NumericValue >= 0.2 && res.NumericValue<= 0.6)
                {
                    res.Name = "middle";
                }
                else
                {
                    res.Name = "height";
                }
                return View(res);
            }
            else
            {
                Phasing.PhasingLv(FKB, valueLv);
                
                return View("GetVievPhasing",FKB);
            }
           
        }
        public ActionResult GetVievPhasing(string[] valueLv)
        {
            Phasing.PhasingLv(FKB, valueLv);
            return View(FKB);
        }
        public ActionResult VievChartPhasing(string nameLv)
        {
            ViewData["NameLv"] = nameLv;
            return PartialView();
        }
        public ActionResult CreateChartPhasing(string nameLv)
        {
            const string Blue = "<Chart BackColor=\"#D3DFF0\" BackGradientStyle=\"TopBottom\" BackSecondaryColor=\"White\" BorderColor=\"26, 59, 105\" BorderlineDashStyle=\"Solid\" BorderWidth=\"15\" Palette=\"BrightPastel\">\r\n    <ChartAreas>\r\n        <ChartArea Name=\"Default\" _Template_=\"All\" BackColor=\"64, 165, 191, 228\" BackGradientStyle=\"TopBottom\" BackSecondaryColor=\"White\" BorderColor=\"64, 64, 64, 64\" BorderDashStyle=\"Solid\" ShadowColor=\"Transparent\" /> \r\n    </ChartAreas>\r\n    <Legends>\r\n        <Legend _Template_=\"All\" BackColor=\"Transparent\" Font=\"Trebuchet MS, 8.25pt, style=Bold\" IsTextAutoFit=\"False\" /> \r\n    </Legends>\r\n    <BorderSkin SkinStyle=\"Emboss\" /> \r\n  </Chart>";
            var chart = new SimpleChart.Chart(width: 800, height: 300, theme: Blue).AddTitle(("Membership function: " + nameLv)).AddLegend();
           
            for (int i = 0; i < FKB.ListVar.Count; i++)
            {
                if (FKB.ListVar[i].Name == nameLv)
                {
                    chart.AddSeries(
                      name: "Numeric Value",
                      chartType: "Line",
                      xValue: new[] { FKB.ListVar[i].NumericValue, FKB.ListVar[i].NumericValue},
                      yValues: new[] { 0, 1});
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
        public ActionResult GetVievAgregation()
        {
            Agregation.AgregationStart(FKB);
      
            return View(FKB);
        }
        public ActionResult GetVievAccumulation()
        {
            Rule resultRule = new Rule();
            double maxvalue = Double.NegativeInfinity;
            for (int i = 0; i < FKB.ListOfRule.Count; i++)
            {
                if (FKB.ListOfRule[i].MinZnach > maxvalue)
                {
                    resultRule = FKB.ListOfRule[i];
                    maxvalue = FKB.ListOfRule[i].MinZnach;
                }
            }
            return View(resultRule);
        }
        public ActionResult GetVievDefuzzication()
        {
            result = Accumulation.AccumulationStart(FKB);
            Defuzzication.DefuzzicationStart(result);
            return View(result);
        }
        
        //public ActionResult CreateChartDefuzzication(string nameLv, double ValueFp,double ResultСonclusion)
        //{
        //    const string Blue = "<Chart BackColor=\"#D3DFF0\" BackGradientStyle=\"TopBottom\" BackSecondaryColor=\"White\" BorderColor=\"26, 59, 105\" BorderlineDashStyle=\"Solid\" BorderWidth=\"15\" Palette=\"BrightPastel\">\r\n    <ChartAreas>\r\n        <ChartArea Name=\"Default\" _Template_=\"All\" BackColor=\"64, 165, 191, 228\" BackGradientStyle=\"TopBottom\" BackSecondaryColor=\"White\" BorderColor=\"64, 64, 64, 64\" BorderDashStyle=\"Solid\" ShadowColor=\"Transparent\" /> \r\n    </ChartAreas>\r\n    <Legends>\r\n        <Legend _Template_=\"All\" BackColor=\"Transparent\" Font=\"Trebuchet MS, 8.25pt, style=Bold\" IsTextAutoFit=\"False\" /> \r\n    </Legends>\r\n    <BorderSkin SkinStyle=\"Emboss\" /> \r\n  </Chart>";
        //    var chart = new SimpleChart.Chart(width: 800, height: 300, theme: Blue).AddTitle(("Membership function: " + nameLv)).AddLegend();

        //    for (int i = 0; i < FKB.ListVar.Count; i++)
        //    {
        //        if (FKB.ListVar[i].Name == nameLv)
        //        {
        //            chart.AddSeries(
        //              name: "Центр Площ",
        //              chartType: "Line",
        //              xValue: new[] { result.NumericValue, result.NumericValue },
        //              yValues: new[] { 0, ValueFp });
        //            chart.AddSeries(
        //              name: "Value Fp",
        //              chartType: "Line",
        //              xValue: new[] {0,1 },
        //              yValues: new[] { ValueFp, ValueFp });
        //            chart.AddSeries(
        //                        name: FKB.ListVar[i].terms[0].Name,
        //                        chartType: "Line",
        //                        xValue: new[] { FKB.ListVar[i].terms[0].a, FKB.ListVar[i].terms[0].b, FKB.ListVar[i].terms[0].c },
        //                        yValues: new[] { 1, 1, 0 });


        //            for (int j = 1; j < FKB.ListVar[i].terms.Count - 1; j++)
        //            {

        //                chart.AddSeries(
        //                name: FKB.ListVar[i].terms[j].Name,
        //                chartType: "Line",
        //                xValue: new[] { FKB.ListVar[i].terms[j].a, FKB.ListVar[i].terms[j].b, FKB.ListVar[i].terms[j].c },
        //                yValues: new[] { 0, 1, 0 });

        //            }
        //            chart.AddSeries(
        //                       name: FKB.ListVar[i].terms[FKB.ListVar[i].terms.Count - 1].Name,
        //                       chartType: "Line",
        //                       xValue: new[] { FKB.ListVar[i].terms[FKB.ListVar[i].terms.Count - 1].a, FKB.ListVar[i].terms[FKB.ListVar[i].terms.Count - 1].b, FKB.ListVar[i].terms[FKB.ListVar[i].terms.Count - 1].c },
        //                       yValues: new[] { 0, 1, 1 })
        //                       .Write();

        //            break;
        //        }
        //    }
        //    return null;
        //}
        
    }
}