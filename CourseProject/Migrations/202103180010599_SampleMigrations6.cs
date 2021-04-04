namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "YearOfAdmission", c => c.Int(nullable: false));
            DropColumn("dbo.Groups", "Course");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Groups", "Course", c => c.Int(nullable: false));
            DropColumn("dbo.Groups", "YearOfAdmission");
        }
    }
}
