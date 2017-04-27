using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace FaziKnowledgeBase_V2._0.FKB.DataStructures
{
    public class Cluster
    {
        public MultiDimensionalVector Centroid { get; set; }
        //public double medoid { get; set; }
        public List<MultiDimensionalVector> Elements { get; set; }
        public Color color { get; set; }

        public Cluster() : this(new MultiDimensionalVector(), new List<MultiDimensionalVector>()) { }
        public Cluster(MultiDimensionalVector Centroid) : this(Centroid, new List<MultiDimensionalVector>()) { }
        public Cluster(MultiDimensionalVector Centroid, List<MultiDimensionalVector> Elements)
        {
            this.Centroid = Centroid;
            this.Elements = Elements;
            this.color = new Color();
        }
    }
}