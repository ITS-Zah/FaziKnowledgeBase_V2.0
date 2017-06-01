using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FuzzyKnowledgeBase_V2._0.Models;

namespace FuzzyKnowledgeBase_V2._0.ActionsFKB
{
    public class Accumulation
    {
        public static List<Rule> AccumulationStart(FuzzyKnowledgeBase fkb)
        {
            List<Rule> ListResultRule = new List<Rule>();
            foreach(Term term in fkb.ListVar.Last().terms)
            {
                double max = Double.MinValue;
                Rule maxRule = new Rule();
                foreach (Rule rule in fkb.ListOfRule)
                {
                    if(term.Name == rule.Cоnsequens.Name)
                    {
                        if (max < rule.MinZnach)
                        {
                            max = rule.MinZnach;
                            maxRule = rule;       
                        }
                    }
                }
                ListResultRule.Add(maxRule);
            }
            return ListResultRule;
        } 
    }
}