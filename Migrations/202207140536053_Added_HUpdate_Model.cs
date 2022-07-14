namespace HTTP_5212_RNA_Group4_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_HUpdate_Model : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HUpdates", "HUpdateTypeId", "dbo.HUpdateTypes");
            DropForeignKey("dbo.Articles", "HUpdateId", "dbo.HUpdates");
            DropIndex("dbo.Articles", new[] { "HUpdateId" });
            DropIndex("dbo.HUpdates", new[] { "HUpdateTypeId" });
            AddColumn("dbo.HUpdates", "HUpdateTitle", c => c.String());
            DropColumn("dbo.HUpdates", "HUpdateName");
            DropColumn("dbo.HUpdates", "HUpdateTypeId");
            DropColumn("dbo.HUpdates", "HUpdateTag");
            DropTable("dbo.Articles");
            DropTable("dbo.HUpdateTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.HUpdateTypes",
                c => new
                    {
                        HUpdateTypeId = c.Int(nullable: false, identity: true),
                        HUpdateTypeName = c.String(),
                    })
                .PrimaryKey(t => t.HUpdateTypeId);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        ArticleID = c.Int(nullable: false, identity: true),
                        ArticleTitle = c.String(),
                        ArticleContent = c.String(),
                        HUpdateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ArticleID);
            
            AddColumn("dbo.HUpdates", "HUpdateTag", c => c.Int(nullable: false));
            AddColumn("dbo.HUpdates", "HUpdateTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.HUpdates", "HUpdateName", c => c.String());
            DropColumn("dbo.HUpdates", "HUpdateTitle");
            CreateIndex("dbo.HUpdates", "HUpdateTypeId");
            CreateIndex("dbo.Articles", "HUpdateId");
            AddForeignKey("dbo.Articles", "HUpdateId", "dbo.HUpdates", "HUpdateId", cascadeDelete: true);
            AddForeignKey("dbo.HUpdates", "HUpdateTypeId", "dbo.HUpdateTypes", "HUpdateTypeId", cascadeDelete: true);
        }
    }
}
