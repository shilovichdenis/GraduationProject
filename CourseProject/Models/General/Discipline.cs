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
        [Display(Name = "Семестр")]
        public int Term { get; set; }

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
        public void SetTerm()
        {
            foreach (var dt in dateTerms)
            {
                var start = dt.Start.AddYears(Group.YearOfAdmission - 1);
                var end = dt.End.AddYears(Group.YearOfAdmission - 1);
                if (start <= DateTime && DateTime <= end)
                {
                    Term = dt.Number;
                    return;
                }
            }
        }

        List<DateTerm> dateTerms = new List<DateTerm>()
        {
            new DateTerm(1, new DateTime(0001, 11, 01), new DateTime(0002, 02, 01)),
            new DateTerm(2, new DateTime(0002, 05, 01), new DateTime(0002, 08, 01)),
            new DateTerm(3, new DateTime(0002, 11, 01), new DateTime(0003, 02, 01)),
            new DateTerm(4, new DateTime(0003, 05, 01), new DateTime(0003, 08, 01)),
            new DateTerm(5, new DateTime(0003, 11, 01), new DateTime(0004, 02, 01)),
            new DateTerm(6, new DateTime(0004, 05, 01), new DateTime(0004, 08, 01)),
            new DateTerm(7, new DateTime(0004, 11, 01), new DateTime(0005, 02, 01)),
            new DateTerm(8, new DateTime(0005, 05, 01), new DateTime(0005, 08, 01))
        };

    }
}
public class DateTerm
{
    public DateTerm(int number, DateTime start, DateTime end)
    {
        Number = number;
        Start = start;
        End = end;
    }

    public int Number { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

}
