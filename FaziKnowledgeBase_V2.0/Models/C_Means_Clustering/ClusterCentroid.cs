using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaziKnowledgeBase_V2._0.Models.C_Means_Clustering
{
    public class ClusterCentroid
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public double ValueCount { get; set; }
        
        public double MembershipSum { get; set; }
        public double Value { get; set; }

        public ClusterCentroid(int column, int row, double value)
        {
            this.Column = column;
            this.Row = row;
            this.ValueCount = 0;
            this.MembershipSum = 0;
            this.Value = value;
        }
    }
}