namespace HTTP_5212_RNA_Group4_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixingCode : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Appointments", "StaffID", "dbo.Staffs");
            DropIndex("dbo.Appointments", new[] { "StaffID" });
            DropTable("dbo.Appointments");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.AppointmentId);
            
            CreateIndex("dbo.Appointments", "StaffID");
            AddForeignKey("dbo.Appointments", "StaffID", "dbo.Staffs", "StaffId", cascadeDelete: true);
        }
    }
}
