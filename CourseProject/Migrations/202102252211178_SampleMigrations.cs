namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cathedras",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DepartmentOffice = c.Int(nullable: false),
                        Building = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        CathedraId = c.Int(nullable: false),
                        AcademicTitle = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cathedras", t => t.CathedraId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CathedraId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Surname = c.String(),
                        Patronymic = c.String(),
                        Gender = c.String(),
                        Role = c.String(),
                        SpecializationId = c.Int(nullable: false),
                        OtherInfo = c.String(),
                        IsConfirmed = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.CourseProjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        TeacherId = c.Int(nullable: false),
                        Name = c.String(),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Teachers", t => t.TeacherId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.TeacherId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        GroupId = c.Int(nullable: false),
                        AverageRating = c.Double(nullable: false),
                        FormOfEducation = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Faculty = c.String(),
                        Specialty = c.String(),
                        FormOfEducation = c.String(),
                        Course = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Disciplines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        Building = c.Int(nullable: false),
                        Audience = c.Int(nullable: false),
                        TeacherId = c.String(),
                        IsExam = c.Boolean(nullable: false),
                        IsPassed = c.Boolean(nullable: false),
                        GroupId = c.Int(nullable: false),
                        СountHours = c.String(),
                        Teacher_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Teachers", t => t.Teacher_Id)
                .Index(t => t.GroupId)
                .Index(t => t.Teacher_Id);
            
            CreateTable(
                "dbo.Information",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Info = c.String(),
                        RecieverId = c.Int(nullable: false),
                        SenderId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NeuralNetworkResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        F1 = c.Double(nullable: false),
                        F2 = c.Double(nullable: false),
                        K = c.Int(nullable: false),
                        M = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Statements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        DisciplineId = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Disciplines", t => t.DisciplineId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.DisciplineId);
            
            CreateTable(
                "dbo.StudyGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeacherId = c.String(),
                        GroupId = c.Int(nullable: false),
                        Teacher_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Teachers", t => t.Teacher_Id)
                .Index(t => t.GroupId)
                .Index(t => t.Teacher_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudyGroups", "Teacher_Id", "dbo.Teachers");
            DropForeignKey("dbo.StudyGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Statements", "DisciplineId", "dbo.Disciplines");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.NeuralNetworkResults", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Disciplines", "Teacher_Id", "dbo.Teachers");
            DropForeignKey("dbo.Disciplines", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.CourseProjects", "TeacherId", "dbo.Teachers");
            DropForeignKey("dbo.CourseProjects", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Students", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Students", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Teachers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Teachers", "CathedraId", "dbo.Cathedras");
            DropIndex("dbo.StudyGroups", new[] { "Teacher_Id" });
            DropIndex("dbo.StudyGroups", new[] { "GroupId" });
            DropIndex("dbo.Statements", new[] { "DisciplineId" });
            DropIndex("dbo.Statements", new[] { "StudentId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.NeuralNetworkResults", new[] { "GroupId" });
            DropIndex("dbo.Disciplines", new[] { "Teacher_Id" });
            DropIndex("dbo.Disciplines", new[] { "GroupId" });
            DropIndex("dbo.Students", new[] { "GroupId" });
            DropIndex("dbo.Students", new[] { "UserId" });
            DropIndex("dbo.CourseProjects", new[] { "TeacherId" });
            DropIndex("dbo.CourseProjects", new[] { "StudentId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Teachers", new[] { "CathedraId" });
            DropIndex("dbo.Teachers", new[] { "UserId" });
            DropTable("dbo.StudyGroups");
            DropTable("dbo.Statements");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.NeuralNetworkResults");
            DropTable("dbo.Information");
            DropTable("dbo.Disciplines");
            DropTable("dbo.Groups");
            DropTable("dbo.Students");
            DropTable("dbo.CourseProjects");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Teachers");
            DropTable("dbo.Cathedras");
        }
    }
}
