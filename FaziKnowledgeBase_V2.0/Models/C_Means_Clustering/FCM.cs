using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaziKnowledgeBase_V2._0.Models.C_Means_Clustering
{
    public class FCM
    {
        private List<ClusterPoint> Points;

        private List<ClusterCentroid> Clusters;

        public double[,] U;

        private double Fuzzyness;

        private double Eps = Math.Pow(10, -5);

        public double J { get; set; }



        public FCM(List<ClusterPoint> points, List<ClusterCentroid> clusters, float fuzzy, int row, int column, int numCluster)
        {
            this.Points = points;
            this.Clusters = clusters;

            U = new double[this.Points.Count, this.Clusters.Count];
            this.Fuzzyness = fuzzy;

            double diff;

            // Iterate through all points to create initial U matrix
            for (int i = 0; i < this.Points.Count; i++)
            {
                ClusterPoint p = this.Points[i];
                double sum = 0.0;

                for (int j = 0; j < this.Clusters.Count; j++)
                {
                    ClusterCentroid c = this.Clusters[j];
                    diff = CalculateEuclideanDistance(p, c);
                    if (diff == 0)
                    {
                        U[i, j] = Eps;
                    }
                    else
                    {
                        U[i, j] = diff;
                    }
                    sum += U[i, j];
                }
            }

            this.RecalculateClusterMembershipValues();
        }

        private void RecalculateClusterMembershipValues()
        {
            for (int i = 0; i < this.Points.Count; i++)
            {
                double max = 0.0;
                double min = 0.0;
                double sum = 0.0;
                double newmax = 0;
                var p = this.Points[i];
                //Normalize the entries
                for (int j = 0; j < this.Clusters.Count; j++)
                {
                    if (U[i, j] > max)
                    {
                        max = U[i, j];
                    }

                    if (U[i, j] < min)
                    {
                        min = U[i, j];
                    }
                }
                //Sets the values to the normalized values between 0 and 1
                for (int j = 0; j < this.Clusters.Count; j++)
                {
                    U[i, j] = (U[i, j] - min) / (max - min);
                    sum += U[i, j];
                }
                //Makes it so that the sum of all values is 1 
                for (int j = 0; j < this.Clusters.Count; j++)
                {
                    U[i, j] = U[i, j] / sum;
                    if (double.IsNaN(U[i, j]))
                    {

                        U[i, j] = 0.0;
                    }

                    newmax = U[i, j] > newmax ? U[i, j] : newmax;
                }
                // ClusterIndex is used to store the strongest membership value to a cluster, used for defuzzification
                p.ClusterIndex = newmax;
            };
        }

        internal void Step()
        {
            for (int c = 0; c < Clusters.Count; c++)
            {
                for (int h = 0; h < Points.Count; h++)
                {

                    double top;
                    top = CalculateEuclideanDistance(Points[h], Clusters[c]);

                    // sumTerms is the sum of distances from this data point to all clusters.
                    double sumTerms = 0.0;

                    for (int ck = 0; ck < Clusters.Count; ck++)
                    {
                        sumTerms += top / CalculateEuclideanDistance(Points[h], Clusters[ck]);

                    }
                    // Then the membership value can be calculated as...
                    U[h, c] = (double)(1.0 / Math.Pow(sumTerms, (2 / (this.Fuzzyness - 1))));
                }
            }


            this.RecalculateClusterMembershipValues();
        }

        internal void CalculateClusterCentroids()
        {
            for (int j = 0; j < this.Clusters.Count; j++)
            {
                ClusterCentroid c = this.Clusters[j];
                double l = 0.0;
                c.ValueCount = 1;
                c.Value = 0;
                c.MembershipSum = 0;

                for (int i = 0; i < this.Points.Count; i++)
                {

                    ClusterPoint p = this.Points[i];
                    l = Math.Pow(U[i, j], this.Fuzzyness);
                    c.Value += l * p.ValueCell;
                    c.MembershipSum += l;

                    if (U[i, j] == p.ClusterIndex)
                    {
                        c.ValueCount += 1;
                    }
                }

                c.Value = (c.Value / c.MembershipSum);
            }

           
        }
        public double CalculateObjectiveFunction()
        {
            double Jk = 0.0;

            for (int i = 0; i < this.Points.Count; i++)
            {
                for (int j = 0; j < this.Clusters.Count; j++)
                {
                    Jk += Math.Pow(U[i, j], this.Fuzzyness) * Math.Pow(this.CalculateEuclideanDistance(Points[i], Clusters[j]), 2);
                }
            }
            return Jk;
        }

        private double CalculateEuclideanDistance(ClusterPoint p, ClusterCentroid c)
        {
            return Math.Sqrt(Math.Pow(p.ValueCell - c.Value, 2.0));
        }
    }
}