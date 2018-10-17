namespace Switch.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateRouteInDbAgain : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Routes (Name,SinkNodeId,CardPan,Description) VALUES ('Route 1',1,1234098756786543,'Route for Routing')");
        }
        
        public override void Down()
        {
        }
    }
}
