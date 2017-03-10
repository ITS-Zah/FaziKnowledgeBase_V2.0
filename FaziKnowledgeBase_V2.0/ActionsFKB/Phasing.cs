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
    }
}