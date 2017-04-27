using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FuzzyKnowledgeBase_V2._0.Models
{
    public class LinguisticVariable
    {
        public Guid ID { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public List<Term> terms;
        public double min = 0;
        public double max = 0;
        public string ShortName = "";
        public double NumericValue=0;
        public LinguisticVariable()
        {
        }
        public LinguisticVariable(Guid IDLing, String NameLing, List<Term> termLing, double min, double max, String DesreptionLing = " ", String Short = "")
        {
            this.ID = IDLing;
            this.Name = NameLing;
            this.Description = DesreptionLing;
            this.terms = termLing;
            this.ShortName = Short;
        }
    }
}