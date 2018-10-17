namespace Switch.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateSourceNodeInDb : DbMigration
    {
        public override void Up()
        {
            Sql(
                "INSERT INTO SourceNodes (Name,HostName,IPAddress,Port,Status,SchemeId) VALUES ('Source Node 1','Source Host 1','127.0.0.13','82','Active',1)");

        }

        public override void Down()
        {
        }
    }
}
