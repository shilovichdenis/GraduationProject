using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class Student
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        public string UserId { get; set; }
        public Group Group { get; set; }
        public int GroupId { get; set; }
        public double AverageRating { get; set; }
        public string FormOfEducation { get; set; }
        [NonSerialized]
        public RecordBook RecordBook;

        public Student()
        {
        }

        public Student(string userId, int groupId, string formOfEducation)
        {
            UserId = userId;
            GroupId = groupId;
            FormOfEducation = formOfEducation;
        }
    }
}