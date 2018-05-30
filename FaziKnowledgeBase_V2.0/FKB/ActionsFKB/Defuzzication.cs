using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FuzzyKnowledgeBase_V2._0.Models;

namespace FuzzyKnowledgeBase_V2._0.ActionsFKB
{
    public class Defuzzication
    {
        public static Term DefuzzicationStart(List<Rule> ListRule)
        {
            Term res = new Term();
            double max = double.MinValue;
            for(int i = 0; i < ListRule.Count; i++)
            {
                if(ListRule[i].MinZnach > max)
                {
                    max = ListRule[i].MinZnach;
                    res = ListRule[i].Cоnsequens;
                }
            }
            res.NumericValue = res.b;//центр максимума
            res.ZnachFp = max;
            return res;
        }
    }
}