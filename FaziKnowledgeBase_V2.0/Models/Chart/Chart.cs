using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaziKnowledgeBase_V2._0.Models.Chart
{
    public class Chart
    {
        public List<Line> SimplyLines { get; set; } = new List<Line>();
        public List<Line> BoldLines { get; set; } = new List<Line>();
        public List<Point> points { get; set; } = new List<Point>();
        public string NameChart { get; set; }
    }
}