namespace HTTP_5212_RNA_Group4_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StaffPharmacyPrescriptionAddition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pharmacies", "StaffId", c => c.Int());
            AddColumn("dbo.Pharmacies", "LastName", c => c.String());
            AddColumn("dbo.Pharmacies", "Title", c => c.String());
            AddColumn("dbo.Prescriptions", "StaffId", c => c.Int());
            AddColumn("dbo.Prescriptions", "FirstName", c => c.String());
            AddColumn("dbo.Prescriptions", "LastName", c => c.String());
            AddColumn("dbo.Prescriptions", "Title", c => c.String());
            CreateIndex("dbo.Pharmacies", "StaffId");
            CreateIndex("dbo.Prescriptions", "StaffId");
            AddForeignKey("dbo.Pharmacies", "StaffId", "dbo.Staffs", "StaffId");
            AddForeignKey("dbo.Prescriptions", "StaffId", "dbo.Staffs", "StaffId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prescriptions", "StaffId", "dbo.Staffs");
            DropForeignKey("dbo.Pharmacies", "StaffId", "dbo.Staffs");
            DropIndex("dbo.Prescriptions", new[] { "StaffId" });
            DropIndex("dbo.Pharmacies", new[] { "StaffId" });
            DropColumn("dbo.Prescriptions", "Title");
            DropColumn("dbo.Prescriptions", "LastName");
            DropColumn("dbo.Prescriptions", "FirstName");
            DropColumn("dbo.Prescriptions", "StaffId");
            DropColumn("dbo.Pharmacies", "Title");
            DropColumn("dbo.Pharmacies", "LastName");
            DropColumn("dbo.Pharmacies", "StaffId");
        }
    }
}
