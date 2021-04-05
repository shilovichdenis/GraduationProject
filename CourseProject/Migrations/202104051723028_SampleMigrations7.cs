namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cathedras", "DepartmentOffice", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cathedras", "DepartmentOffice", c => c.Int(nullable: false));
        }
    }
}
