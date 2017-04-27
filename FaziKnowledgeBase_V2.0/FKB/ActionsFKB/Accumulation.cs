using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FuzzyKnowledgeBase_V2._0.Models;

namespace FuzzyKnowledgeBase_V2._0.ActionsFKB
{
    public class Accumulation
    {
        public static Term AccumulationStart(FuzzyKnowledgeBase fkb)
        {
            Term resultTerm = new Term();
            double maxvalue = Double.NegativeInfinity;
            for(int i = 0; i < fkb.ListOfRule.Count; i++)
            {
                if(fkb.ListOfRule[i].MinZnach > maxvalue)
                {
                    resultTerm = fkb.ListOfRule[i].Cоnsequens;
                    maxvalue = fkb.ListOfRule[i].MinZnach;
                    resultTerm.ZnachFp = maxvalue;
                }
            }
            return resultTerm;
        } 
    }
}