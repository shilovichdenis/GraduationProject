using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CourseProject.Models.Students;

namespace CourseProject.Models.General
{
    public class DisplayRatings
    {
        public DisplayRatings(List<Student> students, List<DisplayDisciplineModel> disciplines)
        {
            Students = students;
            Disciplines = disciplines;
        }

        public List<Student> Students { get; set; }
        public List<DisplayDisciplineModel> Disciplines { get; set; }
    }
    public class DisplayDisciplineModel
    {
        public DisplayDisciplineModel(Discipline discipline, List<Statement> statements)
        {
            Discipline = discipline;
            Statements = statements;
        }

        public Discipline Discipline { get; set; }
        public List<Statement> Statements { get; set; }
    }
}