namespace Switch.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateRouteInDb : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Fees (Name,FlatAmount,PercentOfTrx,Minimum,Maximum) VALUES ('Fee 3',10000,NULL,NULL,NULL)");
        }
        
        public override void Down()
        {
        }
    }
}
