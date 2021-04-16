using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CourseProject.Models.Students;
using CourseProject.Models.Teachers;

namespace CourseProject.Models.General
{
    public class Information
    {
        public int Id { get; set; }
        [Display(Name = "Информация")]
        [Required]
        public string Info { get; set; }
        [Display(Name = "Получатель")]
        public int RecieverId { get; set; }
        [Display(Name = "Получатель")]
        [NonSerialized]
        public Group Reciever;
        [Display(Name = "Оправитель")]
        public int SenderId { get; set; }
        [Display(Name = "Оправитель")]
        [NonSerialized]
        public Teacher Sender;
        [Display(Name = "Дата отправления")]
        [Required]
        public DateTime DateTime { get; set; }
        public Information()
        {
        }
        public Information(string info, int receiverId, int senderId)
        {
            Info = info;
            RecieverId = receiverId;
            SenderId = senderId;
        }

        public Information(string info, int recieverId, int senderId, DateTime dateTime) : this(info, recieverId, senderId)
        {
            DateTime = dateTime;
        }
    }
}