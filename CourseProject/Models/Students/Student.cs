using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseProject.Models.Students
{
    public class Student
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        public string UserId { get; set; }
        public Group Group { get; set; }
        public int GroupId { get; set; }
        [Display(Name = "Средний балл")]
        public double AverageRating { get; set; }
        [Display(Name = "Форма оплаты")]
        public string FormOfPayment{ get; set; }
        [NonSerialized]
        public RecordBook RecordBook;

        public Student()
        {
        }

        public Student(string userId, int groupId, string formOfPayment)
        {
            UserId = userId;
            GroupId = groupId;
            FormOfPayment = formOfPayment;
        }
    }
}