using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class StudyGroups
    {
        public int Id { get; set; }
        [Required]
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        [Required]
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public StudyGroups() { }
        public StudyGroups(Group group, Teacher teacher)
        {
            Group = group;
            Teacher = teacher;
        }
    }
}