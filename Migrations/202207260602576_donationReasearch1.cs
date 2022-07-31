namespace HTTP_5212_RNA_Group4_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class donationReasearch1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Researches", "StaffId", c => c.Int(nullable: false));
            AddColumn("dbo.Researches", "FirstName", c => c.String());
            AddColumn("dbo.Researches", "LastName", c => c.String());
            AddColumn("dbo.Researches", "Title", c => c.String());
            CreateIndex("dbo.Researches", "StaffId");
            AddForeignKey("dbo.Researches", "StaffId", "dbo.Staffs", "StaffId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Researches", "StaffId", "dbo.Staffs");
            DropIndex("dbo.Researches", new[] { "StaffId" });
            DropColumn("dbo.Researches", "Title");
            DropColumn("dbo.Researches", "LastName");
            DropColumn("dbo.Researches", "FirstName");
            DropColumn("dbo.Researches", "StaffId");
        }
    }
}
