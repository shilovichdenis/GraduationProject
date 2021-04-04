using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class NeuralNetwork
    {
        Random random { get; set; }
        public double LowerBound { get; set; }
        public double UpperBound { get; set; }
        public double E { get; set; }


        public int M { get; set; }
        public int K { get; set; }
        public double F1 { get; set; }
        public double F2 { get; set; }
        public string Name { get; set; }
        public List<StudentsForNeuralNetwork> Students { get; set; }
        public Group Group { get; set; }

        public int CountNumericX { get; set; }
        public int[] MaxInColumn { get; set; }
        public int[] MinInColumn { get; set; }


        public double[,] NoramalizeCoeff { get; set; }
        public double[,] prevWij { get; set; }
        public double[,] Wij { get; set; }


        public double[,] StandardOfCoeff { get; set; }
        public double[] StandardOfGroup { get; set; }
        public List<List<StudentsForNeuralNetwork>> GroupOfStudents { get; set; }

        public NeuralNetwork(int m, int k, List<StudentsForNeuralNetwork> students, Group group)
        {
            random = new Random();
            M = m;
            K = k;
            E = 0.0001;
            Name = $"{M} - {K}";
            Students = students;
            Group = group;
            LowerBound = 0.5 - 1 / Math.Sqrt(M);
            UpperBound = 0.5 + 1 / Math.Sqrt(M);
            Wij = new double[K, M];
            prevWij = new double[K, M];
            NoramalizeCoeff = new double[Students.Count, M];
            FindMinMax();
            SetCoeff();
            SetWij();
            SetNormalizeWij();
            SetGroupOfStudents();
            SetF1();
            SetF2();
        }
        public bool Check(double[,] newWij, double[,] prevWij)
        {
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < K; j++)
                {
                    if ((double)(prevWij[j, i] - newWij[j, i]) > E)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private void FindMinMax()
        {
            CountNumericX = M - 3;

            MaxInColumn = new int[CountNumericX];
            MinInColumn = new int[CountNumericX];

            for (int i = 0; i < CountNumericX; i++)
            {
                MinInColumn[i] = 100;
            }

            foreach (var student in Students)
            {
                foreach (var (i, discipline) in student.RecordBook.Exams.Select((i, d) => (d, i)))
                {
                    if (MaxInColumn[i] < discipline.Rating)
                    {
                        MaxInColumn[i] = discipline.Rating;
                    }
                    if (MinInColumn[i] > discipline.Rating)
                    {
                        MinInColumn[i] = discipline.Rating;
                    }
                }
            }
        }
        private void SetCoeff()
        {
            for (int i = 0; i < Students.Count; i++)
            {
                if (Students[i].Student.Gender == "Мужской")
                {
                    NoramalizeCoeff[i, 0] = 1;
                }
                else
                {
                    NoramalizeCoeff[i, 0] = 0;
                }

                if (Students[i].GetAllTests)
                {
                    NoramalizeCoeff[i, 1] = 1;
                }
                else
                {
                    NoramalizeCoeff[i, 1] = 0;
                }
                if (Students[i].Student.OtherInfo == "Бюджетная")
                {
                    NoramalizeCoeff[i, 2] = 1;
                }
                else
                {
                    NoramalizeCoeff[i, 2] = 0;
                }

                for (int j = 0; j < M - 3; j++)
                {
                    NoramalizeCoeff[i, j + 3] = Math.Round((double)(Students[i].RecordBook.Exams[j].Rating - MinInColumn[j]) / (double)(MaxInColumn[j] - MinInColumn[j]), 2);
                }
            }
        }
        private void SetWij()
        {
            for (int i = 0; i < K; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    double temp = Math.Round(random.NextDouble(), 2);
                    while (temp < LowerBound || temp > UpperBound)
                    {
                        temp = Math.Round(random.NextDouble(), 2);
                    }
                    Wij[i, j] = temp;
                }
            }
        }
        private void SetNormalizeWij()
        {
            bool check;
            double[] R = new double[K];
            double coeffSpeed = 0.01;
            do
            {
                List<StudentsForNeuralNetwork> studentsForRemove = new List<StudentsForNeuralNetwork>();
                foreach (var student in Students)
                {
                    studentsForRemove.Add(student);
                }
                while (studentsForRemove.Count > 0)
                {
                    int indexStudent = random.Next(0, studentsForRemove.Count);
                    for (int i = 0; i < K; i++)
                    {
                        double sum = 0d;
                        for (int j = 0; j < M; j++)
                        {
                            sum += Math.Pow(NoramalizeCoeff[indexStudent, j] - Wij[i, j], 2);
                        }
                        R[i] = Math.Round(Math.Sqrt(sum), 2);
                    }

                    double minR = 100;
                    int indexR = 0;
                    for (int i = 0; i < K; i++)
                    {
                        if (minR > R[i])
                        {
                            minR = R[i];
                            indexR = i;
                        }
                    }

                    for (int i = 0; i < M; i++)
                    {
                        var temp = Wij[indexR, i];
                        Wij[indexR, i] = Math.Round(temp + coeffSpeed * (NoramalizeCoeff[indexStudent, i] - temp), 2);
                    }
                    studentsForRemove.RemoveAt(indexStudent);
                }
                check = Check(Wij, prevWij);
                prevWij = Wij;
            }
            while (!check);
        }
        private void SetGroupOfStudents()
        {
            GroupOfStudents = new List<List<StudentsForNeuralNetwork>>();
            double[] R = new double[K];
            for (int i = 0; i < K; i++)
            {
                GroupOfStudents.Add(new List<StudentsForNeuralNetwork>());
            }
            foreach (var (indexStudent, student) in Students.Select((i, s) => (s, i)))
            {

                for (int j = 0; j < K; j++)
                {
                    double sum = 0d;
                    for (int i = 0; i < M; i++)
                    {
                        sum += Math.Pow(NoramalizeCoeff[indexStudent, i] - Wij[j, i], 2);
                    }
                    R[j] = Math.Round(Math.Sqrt(sum), 2);
                }

                double minR = 100;
                int indexR = 0;
                for (int o = 0; o < K; o++)
                {
                    if (minR > R[o])
                    {
                        minR = R[o];
                        indexR = o;
                    }
                }
                GroupOfStudents[indexR].Add(student);
            }
        }

        private void SetF1()
        {
            F1 = 0d;
            StandardOfCoeff = new double[GroupOfStudents.Count, M];
            foreach (var (indexGroup, group) in GroupOfStudents.Select((i, g) => (g, i)))
            {
                double sumR = 0d;
                foreach (var (indexStudent, student) in group.Select((i, s) => (s, i)))
                {
                    for (int i = 0; i < M; i++)
                    {
                        StandardOfCoeff[indexGroup, i] += NoramalizeCoeff[student.NumberInGroup - 1, i];
                    }
                }
                for (int i = 0; i < M; i++)
                {
                    StandardOfCoeff[indexGroup, i] /= (double)group.Count;
                }

                foreach (var (indexStudent, student) in group.Select((i, s) => (s, i)))
                {
                    double sum = 0d;
                    for (int i = 0; i < M; i++)
                    {
                        sum += Math.Pow(StandardOfCoeff[indexGroup, i] - NoramalizeCoeff[student.NumberInGroup - 1, i], 4);
                    }
                    sumR += Math.Sqrt(sum);
                }
                if (group.Count != 0)
                {
                    sumR /= (double)group.Count;
                    F1 += sumR;
                }
                else
                {
                    F1 = 0;
                    return;
                }
            }
            F1 /= (double)GroupOfStudents.Count;
            F1 = Math.Round((double)F1, 4);
        }
        private void SetF2()
        {
            F2 = 0d;
            StandardOfGroup = new double[M];
            foreach (var (indexGroup, group) in GroupOfStudents.Select((i, g) => (g, i)))
            {
                for (int i = 0; i < M; i++)
                {
                    StandardOfGroup[i] += StandardOfCoeff[indexGroup, i];
                }
            }
            for (int i = 0; i < M; i++)
            {
                StandardOfGroup[i] /= (double)GroupOfStudents.Count;
            }
            foreach (var (indexGroup, group) in GroupOfStudents.Select((i, g) => (g, i)))
            {
                double sum = 0d;
                double sumR = 0d;
                for (int i = 0; i < M; i++)
                {
                    sum += Math.Pow(StandardOfGroup[i] - StandardOfCoeff[indexGroup, i], 2);
                }
                sumR += Math.Sqrt(sum);
                if(group.Count != 0)
                {
                    sumR /= (double)group.Count;
                    F2 += sumR;
                }
                else
                {
                    F2 = 0;
                    return;
                }
            }
            F2 /= (double)GroupOfStudents.Count;
            F2 = Math.Round((double)F2, 4);
        }
    }
}