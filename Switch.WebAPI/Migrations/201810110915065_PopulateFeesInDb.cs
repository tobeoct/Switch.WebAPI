namespace Switch.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateFeesInDb : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Fees (Name,FlatAmount,PercentOfTrx,Minimum,Maximum) VALUES ('Fee 1',1000,NULL,NULL,NULL)");
            Sql("INSERT INTO Fees (Name,FlatAmount,PercentOfTrx,Minimum,Maximum) VALUES ('Fee 2',NULL,5,1000,500)");
        }
        
        public override void Down()
        {
        }
    }
}
