using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaziKnowledgeBase_V2._0.Models.DataStructures
{
    public class Experiment
    {
        private List<double> xPart;

        private double yPart;

        public Experiment()
        {
        }

        public Experiment(double y, List<double> x)
        {
            yPart = y;
            xPart = x;
        }

        public List<double> getXPart()
        {
            return xPart;
        }

        public double getYPart()
        {
            return yPart;
        }

        public void setXPart(List<double> x)
        {
            xPart = x;
        }

        public void setYPart(double y)
        {
            yPart = y;
        }

    }
}