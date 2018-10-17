namespace Switch.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateSinkNodes : DbMigration
    {
        public override void Up()
        {
            Sql(
                "INSERT INTO SinkNodes (Name,HostName,IPAddress,Port,Status) VALUES ('Sink Node 1','Sink Host 1','127.0.0.1','80','Active')");
        }
        
        public override void Down()
        {
        }
    }
}
