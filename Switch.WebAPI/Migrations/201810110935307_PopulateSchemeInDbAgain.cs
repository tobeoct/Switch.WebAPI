namespace Switch.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateSchemeInDbAgain : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Schemes (Name,RouteId,TransactionTypeId,ChannelId,FeeId,Description) VALUES ('Scheme 1',1,1,1,1,'Scheme for Scheming')");
        }
        
        public override void Down()
        {
        }
    }
}
