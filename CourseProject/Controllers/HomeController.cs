using CourseProject.Models;
using CourseProject.Models.General;
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
                return View(cathedra);

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
                return View(teacher);

            }
            return HttpNotFound();
        }
    }
}