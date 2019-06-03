using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaziKnowledgeBase_V2._0.Models.C_Means_Clustering
{
    public class ClusterPoint
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public List<double> ParametersValues { get; set; }
        public double Value { get; set; }
        public double ClusterIndex { get; set; }

        public ClusterPoint(int row, List<double> parameters)
        {
            //Column = column;
            Row = row;
            ParametersValues = parameters;
            ClusterIndex = -1;

            Value = 1;

            foreach (var parameter in ParametersValues)
            {
                Value *= parameter;
            }

            double pow = 1 / (double)ParametersValues.Count;
            Value = Math.Pow(Value, pow);
        }


    }
}