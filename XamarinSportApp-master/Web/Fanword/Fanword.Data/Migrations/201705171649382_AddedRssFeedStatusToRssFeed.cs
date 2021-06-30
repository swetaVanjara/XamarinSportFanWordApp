namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRssFeedStatusToRssFeed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RssFeeds", "RssFeedStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RssFeeds", "RssFeedStatus");
        }
    }
}
