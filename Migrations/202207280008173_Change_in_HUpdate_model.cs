namespace HTTP_5212_RNA_Group4_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_in_HUpdate_model : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HUpdates", "HUpdateType", c => c.Int(nullable: false));
            DropColumn("dbo.HUpdates", "HUpdateTagId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HUpdates", "HUpdateTagId", c => c.Int(nullable: false));
            DropColumn("dbo.HUpdates", "HUpdateType");
        }
    }
}
