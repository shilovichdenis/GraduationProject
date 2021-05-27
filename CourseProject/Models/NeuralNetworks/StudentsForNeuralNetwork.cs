using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CourseProject.Models.Students;

namespace CourseProject.Models.NeuralNetworks
{
    public class StudentsForNeuralNetwork
    {
        public Student Student { get; set; }
        public bool GetAllTests { get; set; }
        public int NumberInGroup { get; set; }
        public StudentsForNeuralNetwork()
        {

        }
        public StudentsForNeuralNetwork(int numberingroup, Student student)
        {
            NumberInGroup = numberingroup;
            Student = student;
            GetAllTests = true;
            var tests = Student.RecordBook.Tests;
            foreach (var test in tests)
            {
                if(test.Rating != "1")
                {
                    GetAllTests = false;
                    break;
                }
            }
        }
    }
}