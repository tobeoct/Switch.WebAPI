namespace Switch.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRoutesToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SinkNodeId = c.Int(nullable: false),
                        CardPan = c.Long(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SinkNodes", t => t.SinkNodeId, cascadeDelete: true)
                .Index(t => t.SinkNodeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Routes", "SinkNodeId", "dbo.SinkNodes");
            DropIndex("dbo.Routes", new[] { "SinkNodeId" });
            DropTable("dbo.Routes");
        }
    }
}
