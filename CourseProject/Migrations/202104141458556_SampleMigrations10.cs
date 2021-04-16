namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teachers", "AcademicDegree", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teachers", "AcademicDegree");
        }
    }
}
