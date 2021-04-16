using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CourseProject.Models.Students;
using CourseProject.Models.Teachers;

namespace CourseProject.Models.Students
{
    public class Project
    {
        public int Id { get; set; }
        public Student Student { get; set; }
        [Required]
        public int StudentId { get; set; }
        public Teacher Teacher { get; set; }
        [Required]
        public int TeacherId { get; set; }
        [Required]
        public string Name { get; set; }
        public int Rating { get; set; }
    }
}