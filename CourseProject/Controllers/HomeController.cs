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
    [RequireHttps]
    public class HomeController : Controller
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
        public async Task<ActionResult> Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if(user.IsConfirmed)
                {
                    if (this.User.IsInRole("Admin"))
                    {
                        ViewBag.Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
                        return View();
                    }
                    else if (this.User.IsInRole("Teacher"))
                    {
                        ViewBag.Layout = "~/Views/Shared/_LayoutTeacher.cshtml";
                        return View();
                    }
                    else if (this.User.IsInRole("Student"))
                    {
                        ViewBag.Layout = "~/Views/Shared/_LayoutStudent.cshtml";
                        return View();
                    }
                    else
                    {
                        ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
                    return View();
                }
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
                return View();
            }
        }
        [Authorize(Roles = "Admin, Teacher")]
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
    }
}