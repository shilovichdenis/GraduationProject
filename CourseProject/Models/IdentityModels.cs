using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using CourseProject.Models.Students;
using CourseProject.Models.General;
using CourseProject.Models.Teachers;
using CourseProject.Models.NeuralNetworks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CourseProject.Models
{
    // В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Display(Name = "Пол")]
        public string Gender { get; set; }

        [Display(Name = "Роль")]
        public string Role { get; set; }
        public int SpecializationId { get; set; }

        [Display(Name = "Др. инф-ция")]
        public string OtherInfo { get; set; }
        public bool IsConfirmed { get; set; }

        [Display(Name = "Почта")]
        public override string Email { get; set; }
        [Display(Name = "Телефон")]
        public override string PhoneNumber { get; set; }

        public string DisplayName
        {
            get
            {
                return this.Surname + " " + this.Name + " " + this.Patronymic;
            }
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Cathedra> Cathedras { get; set; }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Statement> Statements { get; set; }
        public DbSet<StudyGroups> StudyGroups { get; set; }


        public DbSet<NeuralNetworkResult> Results { get; set; }
        public DbSet<Information> Information { get; set; }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }

        public DbSet<Publications> Publications { get; set; }
        public DbSet<ScientificWork> ScientificWorks { get; set; }
    }
}