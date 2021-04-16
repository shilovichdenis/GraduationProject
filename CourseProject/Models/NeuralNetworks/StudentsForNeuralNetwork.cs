using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CourseProject.Models.Students;

namespace CourseProject.Models.NeuralNetworks
{
    public class StudentsForNeuralNetwork
    {
        public ApplicationUser Student { get; set; }
        public RecordBook RecordBook { get; set; }
        public bool GetAllTests { get; set; }
        public int NumberInGroup { get; set; }
        public StudentsForNeuralNetwork()
        {

        }
        public StudentsForNeuralNetwork(int numberingroup, ApplicationUser user, RecordBook recordBook)
        {
            NumberInGroup = numberingroup;
            Student = user;
            RecordBook = recordBook;
            GetAllTests = true;
            var tests = recordBook.Tests;
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