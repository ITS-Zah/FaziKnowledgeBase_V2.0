using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FuzzyKnowledgeBase_V2._0.Models
{
    public class Term
    {
        private Guid ID {  get; set; }
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
        public double NumericValue = 0;
        public void EvaluatingFp (double valueLv)
        {
            if (ProverLast == false && ProverFirst == false)
            {
                if (valueLv < a)
                {
                    this.ZnachFp = 0;
                }
                if (valueLv >= a && valueLv <= b)
                {
                    this.ZnachFp = (valueLv - a) / (b - a);
                }

                if (valueLv >= b && valueLv <= c)
                {
                    this.ZnachFp = (c - valueLv) / (c - b);
                }
                if (valueLv > c)
                {
                    this.ZnachFp = 0;
                }
            }
            else if (ProverLast == true && ProverFirst == false)
            {
                if (valueLv < a)
                {
                    this.ZnachFp = 0;
                }
                if (valueLv >= a && valueLv <= b)
                {
                    this.ZnachFp = (valueLv - a) / (b - a);
                }

                if (valueLv >= b && valueLv <= c)
                {
                    this.ZnachFp = 1;
                }
                if (valueLv > c)
                {
                    this.ZnachFp = 1;
                }
            }
            else if (ProverLast == false && ProverFirst == true)
            {
                if (valueLv < a)
                {
                    this.ZnachFp = 1;
                }
                if (valueLv >= a && valueLv <= b)
                {
                    this.ZnachFp = 1;
                }

                if (valueLv >= b && valueLv <= c)
                {
                    this.ZnachFp = (c - valueLv) / (c - b);
                }
                if (valueLv > c)
                {
                    this.ZnachFp = 0;
                }
            }
        }
    }
}