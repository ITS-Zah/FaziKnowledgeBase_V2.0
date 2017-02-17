using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaziKnowledgeBase_V2._0.Models
{
    public class Term
    {
        public Guid ID { get; set; }
        public String Name { get; set; }
        public String NameLP { get; set; }
        public String ShortNameLP { get; set; }
        public String ShortNameTerm { get; set; }
        public double a { get; set; }                //где вектор х – базовое множество, на котором определяется ФП.
        public double c { get; set; }                //Величины а и с задают основание треугольника, b – его вершину.
        public double b { get; set; }
        public double sigmGaus { get; set; } //параметры Гаусовской ФП
        public double aGaus { get; set; } // параметры Гаусовской ФП

        public double d { get; set; }                //Величины а и d задают нижнее основание трапеции, b и с – верхнее
                                                     //основание.

        public double ZnachFp { get; set; }

        public bool ProverkTruk { get; set; }

        public bool ProverkTrap { get; set; }

        public bool ProverGaus { get; set; }

        public bool ProverLast { get; set; }

        public bool ProverFirst { get; set; }
        public bool ProverOut { get; set; }

        public int WeightOfTerm { get; set; }
        public double ag { get; set; }
        public double sigm { get; set; }
    }
}