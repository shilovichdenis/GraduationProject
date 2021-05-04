using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseProject.Models.Students
{
    public class Group
    {
        [Display(Name = "Ид")]
        public int Id { get; set; }

        [Display(Name = "Номер группы")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Факультет")]
        [Required]
        public string Faculty { get; set; }

        public int FacultyId { get; set; }

        [Display(Name = "Специальность")]
        public string Specialty { get; set; }

        public int SpecialtyId { get; set; }

        [Display(Name = "Форма получения образования")]
        public string FormOfEducation { get; set; }

        [Display(Name = "Год поступления")]
        [Required]
        public int YearOfAdmission { get; set; }

        [Display(Name = "Студенты")]
        public List<Student> Students { get; set; }
        public Group()
        {
        }

        public Group(string name, List<Student> students)
        {
            Name = name;
            Students = students;
        }

        public Group(string name, string faculty, string specialty, string formOfEducation, int yearOfAdmission)
        {
            Name = name;
            Faculty = faculty;
            Specialty = specialty;
            FormOfEducation = formOfEducation;
            YearOfAdmission = yearOfAdmission;
        }
        public List<Faculty> Faculties = new List<Faculty>() {
            new Faculty(1, 101, "Автотракторный факультет", "АТФ"),
            new Faculty(2, 102, "Факультет горного дела и инженерной экологии", "ФГДЭ"),
            new Faculty(3, 103, "Машиностроительный факультет", "МСФ"),
            new Faculty(4, 104, "Механико-технологический факультет", "МТФ"),
            new Faculty(5, 105, "Факультет маркетинга, менеджмента и предпринимательства", "ФММП"),
            new Faculty(6, 106, "Энергетический факультет", "ЭФ"),
            new Faculty(7, 107, "Факультет информационных технологий и робототехники", "ФИТР"),
            new Faculty(8, 108, "Факультет технологий управления и гуманитаризации", "ФТУГ"),
            new Faculty(9, 109, "Инженерно-педагогический факультет", "ИПФ"),
            new Faculty(10, 110, "Факультет энергетического строительства", "ФЭС"),
            new Faculty(11, 111, "Архитектурный факультет", "АФ"),
            new Faculty(12, 112, "Строительный факультет", "СФ"),
            new Faculty(13, 113, "Приборостроительный факультет", "ПСФ"),
            new Faculty(14, 114, "Факультет транспортных коммуникаций", "ФТК"),
            new Faculty(15, 115, "Военно-технический факультет", "ВТФ"),
            new Faculty(16, 119, "Спортивно-технический факультет", "СТФ")
        };

        public List<Specialty> Specialties = new List<Specialty>() {
                new Specialty(1, 7, 1, "Программное обеспечение информационных технологий"),
                new Specialty(2, 7, 2, "Информационные системы и технологии в обработке и представлении информации"),
                new Specialty(3, 7, 2, "Информационные системы и технологии в проектировании и производстве"),
                new Specialty(4, 7, 3, "Автоматизация технологических процессов и производств"),
                new Specialty(5, 7, 5, "Автоматизированные электроприводы"),
                new Specialty(6, 7, 6, "Промышленные роботы и робототехнические комплексы"),
                new Specialty(7, 1, "Автомобилестроение (механика)"),
                new Specialty(8, 1, "Автомобилестроение (электроника)"),
                new Specialty(9, 1, "Двигатели внутреннего сгорания"),
                new Specialty(10, 1, "Техническая эксплуатация автомобилей"),
                new Specialty(11, 1, "Автосервис"),
                new Specialty(12, 1, "Тракторостроение"),
                new Specialty(13, 1, "Многоцелевые гусеничные и колесные машины"),
                new Specialty(14, 1, "Дизайн гусеничных и колесных машин"),
                new Specialty(15, 1, "Электрический и автономный транспорт")
        };
    }
    public class Faculty
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public List<Specialty> Specialties { get; set; }
        public Faculty()
        {
        }
        public Faculty(int number, string name)
        {
            Number = number;
            Name = name;
        }

        public Faculty(int number, string name, string abbreviation) : this(number, name)
        {
            Abbreviation = abbreviation;
        }

        public Faculty(int number, string name, string abbreviation, List<Specialty> specialties) : this(number, name, abbreviation)
        {
            Specialties = specialties;
        }

        public Faculty(int id, int number, string name, string abbreviation)
        {
            Id = id;
            Number = number;
            Name = name;
            Abbreviation = abbreviation;
        }
    }
    public class Specialty
    {
        public int Id { get; set; }
        public int FacultyId { get; set; }
        public int CodeNumber { get; set; }
        public string Name { get; set; }

        public Specialty()
        {
        }

        public Specialty(int id, int facultyId, string name)
        {
            Id = id;
            FacultyId = facultyId;
            Name = name;
        }

        public Specialty(int id, int facultyId, int codeNumber, string name)
        {
            Id = id;
            FacultyId = facultyId;
            CodeNumber = codeNumber;
            Name = name;
        }
    }
}

