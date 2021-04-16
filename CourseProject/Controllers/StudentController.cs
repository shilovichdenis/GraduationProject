using CourseProject.Models.Students;
using CourseProject.Models.Teachers;
using CourseProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CourseProject.Controllers
{
    [Authorize(Roles = "Student")]
    [RequireHttps]
    public class StudentController : Controller
    {
        ApplicationDbContext dbT = new ApplicationDbContext();

        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        [HttpGet]
        public async Task<ActionResult> InfoAboutYourself()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var student = dbT.Students.Where(a => a.UserId == user.Id).FirstOrDefault();
            student.User = user;
            student.Group = dbT.Groups.Where(a => a.Id == student.GroupId).FirstOrDefault();
            var dTests = dbT.Disciplines.Where(a => a.GroupId == student.GroupId).Where(a => !a.IsExam).Where(a => a.DateTime > DateTime.Today).Where(a => a.IsPassed == true).ToList();
            var tests = new List<Tests>();
            foreach (var test in dTests)
            {
                var teacher = dbT.Teachers.Where(a => a.Id == test.TeacherId).FirstOrDefault();
                teacher.User = dbT.Users.Where(a => a.Id == teacher.UserId).FirstOrDefault();
                test.Teacher = teacher;

                var statement = dbT.Statements.Where(a => a.DisciplineId == test.Id).Where(b => b.StudentId == student.Id).FirstOrDefault();
                tests.Add(new Tests(test, statement.Rating.ToString()));
            }

            var dExams = dbT.Disciplines.Where(a => a.GroupId == student.GroupId).Where(b => b.IsExam).Where(a => a.DateTime > DateTime.Today).Where(a => a.IsPassed == true).ToList();
            var exams = new List<Exams> { };
            foreach (var exam in dExams)
            {
                var teacher = dbT.Teachers.Where(a => a.Id == exam.TeacherId).FirstOrDefault();
                teacher.User = dbT.Users.Where(a => a.Id == teacher.UserId).FirstOrDefault();
                exam.Teacher = teacher;

                var statement = dbT.Statements.Where(a => a.DisciplineId == exam.Id).Where(b => b.StudentId == student.Id).FirstOrDefault();
                exams.Add(new Exams(exam, statement.Rating));
            }

            var corseprojects = dbT.Projects.Where(a => a.StudentId == student.Id).ToList();
            var recordBook = new RecordBook(exams, tests, corseprojects);
            student.RecordBook = recordBook;
            return View(student);
        }

        public async Task<ActionResult> InfoAboutGroup()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var student = dbT.Students.Where(a => a.UserId == user.Id).FirstOrDefault();
            var group = dbT.Groups.Find(student.GroupId);
            var students = dbT.Students.Where(a => a.GroupId == group.Id).ToList();
            foreach (var stud in students)
            {
                var userStud = dbT.Users.Find(stud.UserId);
                stud.User = userStud;
            }
            group.Students = students.OrderBy(a => a.User.Surname).ToList();
            return View(group);
        }

        public async Task<ActionResult> ViewTeachers()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var student = dbT.Students.Where(a => a.UserId == user.Id).FirstOrDefault();
            var group = dbT.Groups.Find(student.GroupId);
            var studyGroups = dbT.StudyGroups.Where(a => a.GroupId == group.Id).ToList();
            var teachers = new List<Teacher>();
            foreach (var sg in studyGroups)
            {
                teachers.Add(dbT.Teachers.Find(sg.TeacherId));
            }
            foreach (var teacher in teachers)
            {
                teacher.User = dbT.Users.Find(teacher.UserId);
                teacher.Cathedra = dbT.Cathedras.Find(teacher.CathedraId);
            }
            return View(teachers);
        }

        public async Task<ActionResult> ViewDisciplines()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var student = dbT.Students.Where(a => a.UserId == user.Id).FirstOrDefault();
            var group = dbT.Groups.Find(student.GroupId);
            var disciplines = dbT.Disciplines.Where(a => a.GroupId == group.Id).Where(a => a.IsPassed == false).ToList();
            foreach (var discipline in disciplines)
            {
                discipline.Group = group;
                var teacher = dbT.Teachers.Find(discipline.TeacherId);
                teacher.User = dbT.Users.Find(teacher.UserId);
                discipline.Teacher = teacher;
            }
            return View(disciplines);
        }
    }
}