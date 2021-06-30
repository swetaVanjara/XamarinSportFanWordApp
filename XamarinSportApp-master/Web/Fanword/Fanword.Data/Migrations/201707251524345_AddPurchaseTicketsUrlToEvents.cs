namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPurchaseTicketsUrlToEvents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "PurchaseTicketsUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "PurchaseTicketsUrl");
        }
    }
}
