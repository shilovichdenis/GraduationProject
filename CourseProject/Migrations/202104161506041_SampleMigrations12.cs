namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations12 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CourseProjects", newName: "Projects");
            CreateTable(
                "dbo.Publications",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Years = c.String(),
                        TeacherId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teachers", t => t.TeacherId, cascadeDelete: true)
                .Index(t => t.TeacherId);
            
            CreateTable(
                "dbo.ScientificWorks",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Years = c.String(),
                        TeacherId = c.Int(nullable: false),
                        About = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teachers", t => t.TeacherId, cascadeDelete: true)
                .Index(t => t.TeacherId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScientificWorks", "TeacherId", "dbo.Teachers");
            DropForeignKey("dbo.Publications", "TeacherId", "dbo.Teachers");
            DropIndex("dbo.ScientificWorks", new[] { "TeacherId" });
            DropIndex("dbo.Publications", new[] { "TeacherId" });
            DropTable("dbo.ScientificWorks");
            DropTable("dbo.Publications");
            RenameTable(name: "dbo.Projects", newName: "CourseProjects");
        }
    }
}
