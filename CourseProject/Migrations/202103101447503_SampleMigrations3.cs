namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Information", "SenderId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Information", "SenderId", c => c.String());
        }
    }
}
