namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations13 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ScientificWorks");
            AlterColumn("dbo.ScientificWorks", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ScientificWorks", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ScientificWorks");
            AlterColumn("dbo.ScientificWorks", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.ScientificWorks", "Id");
        }
    }
}
