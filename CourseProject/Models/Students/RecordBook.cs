using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CourseProject.Models.General;

namespace CourseProject.Models.Students
{
    public class Exams
    {
        public Exams(Discipline discipline, int rating)
        {
            Discipline = discipline;
            Rating = rating;
        }

        public Discipline Discipline { get; set; }
        public int Rating { get; set; }
    }
    public class Tests
    {
        public Tests(Discipline discipline, string rating)
        {
            Discipline = discipline;
            Rating = rating;
        }

        public Discipline Discipline { get; set; }
        public string Rating { get; set; }
    }
    public class RecordBook
    {
        public RecordBook()
        {
        }

        public RecordBook(List<Exams> exams)
        {
            Exams = exams;
        }

        public RecordBook(List<Exams> exams, List<Tests> tests)
        {
            Exams = exams;
            Tests = tests;
        }

        public RecordBook(List<Exams> exams, List<Tests> tests, List<Project> courseProjects) : this(exams, tests)
        {
            Projects = courseProjects;
        }

        public List<Exams> Exams { get; set; }
        public List<Tests> Tests { get; set; }
        public List<Project> Projects { get; set; }
    }
}