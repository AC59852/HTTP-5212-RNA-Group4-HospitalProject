namespace HTTP_5212_RNA_Group4_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PharmacyStaffBridge : DbMigration
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
                "dbo.PharmacyStaffs",
                c => new
                    {
                        Pharmacy_PharmacyID = c.Int(nullable: false),
                        Staff_StaffId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Pharmacy_PharmacyID, t.Staff_StaffId })
                .ForeignKey("dbo.Pharmacies", t => t.Pharmacy_PharmacyID, cascadeDelete: true)
                .ForeignKey("dbo.Staffs", t => t.Staff_StaffId, cascadeDelete: true)
                .Index(t => t.Pharmacy_PharmacyID)
                .Index(t => t.Staff_StaffId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "StaffID", "dbo.Staffs");
            DropForeignKey("dbo.PharmacyStaffs", "Staff_StaffId", "dbo.Staffs");
            DropForeignKey("dbo.PharmacyStaffs", "Pharmacy_PharmacyID", "dbo.Pharmacies");
            DropIndex("dbo.PharmacyStaffs", new[] { "Staff_StaffId" });
            DropIndex("dbo.PharmacyStaffs", new[] { "Pharmacy_PharmacyID" });
            DropIndex("dbo.Appointments", new[] { "StaffID" });
            DropTable("dbo.PharmacyStaffs");
            DropTable("dbo.Appointments");
        }
    }
}
