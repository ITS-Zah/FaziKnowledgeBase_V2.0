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
            string nameTerm = "";
            double maxvalue = Double.NegativeInfinity;
            for(int i = 0; i < fkb.ListOfRule.Count; i++)
            {
                if(fkb.ListOfRule[i].MinZnach > maxvalue)
                {
                    nameTerm = fkb.ListOfRule[i].Cоnsequens.Name;
                    maxvalue = fkb.ListOfRule[i].MinZnach;
                    //resultTerm.ZnachFp = maxvalue;
                }
            }
            for(int i =0; i < fkb.ListVar.Last().terms.Count; i++)
            {
                if(fkb.ListVar.Last().terms[i].Name == nameTerm)
                {
                    resultTerm = fkb.ListVar.Last().terms[i];
                    resultTerm.ZnachFp = maxvalue;
                    break;
                }
            }
            return resultTerm;
        } 
    }
}