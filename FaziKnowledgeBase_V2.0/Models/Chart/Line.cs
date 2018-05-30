using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaziKnowledgeBase_V2._0.Models.Chart
{
    public class Line
    {
        public List<Point> Points { get; set; }
        public bool IsBold { get; set; }
        public Line(List<Point> listPoints, bool isBold)
        {
            Points = listPoints;
            IsBold = isBold;
        }
    }
}