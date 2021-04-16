using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CourseProject.Models.Students;

namespace CourseProject.Models.NeuralNetworks
{
    public class NeuralNetworkResult
    {
        public NeuralNetworkResult()
        {
        }

        public NeuralNetworkResult(double f1, double f2, int k,  int m, int groupId)
        {
            F1 = f1;
            F2 = f2;
            K = k;
            M = m;
            GroupId = groupId;
        }

        public int Id { get; set; }
        public double F1 { get; set; }
        public double F2 { get; set; }
        public int K { get; set; }
        public int M { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}