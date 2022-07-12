namespace HTTP_5212_RNA_Group4_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StaffPharmacyRemoval : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pharmacies", "StaffId", "dbo.Staffs");
            DropIndex("dbo.Pharmacies", new[] { "StaffId" });
            DropColumn("dbo.Pharmacies", "StaffId");
            DropColumn("dbo.Pharmacies", "LastName");
            DropColumn("dbo.Pharmacies", "Title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pharmacies", "Title", c => c.String());
            AddColumn("dbo.Pharmacies", "LastName", c => c.String());
            AddColumn("dbo.Pharmacies", "StaffId", c => c.Int());
            CreateIndex("dbo.Pharmacies", "StaffId");
            AddForeignKey("dbo.Pharmacies", "StaffId", "dbo.Staffs", "StaffId");
        }
    }
}
