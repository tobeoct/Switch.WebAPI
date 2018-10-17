namespace Switch.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNameToSchemeInDb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schemes", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schemes", "Name");
        }
    }
}
