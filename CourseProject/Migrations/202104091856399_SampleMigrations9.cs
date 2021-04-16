namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations9 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Teachers", "CathedraId", "dbo.Cathedras");
            DropIndex("dbo.Teachers", new[] { "CathedraId" });
            AlterColumn("dbo.Teachers", "CathedraId", c => c.Int());
            CreateIndex("dbo.Teachers", "CathedraId");
            AddForeignKey("dbo.Teachers", "CathedraId", "dbo.Cathedras", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Teachers", "CathedraId", "dbo.Cathedras");
            DropIndex("dbo.Teachers", new[] { "CathedraId" });
            AlterColumn("dbo.Teachers", "CathedraId", c => c.Int(nullable: false));
            CreateIndex("dbo.Teachers", "CathedraId");
            AddForeignKey("dbo.Teachers", "CathedraId", "dbo.Cathedras", "Id", cascadeDelete: true);
        }
    }
}
