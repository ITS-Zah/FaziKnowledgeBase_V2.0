using FuzzyKnowledgeBase_V2._0.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;

namespace FaziKnowledgeBase_V2._0.FKB.Helper
{
    public class FKBHelper
    {
        public static int ostanovkaLP = 0;
        public static int ostanovkaTM = 0;
        public static void Save_BNZ(FuzzyKnowledgeBase FKB, string path2)  // path2 где сохраняем файлик с БНЗ
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(FuzzyKnowledgeBase));

            using (FileStream fs = new FileStream(path2, FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, FKB);
            }
        }
        public static void WithRullToVar(FuzzyKnowledgeBase FKB)
        {
            ostanovkaLP = 0;
            for (int rule = ostanovkaLP; rule < FKB.ListOfRule.Count; rule++)
            {
                for (int anc = 0; anc < FKB.ListOfRule[rule].Antecedents.Count; anc++)
                {
                    List<Term> spusokTermans = new List<Term>();
                    for (int termforlist = ostanovkaLP; termforlist < FKB.ListOfRule.Count; termforlist++)
                    {
                        bool provlistterms = true;
                        Term tm = new Term(new Guid(), FKB.ListOfRule[termforlist].Antecedents[anc].Name, FKB.ListOfRule[termforlist].Antecedents[anc].NameLP);
                        if (spusokTermans.Count == 0)
                        {
                            tm.a = FKB.ListOfRule[termforlist].Antecedents[anc].a;
                            tm.b = FKB.ListOfRule[termforlist].Antecedents[anc].b;
                            tm.c = FKB.ListOfRule[termforlist].Antecedents[anc].c;
                            tm.d = FKB.ListOfRule[termforlist].Antecedents[anc].d;
                            tm.ProverkTruk = FKB.ListOfRule[termforlist].Antecedents[anc].ProverkTruk;
                            tm.WeightOfTerm = FKB.ListOfRule[termforlist].Antecedents[anc].WeightOfTerm;
                            spusokTermans.Add(tm);
                        }
                        else
                        {
                            for (int t = 0; t < spusokTermans.Count; t++)
                            {
                                if (spusokTermans[t].Name == tm.Name)
                                {
                                    provlistterms = false;
                                    break;
                                }
                            }
                            if (provlistterms == true)
                            {
                                tm.a = FKB.ListOfRule[termforlist].Antecedents[anc].a;
                                tm.b = FKB.ListOfRule[termforlist].Antecedents[anc].b;
                                tm.c = FKB.ListOfRule[termforlist].Antecedents[anc].c;
                                tm.d = FKB.ListOfRule[termforlist].Antecedents[anc].d;
                                tm.ProverkTruk = FKB.ListOfRule[termforlist].Antecedents[anc].ProverkTruk;
                                tm.WeightOfTerm = FKB.ListOfRule[termforlist].Antecedents[anc].WeightOfTerm;
                                spusokTermans.Add(tm);
                            }
                        }


                    }
                    LinguisticVariable lpans = new LinguisticVariable(new Guid(), FKB.ListOfRule[rule].Antecedents[anc].NameLP, spusokTermans, 0, 1);
                    if (FKB.ListVar.Count == 0)
                    {
                        FKB.ListVar.Add(lpans);
                    }
                    else
                    {
                        bool provirkaLP = true;
                        for (int n = 0; n < FKB.ListVar.Count; n++)
                        {
                            if (FKB.ListVar[n].Name == lpans.Name)
                            {
                                for (int termlpj = 0; termlpj < lpans.terms.Count; termlpj++)
                                {
                                    bool provtermlp = true;
                                    for (int termlpi = 0; termlpi < FKB.ListVar[n].terms.Count; termlpi++)
                                    {
                                        if (FKB.ListVar[n].terms[termlpi].Name == lpans.terms[termlpj].Name)
                                        {
                                            provtermlp = false;
                                        }
                                    }
                                    if (provtermlp == true)
                                    {
                                        FKB.ListVar[n].terms.Add(lpans.terms[termlpj]);
                                    }
                                }
                                provirkaLP = false;
                                break;
                            }
                        }
                        if (provirkaLP == true)
                        {
                            FKB.ListVar.Add(lpans);
                        }
                    }

                }

                List<Term> spusokTerm = new List<Term>();
                for (int termforlist = ostanovkaLP; termforlist < FKB.ListOfRule.Count; termforlist++, ostanovkaLP++)
                {
                    bool provlistterms = true;
                    Term tm = new Term(new Guid(), FKB.ListOfRule[termforlist].Cоnsequens.Name, FKB.ListOfRule[termforlist].Cоnsequens.NameLP);
                    if (spusokTerm.Count == 0)
                    {
                        tm.a = FKB.ListOfRule[termforlist].Cоnsequens.a;
                        tm.b = FKB.ListOfRule[termforlist].Cоnsequens.b;
                        tm.c = FKB.ListOfRule[termforlist].Cоnsequens.c;
                        tm.d = FKB.ListOfRule[termforlist].Cоnsequens.d;
                        tm.ProverkTruk = FKB.ListOfRule[termforlist].Cоnsequens.ProverkTruk;
                        tm.WeightOfTerm = FKB.ListOfRule[termforlist].Cоnsequens.WeightOfTerm;
                        spusokTerm.Add(tm);
                    }
                    else
                    {
                        for (int t = 0; t < spusokTerm.Count; t++)
                        {
                            if (spusokTerm[t].Name == tm.Name)
                            {
                                provlistterms = false;
                                break;
                            }
                        }
                        if (provlistterms == true)
                        {
                            tm.a = FKB.ListOfRule[termforlist].Cоnsequens.a;
                            tm.b = FKB.ListOfRule[termforlist].Cоnsequens.b;
                            tm.c = FKB.ListOfRule[termforlist].Cоnsequens.c;
                            tm.d = FKB.ListOfRule[termforlist].Cоnsequens.d;
                            tm.ProverkTruk = FKB.ListOfRule[termforlist].Cоnsequens.ProverkTruk;
                            tm.WeightOfTerm = FKB.ListOfRule[termforlist].Cоnsequens.WeightOfTerm;
                            spusokTerm.Add(tm);
                        }
                    }
                }
                LinguisticVariable lp = new LinguisticVariable(new Guid(), FKB.ListOfRule[rule].Cоnsequens.NameLP, spusokTerm, 0, 1);
                if (FKB.ListVar.Count == 0)
                {
                    FKB.ListVar.Add(lp);
                }
                else
                {
                    bool provlp = true;
                    for (int n = 0; n < FKB.ListVar.Count; n++)
                    {

                        if (FKB.ListVar[n].Name == lp.Name)
                        {
                            for (int termlpj = 0; termlpj < lp.terms.Count; termlpj++)
                            {
                                bool provtermlp = true;
                                for (int termlpi = 0; termlpi < FKB.ListVar[n].terms.Count; termlpi++)
                                {
                                    if (FKB.ListVar[n].terms[termlpi].Name == lp.terms[termlpj].Name)
                                    {
                                        provtermlp = false;
                                    }
                                }
                                if (provtermlp == true)
                                {
                                    FKB.ListVar[n].terms.Add(lp.terms[termlpj]);
                                }
                            }
                            provlp = false;
                            break;
                        }
                    }
                    if (provlp == true)
                    {
                        FKB.ListVar.Add(lp);
                    }
                }
                break;
            }
        }
        public static void EditLinguisticVariable(FuzzyKnowledgeBase FKB, string OldNameLv, string NewNameLv)
        {
            foreach (var lv in FKB.ListVar)
            {
                if(lv.Name == OldNameLv)
                {
                    lv.Name = NewNameLv;
                    break;//Імя ЛП унікальне, тому повторних співпадінь бути не може
                }
            }
            foreach (var rule in FKB.ListOfRule)
            {
                if(rule.Cоnsequens.NameLP == OldNameLv)
                {
                    rule.Cоnsequens.NameLP = NewNameLv;
                }
                foreach (var term in rule.Antecedents)
                {
                    if(term.NameLP == OldNameLv)
                    {
                        term.NameLP = NewNameLv;
                    }
                }
            }
        }

    }

}