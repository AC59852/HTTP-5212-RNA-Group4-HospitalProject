namespace HTTP_5212_RNA_Group4_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StaffAppointmentMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentId = c.Int(nullable: false, identity: true),
                        PatientName = c.String(),
                        AppointmentDate = c.DateTime(nullable: false),
                        PatientNotes = c.String(),
                        StaffID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AppointmentId)
                .ForeignKey("dbo.Staffs", t => t.StaffID, cascadeDelete: true)
                .Index(t => t.StaffID);
            
            CreateTable(
                "dbo.Staffs",
                c => new
                    {
                        StaffId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Title = c.String(),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.StaffId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "StaffID", "dbo.Staffs");
            DropIndex("dbo.Appointments", new[] { "StaffID" });
            DropTable("dbo.Staffs");
            DropTable("dbo.Appointments");
        }
    }
}
