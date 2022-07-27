namespace HTTP_5212_RNA_Group4_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pharmacyimage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pharmacies", "PharmacyHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Pharmacies", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pharmacies", "PicExtension");
            DropColumn("dbo.Pharmacies", "PharmacyHasPic");
        }
    }
}
