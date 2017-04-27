using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FuzzyKnowledgeBase_V2._0.Models;

namespace FuzzyKnowledgeBase_V2._0.ActionsFKB
{
    public class Agregation 
    {
        public static void AgregationStart(FuzzyKnowledgeBase fkb)
        {
            for(int i =0; i < fkb.ListOfRule.Count; i++)//по всім правилам
            {
                double minvalue = Double.PositiveInfinity;
                for (int j = 0; j < fkb.ListOfRule[i].Antecedents.Count; j++)//по всім параметрам правила
                {
                    if(fkb.ListOfRule[i].Antecedents[j].ZnachFp < minvalue)
                    {
                        minvalue = fkb.ListOfRule[i].Antecedents[j].ZnachFp;
                    }
                }
                fkb.ListOfRule[i].MinZnach = minvalue;
            }
            
        }
    }
}