namespace HTTP_5212_RNA_Group4_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Article_Model : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        ArticleID = c.Int(nullable: false, identity: true),
                        ArticleTitle = c.String(),
                        ArticleContent = c.String(),
                        HUpdateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ArticleID)
                .ForeignKey("dbo.HUpdates", t => t.HUpdateId, cascadeDelete: true)
                .Index(t => t.HUpdateId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Articles", "HUpdateId", "dbo.HUpdates");
            DropIndex("dbo.Articles", new[] { "HUpdateId" });
            DropTable("dbo.Articles");
        }
    }
}
