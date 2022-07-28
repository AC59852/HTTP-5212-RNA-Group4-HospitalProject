namespace HTTP_5212_RNA_Group4_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Connected_HUpdate_and_Department : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HUpdates", "DepartmentId", c => c.Int(nullable: false));
            CreateIndex("dbo.HUpdates", "DepartmentId");
            AddForeignKey("dbo.HUpdates", "DepartmentId", "dbo.Departments", "DepartmentID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HUpdates", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.HUpdates", new[] { "DepartmentId" });
            DropColumn("dbo.HUpdates", "DepartmentId");
        }
    }
}
