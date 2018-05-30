using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FuzzyKnowledgeBase_V2._0.Models;

namespace FuzzyKnowledgeBase_V2._0.ActionsFKB
{
    public class Phasing
    {
        public static void StartPhasing(FuzzyKnowledgeBase fkb, string[] parematrlLv)
        {
            for(int i =0; i< fkb.ListOfRule.Count; i++)// по всім правилам
            {
                for(int j = 0; j < fkb.ListOfRule[i].Antecedents.Count; j++)// по всім вхідним лп правила
                {
                    fkb.ListOfRule[i].Antecedents[j].EvaluatingFp(Convert.ToDouble(parematrlLv[j]));
                }
            }
        }
        public static void PhasingLv(FuzzyKnowledgeBase fkb, string[] parematrlLv)
        {
            StartPhasing(fkb, parematrlLv);
            for(int i = 0; i < fkb.ListVar.Count - 1; i++)
            {
                fkb.ListVar[i].NumericValue = Convert.ToDouble(parematrlLv[i]);
                for (int j = 0; j < fkb.ListVar[i].terms.Count; j++)
                {
                    if(j == 0)
                    {
                        fkb.ListVar[i].terms[j].ProverFirst = true;
                    }
                    if(j == fkb.ListVar[i].terms.Count -1 )
                    {
                        fkb.ListVar[i].terms[j].ProverLast = true;
                    }
                    fkb.ListVar[i].terms[j].EvaluatingFp(Convert.ToDouble(parematrlLv[i]));
                }
            }
        }
    }
}