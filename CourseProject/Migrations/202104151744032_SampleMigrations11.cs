namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cathedras", "About", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cathedras", "About");
        }
    }
}
