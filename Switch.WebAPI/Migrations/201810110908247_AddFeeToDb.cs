namespace Switch.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFeeToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        FlatAmount = c.Double(),
                        PercentOfTrx = c.Double(),
                        Minimum = c.Double(),
                        Maximum = c.Double(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Fees");
        }
    }
}
