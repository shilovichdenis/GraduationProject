namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StudyGroups", "Teacher_Id", "dbo.Teachers");
            DropIndex("dbo.StudyGroups", new[] { "Teacher_Id" });
            DropColumn("dbo.StudyGroups", "TeacherId");
            RenameColumn(table: "dbo.StudyGroups", name: "Teacher_Id", newName: "TeacherId");
            AlterColumn("dbo.StudyGroups", "TeacherId", c => c.Int(nullable: false));
            AlterColumn("dbo.StudyGroups", "TeacherId", c => c.Int(nullable: false));
            CreateIndex("dbo.StudyGroups", "TeacherId");
            AddForeignKey("dbo.StudyGroups", "TeacherId", "dbo.Teachers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudyGroups", "TeacherId", "dbo.Teachers");
            DropIndex("dbo.StudyGroups", new[] { "TeacherId" });
            AlterColumn("dbo.StudyGroups", "TeacherId", c => c.Int());
            AlterColumn("dbo.StudyGroups", "TeacherId", c => c.String());
            RenameColumn(table: "dbo.StudyGroups", name: "TeacherId", newName: "Teacher_Id");
            AddColumn("dbo.StudyGroups", "TeacherId", c => c.String());
            CreateIndex("dbo.StudyGroups", "Teacher_Id");
            AddForeignKey("dbo.StudyGroups", "Teacher_Id", "dbo.Teachers", "Id");
        }
    }
}
