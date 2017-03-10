using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FuzzyKnowledgeBase_V2._0.Models;

namespace FuzzyKnowledgeBase_V2._0.ActionsFKB
{
    public class Defuzzication
    {
        public static void DefuzzicationStart(Term term)
        {
            term.ZnachFp = term.ZnachFp * (term.b - term.a) + term.a;
        }
    }
}