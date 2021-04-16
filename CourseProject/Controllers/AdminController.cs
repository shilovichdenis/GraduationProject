using CourseProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.Entity;
using CourseProject.Models.General;
using CourseProject.Models.Teachers;
using CourseProject.Models.Students;
using CourseProject.Models.NeuralNetworks;

namespace CourseProject.Controllers
{
    [Authorize(Roles = "Admin")]
    [RequireHttps]
    public class AdminController : Controller
    {
        ApplicationDbContext dbT = new ApplicationDbContext();
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
        [HttpGet]
        public ActionResult ViewUsers()
        {
            return View(dbT.Users.OrderBy(a => a.Surname).ToList());
        }
        [HttpPost]
        public ActionResult ConfirmUser(string id)
        {
            var user = dbT.Users.Find(id);
            if (user != null)
            {
                user.IsConfirmed = true;
                dbT.Entry(user).State = EntityState.Modified;
                dbT.SaveChanges();
                return RedirectToAction("ConfirmUser");
            }
            else
            {
                return View("Error");
            }
        }



        public ActionResult ViewDisciplines()
        {
            var disciplines = dbT.Disciplines.ToList();
            foreach (var discipline in disciplines)
            {
                var teacher = dbT.Teachers.Find(discipline.TeacherId);
                teacher.User = dbT.Users.Find(teacher.UserId);
                discipline.Teacher = teacher;
                discipline.Group = dbT.Groups.Find(discipline.GroupId);
            }
            return View(disciplines);
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
                    student.User = dbT.Users.Find(student.UserId);
                    statement.Student = student;
                    statement.Discipline = discipline;
                }
                ViewBag.Discipline = discipline;
                return PartialView(statements);
            }
            return HttpNotFound();
        }

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
        public ActionResult CreateDiscipline()
        {
            ViewBag.Groups = new SelectList(dbT.Groups.OrderBy(a => a.Name), "Id", "Name");
            var teachers = dbT.Teachers.ToList();
            foreach (var teacher in teachers)
            {
                teacher.User = dbT.Users.Find(teacher.UserId);
            }
            ViewBag.Teachers = new SelectList(teachers.OrderBy(a => a.User.Surname), "Id", "User.DisplayName");
            return PartialView();
        }
        [HttpPost]
        public ActionResult CreateDiscipline(Discipline discipline)
        {
            if (discipline != null)
            {
                discipline.IsPassed = false;
                var teacher = dbT.Teachers.Find(discipline.TeacherId);
                teacher.User = dbT.Users.Find(teacher.UserId);
                var group = dbT.Groups.Find(discipline.GroupId);
                discipline.Teacher = teacher;
                discipline.Group = group;
                var students = dbT.Students.Where(a => a.GroupId == group.Id).ToList();
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
                var oldTeacher = dbT.Teachers.Find(discipline.TeacherId);
                oldTeacher.User = dbT.Users.Find(oldTeacher.UserId);
                var group = dbT.Groups.Find(discipline.GroupId);
                discipline.Group = group;
                discipline.Teacher = oldTeacher;
                ViewBag.Groups = new SelectList(dbT.Groups.OrderBy(a => a.Name), "Id", "Name");
                var teachers = dbT.Teachers.ToList();
                foreach (var teacher in teachers)
                {
                    teacher.User = dbT.Users.Find(teacher.UserId);
                }
                ViewBag.Teachers = new SelectList(teachers.OrderBy(a => a.User.Surname), "Id", "User.DisplayName");
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
                var teacher = dbT.Teachers.Find(discipline.TeacherId);
                var group = dbT.Groups.Find(discipline.GroupId);
                discipline.Group = group;
                discipline.Teacher = teacher;
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



        public ActionResult ViewRoles()
        {
            var roles = RoleManager.Roles;
            return View(roles);
        }
        [HttpGet]
        public ActionResult CreateRole()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<ActionResult> CreateRole([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result
                    = await RoleManager.CreateAsync(new ApplicationRole(name));

                if (result.Succeeded)
                {
                    return RedirectToAction("ViewRoles");
                }
                else
                {
                    return View("Error");
                }
            }
            return View(name);
        }
        public async Task<ActionResult> EditRole(string id)
        {
            var roles = dbT.Roles.ToList();

            ApplicationRole selectedRole = await RoleManager.FindByIdAsync(id);
            roles.Remove(selectedRole);
            string[] memberIDs = selectedRole.Users.Select(x => x.UserId).ToArray();

            IEnumerable<ApplicationUser> members
                = UserManager.Users.Where(a => a.IsConfirmed == true).Where(x => memberIDs.Any(y => y == x.Id)).OrderBy(a => a.UserName);

            List<ApplicationUser> membersInOtherRole = new List<ApplicationUser>();

            List<ApplicationUser> nonMembers = UserManager.Users.Where(a => a.IsConfirmed == true).Except(members).OrderBy(a => a.UserName).ToList();

            foreach (var role in roles)
            {
                foreach (var member in nonMembers)
                {
                    if (UserManager.IsInRole(member.Id, role.Name))
                    {
                        membersInOtherRole.Add(member);
                    }
                }
            }
            nonMembers = nonMembers.Except(membersInOtherRole).ToList();
            return PartialView(new RoleEditModel
            {
                Role = selectedRole,
                Members = members,
                NonMembers = nonMembers
            });
        }
        [HttpPost]
        public async Task<ActionResult> EditRole(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    result = await UserManager.AddToRoleAsync(userId, model.RoleName);

                    if (result.Succeeded)
                    {
                        var user = dbT.Users.Find(userId);
                        if (model.RoleName.Equals("Student"))
                        {
                            var student = new Student(userId, user.SpecializationId, user.OtherInfo);
                            dbT.Students.Add(student);
                        }
                        else if (model.RoleName.Equals("Teacher"))
                        {
                            var teacher = new Teacher(userId, user.SpecializationId);
                            dbT.Teachers.Add(teacher);
                        }
                        dbT.SaveChanges();
                    }
                    else
                    {
                        return View("Error");
                    }
                }
                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    result = await UserManager.RemoveFromRoleAsync(userId,
                    model.RoleName);

                    if (result.Succeeded)
                    {
                        if (model.RoleName.Equals("Student"))
                        {
                            var student = dbT.Students.Where(a => a.UserId == userId).FirstOrDefault();
                            dbT.Students.Remove(student);
                        }
                        else if (model.RoleName.Equals("Teacher"))
                        {
                            var teacher = dbT.Teachers.Where(a => a.UserId == userId).FirstOrDefault();
                            dbT.Teachers.Remove(teacher);
                        }
                        dbT.SaveChanges();
                    }
                    else
                    {
                        return View("Error");
                    }
                }
                return RedirectToAction("ViewRoles");
            }
            return View("Error");
        }
        public async Task<ActionResult> DeleteRole(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await RoleManager.DeleteAsync(role);
            }
            return RedirectToAction("IndexRole");
        }


        public ActionResult ViewInformation()
        {
            var information = dbT.Information.ToList();
            foreach (var info in information)
            {
                if (info.SenderId != 0)
                {
                    var teacher = dbT.Teachers.Find(info.SenderId);
                    teacher.User = dbT.Users.Find(teacher.UserId);
                    info.Sender = teacher;
                }
                info.Reciever = dbT.Groups.Find(info.RecieverId);
            }
            return View(information);
        }

        [HttpGet]
        public ActionResult CreateInformation()
        {
            ViewBag.Groups = new SelectList(dbT.Groups.OrderBy(a => a.Name), "Id", "Name");
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
                if (information.SenderId != 0)
                {
                    var teacher = dbT.Teachers.Find(information.SenderId);
                    teacher.User = dbT.Users.Find(teacher.UserId);
                    information.Sender = teacher;
                }
                information.Reciever = dbT.Groups.Find(information.RecieverId);
                ViewBag.Groups = new SelectList(dbT.Groups.OrderBy(a => a.Name), "Id", "Name");
                return PartialView(information);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        public ActionResult EditInformation(Information information)
        {
            information.DateTime = DateTime.Today;
            dbT.Entry(information).State = EntityState.Modified;
            dbT.SaveChanges();
            dbT.Dispose();
            return RedirectToAction("ViewInformation");
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

        public ActionResult ViewStudents()
        {
            var students = dbT.Students.ToList();
            foreach (var student in students)
            {
                student.User = dbT.Users.Find(student.UserId);
                student.Group = dbT.Groups.Find(student.GroupId);
            }
            return View(students.OrderBy(a => a.User.Surname));
        }

        public ActionResult ViewTeachers()
        {
            var teachers = dbT.Teachers.ToList();
            foreach (var teacher in teachers)
            {
                teacher.User = dbT.Users.Find(teacher.UserId);
                if(teacher.CathedraId != null)
                {
                    teacher.Cathedra = dbT.Cathedras.Find(teacher.CathedraId);
                }
                else
                {
                    teacher.Cathedra = new Cathedra();
                }
            }
            return View(teachers.OrderBy(a => a.User.Surname));
        }

        public async Task<ActionResult> InfoAboutUser(string id)
        {
            if (id != null)
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user.Role.Equals("Студент"))
                {
                    //info.User = user;
                    //var tests = dbT.Disciplines.Where(a => a.GroupId == user.SpecializationId).Where(b => b.Type.Contains("Зачет")).ToList();
                    //var testsWithRating = new List<Tests>();
                    //foreach (var test in tests)
                    //{
                    //    var teacher = dbT.Users.Find(test.TeacherId);
                    //    test.Teacher = teacher;
                    //    var statement = dbT.Statements.Where(a => a.DisciplineId == test.Id).Where(b => b.StudentId == user.Id).FirstOrDefault();
                    //    string result;
                    //    if (statement.Rating == 1)
                    //    {
                    //        result = "Зачет";
                    //    }
                    //    else if (statement.Rating == 0)
                    //    {
                    //        result = "Не зачет";
                    //    }
                    //    else if (test.Status.Equals("Сдана"))
                    //    {
                    //        result = "Не явился";
                    //    }
                    //    else
                    //    {
                    //        result = "";
                    //    }
                    //    testsWithRating.Add(new Tests(test, result));
                    //}
                    //var exams = dbT.Disciplines.Where(a => a.GroupId == user.SpecializationId).Where(b => b.Type.Contains("Экзамен")).ToList();
                    //var examsWithRating = new List<Exams> { };
                    //foreach (var exam in exams)
                    //{
                    //    var teacher = dbT.Users.Find(exam.TeacherId);
                    //    exam.Teacher = teacher;
                    //    var statement = dbT.Statements.Where(a => a.DisciplineId == exam.Id).Where(b => b.StudentId == user.Id).FirstOrDefault();
                    //    examsWithRating.Add(new Exams(exam, statement.Rating));
                    //}
                    //var recordBook = new RecordBook(user, examsWithRating, testsWithRating);
                    //info.RecordBook = recordBook;
                    //ViewBag.Specialization = dbT.Groups.Find(user.SpecializationId) as Group;
                    return View();
                }
                else
                {
                    //info.User = user;
                    //ViewBag.Specialization = dbT.Cathedras.Find(user.SpecializationId) as Cathedra;
                    return View();
                }
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ViewUsers");
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
        public ActionResult ViewNeuralGroups()
        {
            ViewBag.Results = dbT.Results.ToList() as List<NeuralNetworkResult>;
            ViewBag.Groups = new SelectList(dbT.Groups.OrderBy(a => a.Name), "Id", "Name");
            return View();
        }

        //public ActionResult ViewNeuralGroups(int groupId, int? k)
        //{
        //    ViewBag.Groups = new SelectList(dbT.Groups.OrderBy(a => a.Name), "Id", "Name");
        //    if (k == null)
        //    {
        //        k = 1;
        //    }

        //    List<StudentsForNeuralNetwork> studentsForNeuralNetwork = new List<StudentsForNeuralNetwork>();
        //    var students = dbT.Users.Where(a => a.Role.Equals("Студент")).Where(b => b.SpecializationId == groupId).OrderBy(c => c.Surname).ToList();
        //    var group = dbT.Groups.Find(groupId);
        //    if (students.Count != 0)
        //    {
        //        var countexams = new int();
        //        foreach (var (index, student) in students.Select((i, s) => (s, i)))
        //        {
        //            StudentsForNeuralNetwork studentForNeuralNetwork = new StudentsForNeuralNetwork();
        //            var tests = dbT.Disciplines.Where(a => a.GroupId == student.SpecializationId).Where(b => b.Type.Contains("Зачет")).ToList();
        //            var testsWithRating = new List<Tests>();
        //            foreach (var test in tests)
        //            {
        //                var statement = dbT.Statements.Where(a => a.DisciplineId == test.Id).Where(b => b.StudentId == student.Id).FirstOrDefault();
        //                testsWithRating.Add(new Tests(test, statement.Rating.ToString()));
        //            }
        //            var exams = dbT.Disciplines.Where(a => a.GroupId == student.SpecializationId).Where(b => b.Type.Contains("Экзамен")).ToList();
        //            var examsWithRating = new List<Exams> { };
        //            foreach (var exam in exams)
        //            {
        //                var statement = dbT.Statements.Where(a => a.DisciplineId == exam.Id).Where(b => b.StudentId == student.Id).FirstOrDefault();
        //                examsWithRating.Add(new Exams(exam, statement.Rating));
        //            }
        //            var recordBook = new RecordBook(student, examsWithRating, testsWithRating);
        //            studentsForNeuralNetwork.Add(new StudentsForNeuralNetwork(index + 1, student, recordBook));
        //        }
        //        countexams = studentsForNeuralNetwork.ElementAt(0).RecordBook.Exams.Count;
        //        ViewBag.Disciplines = studentsForNeuralNetwork.ElementAt(0).RecordBook.Exams as List<Exams>;
        //        NeuralNetwork network = new NeuralNetwork(countexams + 3, (int)k, studentsForNeuralNetwork, group);
        //        NeuralNetworkResult result = new NeuralNetworkResult(network.F1, network.F2, network.K, network.M, groupId);
        //        var results = dbT.Results.Where(a => a.GroupId == group.Id).ToList();
        //        foreach (var res in results)
        //        {
        //            res.Group = group;
        //        }
        //        ViewBag.Results = results as List<NeuralNetworkResult>;
        //        dbT.Results.Add(result);
        //        dbT.SaveChanges();
        //        return View(network);
        //    }
        //    else
        //    {
        //        return View();
        //    }

        //}

        public ActionResult ViewCathedras()
        {
            var cathedras = dbT.Cathedras.OrderBy(a => a.Name).ToList();
            foreach (var cathedra in cathedras)
            {
                var teachers = dbT.Teachers.Where(a => a.CathedraId == cathedra.Id).ToList();
                cathedra.Teachers = teachers;
            }
            return View(cathedras);
        }
        [HttpGet]
        public ActionResult CreateCathedra()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult CreateCathedra(Cathedra cathedra)
        {
            if (cathedra != null)
            {
                dbT.Cathedras.Add(cathedra);
                dbT.SaveChanges();
                dbT.Dispose();
                return RedirectToAction("ViewCathedras");
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpGet]
        public ActionResult EditCathedra(int id)
        {
            var cathedra = dbT.Cathedras.Find(id);
            if (cathedra != null)
            {
                return PartialView(cathedra);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        public ActionResult EditCathedra(Cathedra cathedra)
        {
            if (cathedra != null)
            {
                dbT.Entry(cathedra).State = EntityState.Modified;
                dbT.SaveChanges();
                dbT.Dispose();
                return RedirectToAction("ViewCathedras");
            }
            else
            {
                return HttpNotFound();
            }
        }
        public ActionResult DeleteCathedra(int id)
        {
            var cathedra = dbT.Cathedras.Find(id);
            var teachers = dbT.Teachers.Where(a => a.CathedraId == cathedra.Id).ToList();
            foreach (var teacher in teachers)
            {
                teacher.CathedraId = null;
            }
            if (cathedra != null)
            {
                dbT.Cathedras.Remove(cathedra);
                dbT.SaveChanges();
                dbT.Dispose();
                return RedirectToAction("ViewCathedras");
            }
            else
            {
                return HttpNotFound();
            }

        }

        public ActionResult ViewGroups()
        {
            var groups = dbT.Groups.ToList();
            foreach (var group in groups)
            {
                group.Students = dbT.Students.Where(a => a.GroupId == group.Id).ToList();
            }
            return View(groups.OrderBy(a => a.Name));
        }

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
        [HttpGet]
        public ActionResult CreateGroup()
        {
            Group group = new Group();
            var faculties = group.Faculties.OrderBy(a => a.Name);
            var specialties = group.Specialties.OrderBy(a => a.Name);
            int selectedIndex = 7;
            ViewBag.Faculties = new SelectList(faculties, "Id", "Name", selectedIndex);
            ViewBag.Specialties = new SelectList(specialties.Where(c => c.FacultyId == selectedIndex), "Id", "Name");
            return PartialView();
        }

        public ActionResult GetSpecialties(int id)
        {
            Group group = new Group();
            var specialties = group.Specialties.OrderBy(a => a.Name);
            return PartialView(specialties.Where(c => c.FacultyId == id).ToList());
        }

        [HttpPost]
        public ActionResult CreateGroup(Group group)
        {
            if (group != null)
            {
                if (ModelState.IsValid)
                {
                    dbT.Groups.Add(group);
                    dbT.SaveChanges();
                    return RedirectToAction("ViewGroups");
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
        public ActionResult DeleteGroup(int id)
        {
            var group = dbT.Groups.Find(id);
            if (group != null)
            {
                var students = dbT.Students.Where(b => b.GroupId == group.Id).ToList();
                foreach (var student in students)
                {
                    student.GroupId = 0;
                    dbT.Entry(student).State = EntityState.Modified;
                }
                dbT.Groups.Remove(group);
                dbT.SaveChanges();
                return RedirectToAction("ViewGroups");
            }
            else
            {
                return View("Error");
            }
        }
        /*public ActionResult DeleteFromGroup(string id)
        {
            var student = dbT.Users.Find(id);
            if (student != null)
            {
                student.SpecializationId = null;
                dbT.Entry(student).State = EntityState.Modified;
                dbT.SaveChanges();
                return RedirectToAction("ViewGroups");
            }
            else
            {
                return View("Error");
            }
        }*/
    }
}