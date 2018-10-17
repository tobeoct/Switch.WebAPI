namespace Switch.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNameToRouteInDb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Routes", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Routes", "Name");
        }
    }
}
