namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "FormOfPayment", c => c.String());
            DropColumn("dbo.Students", "FormOfEducation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "FormOfEducation", c => c.String());
            DropColumn("dbo.Students", "FormOfPayment");
        }
    }
}
