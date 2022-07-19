namespace HTTP_5212_RNA_Group4_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDepartmentToStaff : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Staffs", "DepartmentID", c => c.Int(nullable: false));
            CreateIndex("dbo.Staffs", "DepartmentID");
            AddForeignKey("dbo.Staffs", "DepartmentID", "dbo.Departments", "DepartmentID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Staffs", "DepartmentID", "dbo.Departments");
            DropIndex("dbo.Staffs", new[] { "DepartmentID" });
            DropColumn("dbo.Staffs", "DepartmentID");
        }
    }
}
