namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConnectedRssFeedToRssKeyword : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RssKeywords", "RssFeedId", c => c.String(maxLength: 128));
            AlterColumn("dbo.RssKeywords", "Name", c => c.String(nullable: false));
            CreateIndex("dbo.RssKeywords", "RssFeedId");
            AddForeignKey("dbo.RssKeywords", "RssFeedId", "dbo.RssFeeds", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RssKeywords", "RssFeedId", "dbo.RssFeeds");
            DropIndex("dbo.RssKeywords", new[] { "RssFeedId" });
            AlterColumn("dbo.RssKeywords", "Name", c => c.String());
            DropColumn("dbo.RssKeywords", "RssFeedId");
        }
    }
}
