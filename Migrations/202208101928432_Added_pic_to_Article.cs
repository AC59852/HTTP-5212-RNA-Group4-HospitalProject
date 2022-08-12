namespace HTTP_5212_RNA_Group4_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_pic_to_Article : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "ArticleHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Articles", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "PicExtension");
            DropColumn("dbo.Articles", "ArticleHasPic");
        }
    }
}
