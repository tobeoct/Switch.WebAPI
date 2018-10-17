namespace Switch.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSourceNodesToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SourceNodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        HostName = c.String(nullable: false),
                        IPAddress = c.String(nullable: false),
                        Port = c.String(nullable: false),
                        Status = c.String(nullable: false),
                        SchemeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schemes", t => t.SchemeId, cascadeDelete: true)
                .Index(t => t.SchemeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SourceNodes", "SchemeId", "dbo.Schemes");
            DropIndex("dbo.SourceNodes", new[] { "SchemeId" });
            DropTable("dbo.SourceNodes");
        }
    }
}
