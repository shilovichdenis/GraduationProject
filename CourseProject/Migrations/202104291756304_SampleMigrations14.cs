namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "FacultyId", c => c.Int(nullable: false));
            AddColumn("dbo.Groups", "SpecialtyId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "SpecialtyId");
            DropColumn("dbo.Groups", "FacultyId");
        }
    }
}
