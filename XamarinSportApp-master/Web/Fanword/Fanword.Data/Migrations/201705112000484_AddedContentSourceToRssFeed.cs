namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedContentSourceToRssFeed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RssFeeds", "ContentSourceId", c => c.String(maxLength: 128));
            CreateIndex("dbo.RssFeeds", "ContentSourceId");
            AddForeignKey("dbo.RssFeeds", "ContentSourceId", "dbo.ContentSources", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RssFeeds", "ContentSourceId", "dbo.ContentSources");
            DropIndex("dbo.RssFeeds", new[] { "ContentSourceId" });
            DropColumn("dbo.RssFeeds", "ContentSourceId");
        }
    }
}
