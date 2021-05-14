using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseProject.Models.Teachers
{
    public class Cathedra
    {
        public Cathedra()
        {
        }

        public Cathedra(string name, string departmentOffice, string building)
        {
            Name = name;
            DepartmentOffice = departmentOffice;
            Building = building;
        }

        public int Id { get; set; }
        [Required]
        [Display(Name = "Название кафедры")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Кабинет")]
        public string DepartmentOffice { get; set; }
        [Required]
        [Display(Name = "Корпус")]
        public string Building { get; set; }
        [Display(Name = "О кафедре")]
        public string About { get; set; }
        public List<Teacher> Teachers { get; set; }
    }
}