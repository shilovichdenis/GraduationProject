namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Disciplines", "Teacher_Id", "dbo.Teachers");
            DropIndex("dbo.Disciplines", new[] { "Teacher_Id" });
            DropColumn("dbo.Disciplines", "TeacherId");
            RenameColumn(table: "dbo.Disciplines", name: "Teacher_Id", newName: "TeacherId");
            AlterColumn("dbo.Disciplines", "TeacherId", c => c.Int(nullable: false));
            AlterColumn("dbo.Disciplines", "TeacherId", c => c.Int(nullable: false));
            CreateIndex("dbo.Disciplines", "TeacherId");
            AddForeignKey("dbo.Disciplines", "TeacherId", "dbo.Teachers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Disciplines", "TeacherId", "dbo.Teachers");
            DropIndex("dbo.Disciplines", new[] { "TeacherId" });
            AlterColumn("dbo.Disciplines", "TeacherId", c => c.Int());
            AlterColumn("dbo.Disciplines", "TeacherId", c => c.String());
            RenameColumn(table: "dbo.Disciplines", name: "TeacherId", newName: "Teacher_Id");
            AddColumn("dbo.Disciplines", "TeacherId", c => c.String());
            CreateIndex("dbo.Disciplines", "Teacher_Id");
            AddForeignKey("dbo.Disciplines", "Teacher_Id", "dbo.Teachers", "Id");
        }
    }
}
