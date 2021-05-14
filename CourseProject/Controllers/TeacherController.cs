using CourseProject.Models;
using CourseProject.Models.Teachers;
using CourseProject.Models.General;
using CourseProject.Models.Students;
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
    [RequireHttps]
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
            if (teacher != null)
            {
                teacher.User = user;
                teacher.Cathedra = dbT.Cathedras.Where(a => a.Id == teacher.CathedraId).FirstOrDefault();
                teacher.ScientificWorks = dbT.ScientificWorks.Where(a => a.TeacherId == teacher.Id).ToList();
                teacher.Publications = dbT.Publications.Where(a => a.TeacherId == teacher.Id).ToList();
                return View(teacher);
            }
            return HttpNotFound();
        }


        public async Task<ActionResult> ViewGroups()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var teacher = dbT.Teachers.Where(a => a.UserId == user.Id).FirstOrDefault();
            if (teacher != null)
            {
                var studyGroups = dbT.StudyGroups.Where(a => a.TeacherId == teacher.Id).ToList();
                foreach (var sg in studyGroups)
                {
                    sg.Group = dbT.Groups.Find(sg.GroupId);
                }
                return View(studyGroups.OrderBy(a => a.Group.Name));
            }
            return HttpNotFound();
        }

        [HttpGet]
        public async Task<ActionResult> AddGroup()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var teacher = dbT.Teachers.Where(a => a.UserId == user.Id).FirstOrDefault();
            if (teacher != null)
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
                dbT.Dispose();
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
                    dbT.Dispose();
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
                return View(disciplines.OrderBy(a => a.Name));
            }
            return HttpNotFound();
        }

        //[HttpGet]
        //public ActionResult InfoAboutDiscipline(int id)
        //{
        //    var discipline = dbT.Disciplines.Find(id);
        //    discipline.Group = dbT.Groups.Find(discipline.GroupId);
        //    discipline.Teacher = dbT.Teachers.Find(discipline.TeacherId);
        //    discipline.Teacher.User = dbT.Users.Find(discipline.Teacher.UserId);
        //    return PartialView(discipline);
        //}

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
                discipline.SetTerm();
                var students = dbT.Students.Where(b => b.GroupId == group.Id).ToList();
                foreach (var student in students)
                {
                    dbT.Statements.Add(new Statement(student.Id, discipline.Id));
                }
                dbT.Disciplines.Add(discipline);
                dbT.SaveChanges();
                dbT.Dispose();
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
                dbT.Dispose();
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
                dbT.Dispose();
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
                    var student = dbT.Students.Find(statement.StudentId);
                    var user = dbT.Users.Find(student.UserId);
                    student.User = user;
                    statement.Student = student;
                    statement.Discipline = discipline;
                }
                ViewBag.Discipline = discipline;
                return PartialView(statements.OrderBy(a => a.Student.User.Surname));
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
                //пересчет (функция вызов)
                dbT.SaveChanges();
                dbT.Dispose();
                return RedirectToAction("ViewDisciplines");
            }
            else
            {
                return View("Error");
            }
        }

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
            return View(information.OrderBy(a => a.DateTime));
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
            if (information != null)
            {
                information.DateTime = DateTime.Today;
                dbT.Information.Add(information);
                dbT.SaveChanges();
                dbT.Dispose();
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

        [HttpGet]
        public ActionResult AddScientificWork()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var teacher = dbT.Teachers.Where(a => a.UserId == user.Id).FirstOrDefault();
            teacher.User = user;
            ViewBag.Teacher = teacher;
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddScientificWork(ScientificWork scientificWork)
        {
            if (scientificWork != null)
            {
                dbT.ScientificWorks.Add(scientificWork);
                dbT.SaveChanges();
                dbT.Dispose();
                return RedirectToAction("InfoAboutYourself");
            }
            return HttpNotFound();
        }
        [HttpGet]
        public ActionResult EditScientificWork(int id)
        {
            var work = dbT.ScientificWorks.Find(id);
            work.Teacher = dbT.Teachers.Find(work.TeacherId);
            work.Teacher.User = dbT.Users.Find(work.Teacher.UserId);
            return PartialView(work);
        }
        [HttpPost]
        public ActionResult EditScientificWork(ScientificWork work)
        {
            if(work != null)
            {
                dbT.Entry(work).State = EntityState.Modified;
                dbT.SaveChanges();
                dbT.Dispose();
            }
            return HttpNotFound();
        }
        public ActionResult DeleteScientificWork(int id)
        {
            var work = dbT.ScientificWorks.Find(id);
            if(work != null)
            {
                dbT.ScientificWorks.Remove(work);
                dbT.SaveChanges();
                dbT.Dispose();
                return RedirectToAction("InfoAboutYourself");
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult AddPublication()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var teacher = dbT.Teachers.Where(a => a.UserId == user.Id).FirstOrDefault();
            teacher.User = user;
            ViewBag.Teacher = teacher;
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddPublication(Publication publication)
        {
            if (publication != null)
            {
                dbT.Publications.Add(publication);
                dbT.SaveChanges();
                dbT.Dispose();
                return RedirectToAction("InfoAboutYourself");
            }
            return HttpNotFound();
        }
        [HttpGet]
        public ActionResult EditPublication(string id)
        {
            var publication = dbT.Publications.Find(id);
            publication.Teacher = dbT.Teachers.Find(publication.TeacherId);
            publication.Teacher.User = dbT.Users.Find(publication.Teacher.UserId);
            return PartialView(publication);
        }
        [HttpPost]
        public ActionResult EditPublication(Publication publication)
        {
            if (publication != null)
            {
                dbT.Entry(publication).State = EntityState.Modified;
                dbT.SaveChanges();
                dbT.Dispose();
            }
            return HttpNotFound();
        }
        public ActionResult DeletePublication(string id)
        {
            var publication = dbT.Publications.Find(id);
            if (publication != null)
            {
                dbT.Publications.Remove(publication);
                dbT.SaveChanges();
                dbT.Dispose();
                return RedirectToAction("InfoAboutYourself");
            }
            return HttpNotFound();
        }
    }
}