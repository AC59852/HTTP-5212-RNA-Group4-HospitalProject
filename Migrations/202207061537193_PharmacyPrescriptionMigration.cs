namespace HTTP_5212_RNA_Group4_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PharmacyPrescriptionMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pharmacies",
                c => new
                    {
                        PharmacyID = c.Int(nullable: false, identity: true),
                        PharmacyName = c.String(),
                        PharmacyLocation = c.String(),
                        PharmacyWaitTime = c.Int(nullable: false),
                        PharmacyOpenTime = c.Int(nullable: false),
                        PharmacyClosetime = c.Int(nullable: false),
                        PharmacyDelivery = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PharmacyID);
            
            CreateTable(
                "dbo.Prescriptions",
                c => new
                    {
                        PrescriptionID = c.Int(nullable: false, identity: true),
                        PatientName = c.String(),
                        PrescriptionDrug = c.String(),
                        PrescriptionRefills = c.Int(nullable: false),
                        PrescriptionDosage = c.String(),
                        PrescriptionInstructions = c.String(),
                        PharmacyID = c.Int(nullable: false),
                        PharmacyName = c.String(),
                    })
                .PrimaryKey(t => t.PrescriptionID)
                .ForeignKey("dbo.Pharmacies", t => t.PharmacyID, cascadeDelete: true)
                .Index(t => t.PharmacyID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prescriptions", "PharmacyID", "dbo.Pharmacies");
            DropIndex("dbo.Prescriptions", new[] { "PharmacyID" });
            DropTable("dbo.Prescriptions");
            DropTable("dbo.Pharmacies");
        }
    }
}
