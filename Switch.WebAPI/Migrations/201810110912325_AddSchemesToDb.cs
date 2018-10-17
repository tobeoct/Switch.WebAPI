namespace Switch.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSchemesToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Schemes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionTypeId = c.Int(nullable: false),
                        RouteId = c.Int(nullable: false),
                        ChannelId = c.Int(nullable: false),
                        FeeId = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Channels", t => t.ChannelId, cascadeDelete: true)
                .ForeignKey("dbo.Fees", t => t.FeeId, cascadeDelete: true)
                .ForeignKey("dbo.Routes", t => t.RouteId, cascadeDelete: true)
                .ForeignKey("dbo.TransactionTypes", t => t.TransactionTypeId, cascadeDelete: true)
                .Index(t => t.TransactionTypeId)
                .Index(t => t.RouteId)
                .Index(t => t.ChannelId)
                .Index(t => t.FeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Schemes", "TransactionTypeId", "dbo.TransactionTypes");
            DropForeignKey("dbo.Schemes", "RouteId", "dbo.Routes");
            DropForeignKey("dbo.Schemes", "FeeId", "dbo.Fees");
            DropForeignKey("dbo.Schemes", "ChannelId", "dbo.Channels");
            DropIndex("dbo.Schemes", new[] { "FeeId" });
            DropIndex("dbo.Schemes", new[] { "ChannelId" });
            DropIndex("dbo.Schemes", new[] { "RouteId" });
            DropIndex("dbo.Schemes", new[] { "TransactionTypeId" });
            DropTable("dbo.Schemes");
        }
    }
}
