using CourseProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CourseProject.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        ApplicationDbContext dbT = new ApplicationDbContext();
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        public async Task<ActionResult> InfoAboutYourself()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var teacher = dbT.Teachers.Where(a => a.UserId == user.Id).FirstOrDefault();
            if(teacher != null)
            {
                teacher.User = user;
                teacher.Cathedra = dbT.Cathedras.Find(teacher.CathedraId);
                return View(teacher);
            }
            return HttpNotFound();
        }


        public async Task<ActionResult> ViewGroups()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var teacher = dbT.Teachers.Where(a => a.UserId == user.Id).FirstOrDefault();
            if(teacher != null)
            {
                var studyGroups = dbT.StudyGroups.Where(a => a.TeacherId == teacher.Id).ToList();
                foreach (var sg in studyGroups)
                {
                    sg.Group = dbT.Groups.Find(sg.GroupId);
                }
                return View(studyGroups);
            }
            return HttpNotFound();
        }

        [HttpGet]
        public async Task<ActionResult> AddGroup()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var teacher = dbT.Teachers.Where(a => a.UserId == user.Id).FirstOrDefault();
            if(teacher != null)
            {
                ViewBag.Groups = new SelectList(dbT.Groups.OrderBy(a => a.Name), "Id", "Name");
                ViewBag.TeacherId = teacher.Id;
                return PartialView();
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult AddGroup(StudyGroups studyGroups)
        {
            if (studyGroups != null)
            {
                var group = dbT.Groups.Find(studyGroups.GroupId);
                var teacher = dbT.Teachers.Find(studyGroups.TeacherId);
                studyGroups.Group = group;
                studyGroups.Teacher = teacher;
                dbT.StudyGroups.Add(studyGroups);
                dbT.SaveChanges();
                return RedirectToAction("ViewGroups");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> RemoveGroup(int id)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var teacher = dbT.Teachers.Where(a => a.UserId == user.Id).FirstOrDefault();
            if (teacher != null)
            {
                var group = dbT.Groups.Find(id);
                if (group != null)
                {
                    var studyGroup = dbT.StudyGroups.Where(a => a.TeacherId == teacher.Id).Where(a => a.GroupId == group.Id).FirstOrDefault();
                    dbT.StudyGroups.Remove(studyGroup);
                    dbT.SaveChanges();
                    return RedirectToAction("ViewGroups");
                }
                return HttpNotFound();
            }
            return HttpNotFound();

        }
        [HttpGet]
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


        public ActionResult ViewDisciplines()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var teacher = dbT.Teachers.Where(a => a.UserId == user.Id).FirstOrDefault();
            if (teacher != null)
            {
                var disciplines = dbT.Disciplines.Where(a => a.TeacherId == teacher.Id).ToList();
                foreach (var discipline in disciplines)
                {
                    discipline.Group = dbT.Groups.Find(discipline.GroupId);
                }
                return View(disciplines);
            }
            return HttpNotFound();

        }

        [HttpGet]
        public ActionResult InfoAboutDiscipline(int id)
        {
            var discipline = dbT.Disciplines.Find(id);
            discipline.Group = dbT.Groups.Find(discipline.GroupId);
            discipline.Teacher = dbT.Teachers.Find(discipline.TeacherId);
            discipline.Teacher.User = dbT.Users.Find(discipline.Teacher.UserId);
            return PartialView(discipline);
        }

        [HttpGet]
        public ActionResult CreateDiscipline()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var teacher = dbT.Teachers.Where(a => a.UserId == user.Id).FirstOrDefault();
            if (teacher != null)
            {
                teacher.User = user;
                var studyGroups = dbT.StudyGroups.Where(a => a.TeacherId == teacher.Id).ToList();
                var groups = new List<Group>();
                foreach (var sg in studyGroups)
                {
                    var group = dbT.Groups.Where(a => a.Id == sg.GroupId).FirstOrDefault();
                    groups.Add(group);
                }
                ViewBag.Groups = new SelectList(groups.OrderBy(a => a.Name), "Id", "Name");
                ViewBag.Teacher = teacher;
                return PartialView();
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult CreateDiscipline(Discipline discipline)
        {
            if (discipline != null)
            {
                discipline.IsPassed = false;
                var group = dbT.Groups.Find(discipline.GroupId);
                discipline.Group = group;
                var students = dbT.Students.Where(b => b.GroupId == group.Id).ToList();
                foreach (var student in students)
                {
                    dbT.Statements.Add(new Statement(student.Id, discipline.Id));
                }
                dbT.Disciplines.Add(discipline);
                dbT.SaveChanges();
                return RedirectToAction("ViewDisciplines");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult EditDiscipline(int id)
        {
            var discipline = dbT.Disciplines.Find(id);
            if (discipline != null)
            {
                var teacher = dbT.Teachers.Find(discipline.TeacherId);
                var user = dbT.Users.Find(teacher.UserId);
                teacher.User = user;
                discipline.Teacher = teacher;
                var studyGroups = dbT.StudyGroups.Where(a => a.TeacherId == teacher.Id).ToList();
                var groups = new List<Group>();
                foreach (var sg in studyGroups)
                {
                    var group = dbT.Groups.Where(a => a.Id == sg.GroupId).FirstOrDefault();
                    groups.Add(group);
                }
                ViewBag.Groups = new SelectList(groups.OrderBy(a => a.Name), "Id", "Name");
                ViewBag.Teacher = teacher;
                return PartialView(discipline);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult EditDiscipline(Discipline discipline)
        {
            if (discipline != null)
            {
                var group = dbT.Groups.Find(discipline.GroupId);
                discipline.Group = group;
                dbT.Entry(discipline).State = EntityState.Modified;
                dbT.SaveChanges();
                return RedirectToAction("ViewDisciplines");
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult DeleteDiscipline(int id)
        {
            var discipline = dbT.Disciplines.Find(id);
            if (discipline != null)
            {
                var statements = dbT.Statements.Where(a => a.DisciplineId == discipline.Id);
                foreach (var statement in statements)
                {
                    dbT.Statements.Remove(statement);
                }
                dbT.Disciplines.Remove(discipline);
                dbT.SaveChanges();
                return RedirectToAction("ViewDisciplines");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult SetRatings(int? id)
        {
            if (id != null)
            {
                var discipline = dbT.Disciplines.Find(id);
                var statements = dbT.Statements.Where(a => a.DisciplineId == id).ToList();
                foreach (var statement in statements)
                {
                    var student = dbT.Students.Where(a => a.Id == statement.StudentId).FirstOrDefault();
                    var user = dbT.Users.Find(student.UserId);
                    student.User = user;
                    statement.Student = student;
                    statement.Discipline = discipline;
                }
                ViewBag.Discipline = discipline;
                return PartialView(statements);
            }
            else
            {
                return View("Error");
            }
        }
        [HttpPost]
        public ActionResult SetRatings(string[] rating, int[] id)
        {
            var stat = dbT.Statements.Find(id.ElementAt(0));
            var discipline = dbT.Disciplines.Find(stat.DisciplineId);
            if (rating.Length != 0 && id.Length != 0)
            {
                if (discipline.IsExam)
                {
                    foreach (var (index, item) in id.Select((v, i) => (i, v)))
                    {
                        var statement = dbT.Statements.Find(item);
                        statement.Rating = Int32.Parse(rating.ElementAt(index));
                        dbT.Entry(statement).State = EntityState.Modified;
                    }
                }
                else
                {
                    foreach (var (index, item) in id.Select((v, i) => (i, v)))
                    {
                        var statement = dbT.Statements.Find(item);
                        if (rating.ElementAt(index).Equals("зч"))
                        {
                            statement.Rating = 1;
                        }
                        else if (rating.ElementAt(index).Equals("нзч"))
                        {
                            statement.Rating = 0;
                        }
                        else
                        {
                            statement.Rating = -1;
                        }
                        dbT.Entry(statement).State = EntityState.Modified;
                    }
                }
                dbT.SaveChanges();
                return RedirectToAction("ViewDisciplines");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult ViewInformation()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var teacher = dbT.Teachers.Where(a => a.UserId == user.Id).FirstOrDefault();
            var studyGroups = dbT.StudyGroups.Where(a => a.TeacherId == teacher.Id).ToList();
            var groups = new List<Group>();
            foreach (var sg in studyGroups)
            {
                var group = dbT.Groups.Where(a => a.Id == sg.GroupId).FirstOrDefault();
                groups.Add(group);
            }
            ViewBag.Groups = new SelectList(groups.OrderBy(a => a.Name), "Id", "Name");
            var information = dbT.Information.Where(a => a.SenderId == teacher.Id).ToList();
            foreach (var info in information)
            {
                info.Reciever = dbT.Groups.Find(info.RecieverId);
            }
            ViewBag.Teacher = teacher;
            return View(information);
        }
        [HttpPost]
        public ActionResult ViewInformation(string info, int receiverId, int senderId)
        {
            if (!string.IsNullOrEmpty(info))
            {
                var group = dbT.Groups.Find(receiverId);
                if (group != null)
                {
                    var information = new Information(info, receiverId, senderId, DateTime.Today);
                    dbT.Information.Add(information);
                    dbT.SaveChanges();
                    dbT.Dispose();
                    return RedirectToAction("ViewInformation");
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<ActionResult> CreateInformation()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var teacher = dbT.Teachers.Where(a => a.UserId == user.Id).FirstOrDefault();
            teacher.User = user;
            var studyGroups = dbT.StudyGroups.Where(a => a.TeacherId == teacher.Id).ToList();
            var groups = new List<Group>();
            foreach (var sg in studyGroups)
            {
                var group = dbT.Groups.Where(a => a.Id == sg.GroupId).FirstOrDefault();
                groups.Add(group);
            }
            ViewBag.Groups = new SelectList(groups.OrderBy(a => a.Name), "Id", "Name");
            ViewBag.Teacher = teacher;
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreateInformation(Information information)
        {
            if(information != null)
            {
                information.DateTime = DateTime.Today;
                dbT.Information.Add(information);
                dbT.SaveChanges();
                return RedirectToAction("ViewInformation");
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult EditInformation(int id)
        {
            var information = dbT.Information.Find(id);
            if (information != null)
            {
                var teacher = dbT.Teachers.Find(information.SenderId);
                var user = UserManager.FindById(User.Identity.GetUserId());
                teacher.User = user;
                information.Sender = teacher;
                var studyGroups = dbT.StudyGroups.Where(a => a.TeacherId == teacher.Id).ToList();
                var groups = new List<Group>();
                foreach (var sg in studyGroups)
                {
                    var group = dbT.Groups.Where(a => a.Id == sg.GroupId).FirstOrDefault();
                    groups.Add(group);
                }
                ViewBag.Groups = new SelectList(groups.OrderBy(a => a.Name), "Id", "Name");
                ViewBag.Teacher = teacher;
                return PartialView(information);
            }
            else
            {
                return View("Error");
            }
        }
        [HttpPost]
        public ActionResult EditInformation(Information information)
        {
            if (information != null)
            {
                information.DateTime = DateTime.Today;
                dbT.Entry(information).State = EntityState.Modified;
                dbT.SaveChanges();
                dbT.Dispose();
                return RedirectToAction("ViewInformation");
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult DeleteInformation(int id)
        {
            var information = dbT.Information.Find(id);
            if (information != null)
            {
                dbT.Information.Remove(information);
                dbT.SaveChanges();
                dbT.Dispose();
                return RedirectToAction("ViewInformation");
            }
            return View();
        }

    }
}