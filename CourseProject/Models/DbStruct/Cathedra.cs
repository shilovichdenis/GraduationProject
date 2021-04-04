using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class Cathedra
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int DepartmentOffice { get; set; }
        [Required]
        public string Building { get; set; }
        public List<Teacher> Teachers { get; set; }

    }
}