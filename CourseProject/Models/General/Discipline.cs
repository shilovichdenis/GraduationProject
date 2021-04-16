using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CourseProject.Models.Students;
using CourseProject.Models.Teachers;

namespace CourseProject.Models.General
{
    public class Discipline
    {
        [Display(Name = "Ид")]
        public int Id { get; set; }

        [Display(Name = "Название")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Дата сдачи")]
        [Required]
        public DateTime DateTime { get; set; }

        [Display(Name = "Корпус")]
        [Required]
        public int Building { get; set; }

        [Display(Name = "Аудитория")]
        [Required]
        public int Audience { get; set; }

        [Display(Name = "Преподаватель")]
        public Teacher Teacher { get; set; }
        [Required]
        public int TeacherId { get; set; }

        [Display(Name = "Тип")]
        [Required]
        public bool IsExam { get; set; }

        [Display(Name = "Статус дисциплины")]
        public bool IsPassed { get; set; }

        [Display(Name = "Группа")]
        public Group Group { get; set; }

        [Display(Name = "Ид группы")]
        [Required]
        public int GroupId { get; set; }

        [Display(Name = "Количество часов")]
        public string СountHours { get; set; }

        public string Type
        {
            get
            {
                return IsExam ? "Экзамен" : "Зачет";
            }
        }
        public string Status
        {
            get
            {
                return IsPassed ? "Сдана" : "Не сдана";
            }
        }
    }
}