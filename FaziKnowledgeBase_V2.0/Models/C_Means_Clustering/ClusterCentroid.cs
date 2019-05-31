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

        public List<double> ParametersValues { get; set; }

        public ClusterCentroid(int row, List<double> parameters)
        {
            ParametersValues = parameters;
            Row = row;

            ValueCount = 0;
            MembershipSum = 0;

            Value = 1;

            foreach (var parameter in ParametersValues)
            {
                Value *= parameter;
            }

            var pow = 1 / ParametersValues.Count;
            Value = Math.Pow(Value, pow);
        }
    }
}