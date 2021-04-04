namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations5 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Teachers", new[] { "UserId" });
            DropIndex("dbo.Students", new[] { "UserId" });
            AlterColumn("dbo.Cathedras", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Cathedras", "Building", c => c.String(nullable: false));
            AlterColumn("dbo.Teachers", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.CourseProjects", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Groups", "Faculty", c => c.String(nullable: false));
            AlterColumn("dbo.Disciplines", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Information", "Info", c => c.String(nullable: false));
            CreateIndex("dbo.Teachers", "UserId");
            CreateIndex("dbo.Students", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Students", new[] { "UserId" });
            DropIndex("dbo.Teachers", new[] { "UserId" });
            AlterColumn("dbo.Information", "Info", c => c.String());
            AlterColumn("dbo.Disciplines", "Name", c => c.String());
            AlterColumn("dbo.Groups", "Faculty", c => c.String());
            AlterColumn("dbo.Students", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.CourseProjects", "Name", c => c.String());
            AlterColumn("dbo.Teachers", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Cathedras", "Building", c => c.String());
            AlterColumn("dbo.Cathedras", "Name", c => c.String());
            CreateIndex("dbo.Students", "UserId");
            CreateIndex("dbo.Teachers", "UserId");
        }
    }
}
