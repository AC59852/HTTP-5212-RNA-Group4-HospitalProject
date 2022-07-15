namespace HTTP_5212_RNA_Group4_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class donationReasearch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Donars",
                c => new
                    {
                        DonarID = c.Int(nullable: false, identity: true),
                        DonarName = c.String(),
                        DonarBio = c.String(),
                        Donation = c.Int(nullable: false),
                        ResearchID = c.Int(nullable: false),
                        ResearchName = c.String(),
                    })
                .PrimaryKey(t => t.DonarID)
                .ForeignKey("dbo.Researches", t => t.ResearchID, cascadeDelete: true)
                .Index(t => t.ResearchID);
            
            CreateTable(
                "dbo.Researches",
                c => new
                    {
                        ResearchID = c.Int(nullable: false, identity: true),
                        ResearchName = c.String(),
                        ResearchHead = c.String(),
                        ResearchDesc = c.String(),
                        NoOfCohorts = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ResearchID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Donars", "ResearchID", "dbo.Researches");
            DropIndex("dbo.Donars", new[] { "ResearchID" });
            DropTable("dbo.Researches");
            DropTable("dbo.Donars");
        }
    }
}
