using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CourseProject.Models.Students;

namespace CourseProject.Models.General
{
    public class Statement
    {
        public Statement()
        {
        }

        public Statement(int studentId, int disciplineId)
        {
            StudentId = studentId;
            DisciplineId = disciplineId;
        }

        public Statement(int studentId, int disciplineId, int rating) : this(studentId, disciplineId)
        {
            Rating = rating;
        }

        public int Id { get; set; }
        public Student Student { get; set; }
        [Required]
        public int StudentId { get; set; }
        public Discipline Discipline { get; set; }
        [Required]
        public int DisciplineId { get; set; }
        public int Rating { get; set; }
    }
}