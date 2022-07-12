namespace HTTP_5212_RNA_Group4_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddServiceClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentID = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(),
                    })
                .PrimaryKey(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ServiceID = c.Int(nullable: false, identity: true),
                        ServiceName = c.String(),
                        ServiceDescription = c.String(),
                        ServiceImage = c.String(),
                    })
                .PrimaryKey(t => t.ServiceID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Services");
            DropTable("dbo.Departments");
        }
    }
}
