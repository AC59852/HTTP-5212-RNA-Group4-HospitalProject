namespace HTTP_5212_RNA_Group4_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prescriptions", "PharmacyName", c => c.String());
            AddColumn("dbo.Prescriptions", "FirstName", c => c.String());
            AddColumn("dbo.Prescriptions", "LastName", c => c.String());
            AddColumn("dbo.Prescriptions", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prescriptions", "Title");
            DropColumn("dbo.Prescriptions", "LastName");
            DropColumn("dbo.Prescriptions", "FirstName");
            DropColumn("dbo.Prescriptions", "PharmacyName");
        }
    }
}
