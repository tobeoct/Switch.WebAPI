namespace Switch.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateChannelInDb : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Channels (Name,Code,Description) VALUES ('ATM','10','Channel for Automated Teller Machines')");
            Sql("INSERT INTO Channels (Name,Code,Description) VALUES ('POS','20','Channel for Point-Of-Sales')");
            Sql("INSERT INTO Channels (Name,Code,Description) VALUES ('WEB','30','Channel for Internet or Online Web ')");
        }

        public override void Down()
        {
        }
    }
}
