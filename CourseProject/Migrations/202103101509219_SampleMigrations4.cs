namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Information", "DateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Information", "DateTime");
        }
    }
}
