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
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Кабинет")]
        public string DepartmentOffice { get; set; }
        [Required]
        [Display(Name = "Корпус")]
        public string Building { get; set; }
        public List<Teacher> Teachers { get; set; }

    }
}