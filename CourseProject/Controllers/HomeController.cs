using CourseProject.Models;
using CourseProject.Models.General;
using CourseProject.Models.Students;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CourseProject.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        ApplicationDbContext dbT = new ApplicationDbContext();
        public string SetLayout(IPrincipal iUser)
        {
            if (iUser.Identity.IsAuthenticated)
            {
                var user = UserManager.FindById(iUser.Identity.GetUserId());
                if (user.IsConfirmed)
                {
                    if (iUser.IsInRole("Admin"))
                    {
                        return "~/Views/Shared/_LayoutAdmin.cshtml";
                    }
                    else if (iUser.IsInRole("Teacher"))
                    {
                        return "~/Views/Shared/_LayoutTeacher.cshtml";
                    }
                    else if (iUser.IsInRole("Student"))
                    {
                        return "~/Views/Shared/_LayoutStudent.cshtml";
                    }
                    else
                    {
                        return "~/Views/Shared/_Layout.cshtml";
                    }
                }
                else
                {
                    return "~/Views/Shared/_Layout.cshtml";
                }
            }
            else
            {
                return "~/Views/Shared/_Layout.cshtml";
            }
        }
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        private ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }
        public async Task<ActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user.IsConfirmed)
                {
                    if (this.User.IsInRole("Admin"))
                    {
                        ViewBag.Information = new List<Information>();
                        ViewBag.Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
                        return View();
                    }
                    else if (this.User.IsInRole("Teacher"))
                    {
                        ViewBag.Information = new List<Information>();
                        ViewBag.Layout = "~/Views/Shared/_LayoutTeacher.cshtml";
                        return View();
                    }
                    else if (this.User.IsInRole("Student"))
                    {
                        ViewBag.Layout = "~/Views/Shared/_LayoutStudent.cshtml";
                        var student = dbT.Students.Where(a => a.UserId == user.Id).FirstOrDefault();
                        var group = dbT.Groups.Find(student.GroupId);
                        var information = dbT.Information.Where(a => a.RecieverId == group.Id).ToList();
                        var teachersGroup = dbT.StudyGroups.Where(a => a.GroupId == group.Id).ToList();
                        foreach (var teacher in teachersGroup)
                        {
                            information.AddRange(dbT.Information.Where(a => a.SenderId == teacher.TeacherId).Where(a => a.RecieverId == 0).ToList());
                        }
                        information.AddRange(dbT.Information.Where(a => a.SenderId == 0).ToList());
                        information = information.Distinct().ToList();
                        foreach (var info in information)
                        {
                            info.Reciever = group;
                            var teacher = info.SenderId != 0 ? dbT.Teachers.Find(info.SenderId) : null;
                            if (teacher != null)
                            {
                                teacher.User = dbT.Users.Find(teacher.UserId);
                            }
                            info.Sender = teacher;
                        }
                        ViewBag.Information = information;
                        return View();
                    }
                    else
                    {
                        ViewBag.Information = new List<Information>();
                        ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Information = new List<Information>();
                    ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
                    return View();
                }
            }
            else
            {
                ViewBag.Information = new List<Information>();
                ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
                return View();
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult InfoAboutRatings(int id)
        {
            ViewBag.Layout = SetLayout(User);
            var group = dbT.Groups.Find(id);
            if (group != null)
            {
                ViewBag.Group = group;
                var disciplines = dbT.Disciplines.Where(a => a.GroupId == group.Id).Where(a => a.IsExam).Where(a => a.IsPassed).ToList();
                var displayDisciplineModel = new List<DisplayDisciplineModel>();
                foreach (var discipline in disciplines)
                {
                    var statements = dbT.Statements.Where(a => a.DisciplineId == discipline.Id).ToList();
                    foreach (var statement in statements)
                    {
                        var student = dbT.Students.Find(statement.StudentId);
                        student.User = dbT.Users.Find(student.UserId);
                        student.Group = group;
                        statement.Student = student;
                    }
                    displayDisciplineModel.Add(new DisplayDisciplineModel(discipline, statements));
                }
                var students = dbT.Students.Where(a => a.GroupId == group.Id).ToList();
                foreach (var student in students)
                {
                    student.User = dbT.Users.Find(student.UserId);
                    student.Group = group;
                }
                var result = new DisplayRatings(students, displayDisciplineModel);
                return View(result);
            }
            return HttpNotFound();
        }


        [Authorize]
        public ActionResult InfoAboutCathedra(int id)
        {
            var cathedra = dbT.Cathedras.Find(id);
            if (cathedra != null)
            {
                cathedra.Teachers = dbT.Teachers.Where(a => a.CathedraId == cathedra.Id).OrderBy(a => a.User.Surname).ToList();
                foreach (var teacher in cathedra.Teachers)
                {
                    teacher.User = dbT.Users.Find(teacher.UserId);
                }
                ViewBag.Layout = SetLayout(User);
                return PartialView(cathedra);

            }
            return HttpNotFound();
        }
        [Authorize]
        public ActionResult InfoAboutTeacher(int id)
        {
            var teacher = dbT.Teachers.Find(id);
            if (teacher != null)
            {
                teacher.Cathedra = dbT.Cathedras.Find(teacher.CathedraId);
                teacher.User = dbT.Users.Find(teacher.UserId);
                ViewBag.Layout = SetLayout(User);
                return PartialView(teacher);
            }
            return HttpNotFound();
        }
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult InfoAboutGroup(int id)
        {
            var group = dbT.Groups.Find(id);
            var students = dbT.Students.Where(a => a.GroupId == group.Id).ToList();
            foreach (var student in students)
            {
                student.User = dbT.Users.Find(student.UserId);
            }
            group.Students = students.OrderBy(a => a.User.Surname).ToList();
            return PartialView(group);
        }
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult InfoAboutStudent(int id)
        {
            var student = dbT.Students.Find(id);

            if (student != null)
            {
                student.User = dbT.Users.Find(student.UserId);
                student.Group = dbT.Groups.Find(student.GroupId);
                var dTests = dbT.Disciplines.Where(a => a.GroupId == student.GroupId).Where(a => !a.IsExam).Where(a => a.DateTime < DateTime.Today).Where(a => a.IsPassed == true).OrderByDescending(a => a.DateTime).ToList();
                var tests = new List<Tests> { };
                foreach (var test in dTests)
                {
                    var teacher = dbT.Teachers.Where(a => a.Id == test.TeacherId).FirstOrDefault();
                    teacher.User = dbT.Users.Where(a => a.Id == teacher.UserId).FirstOrDefault();
                    test.Teacher = teacher;

                    var statement = dbT.Statements.Where(a => a.DisciplineId == test.Id).Where(b => b.StudentId == student.Id).FirstOrDefault();
                    tests.Add(new Tests(test, statement.Rating == 1 ? "Зачтено" : "Не зачтено"));
                }


                var dExams = dbT.Disciplines.Where(a => a.GroupId == student.GroupId).Where(b => b.IsExam).Where(a => a.DateTime < DateTime.Today).Where(a => a.IsPassed == true).OrderByDescending(a => a.DateTime).ToList();
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
                ViewBag.Layout = SetLayout(User);
                return PartialView(student);
            }
            return HttpNotFound();
        }
    }
}