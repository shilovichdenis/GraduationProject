namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Disciplines", "Term", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Disciplines", "Term");
        }
    }
}
