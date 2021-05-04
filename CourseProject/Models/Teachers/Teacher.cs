using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseProject.Models.Teachers
{
    public class Teacher
    {
        public Teacher()
        {
        }

        public Teacher(string userId, int cathedraId)
        {
            UserId = userId;
            CathedraId = cathedraId;
        }

        public int Id { get; set; }
        [Display(Name = "Преподаватель")]
        public ApplicationUser User { get; set; }

        [Required]
        public string UserId { get; set; }
        [Display(Name = "Кафедра")]
        public Cathedra Cathedra { get; set; }
        public int? CathedraId { get; set; }
        [Display(Name = "Ученое звание")]
        public string AcademicTitle { get; set; }
        [Display(Name = "Ученая степень")]
        public string AcademicDegree { get; set; }
        public List<ScientificWork> ScientificWorks { get; set; }
        public List<Publication> Publications { get; set; }
    }

    public class EducationalMaterials
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public byte[] File {get; set;}
    }
}