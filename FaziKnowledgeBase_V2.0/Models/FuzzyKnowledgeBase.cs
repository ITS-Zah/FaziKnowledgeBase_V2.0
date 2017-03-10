using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace FuzzyKnowledgeBase_V2._0.Models
{
    [DataContract]
    public class FuzzyKnowledgeBase
    {
        [DataMember]
        public List<LinguisticVariable> ListVar = new List<LinguisticVariable>();

        [DataMember]
        public List<Rule> ListOfRule = new List<Rule>();
    }
}