namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConnectedRssFeedAndSport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RssFeeds", "SportId", c => c.String(maxLength: 128));
            CreateIndex("dbo.RssFeeds", "SportId");
            AddForeignKey("dbo.RssFeeds", "SportId", "dbo.Sports", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RssFeeds", "SportId", "dbo.Sports");
            DropIndex("dbo.RssFeeds", new[] { "SportId" });
            DropColumn("dbo.RssFeeds", "SportId");
        }
    }
}
