namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreFieldsToCampaigns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "DateDeletedUtc", c => c.DateTime());
            AddColumn("dbo.Campaigns", "Url", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campaigns", "Url");
            DropColumn("dbo.Campaigns", "DateDeletedUtc");
        }
    }
}
