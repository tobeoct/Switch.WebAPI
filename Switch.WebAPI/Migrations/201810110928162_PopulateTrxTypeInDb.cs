namespace Switch.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateTrxTypeInDb : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO TransactionTypes (Name,Code,Description) VALUES ('Withdrawal','01','All Withdrawals')");
            Sql("INSERT INTO TransactionTypes (Name,Code,Description) VALUES ('Payment','02','All Payments')");
        }
        
        public override void Down()
        {
        }
    }
}
