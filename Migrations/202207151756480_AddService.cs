namespace HTTP_5212_RNA_Group4_HospitalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddService : DbMigration
    {
        public override void Up()
        {
            
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Services");
            DropTable("dbo.Departments");
        }
    }
}
