using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseProject.Models.Teachers
{
    public class ScientificWork
    {
        public string Id { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Годы ")]
        public string Years { get; set; }
        public int TeacherId { get; set; }
        [Display(Name = "Преподаватель")]
        public Teacher Teacher { get; set; }
        [Display(Name = "О работе")]
        public string About { get; set; }
    }
}