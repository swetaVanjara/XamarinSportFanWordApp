namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixRssTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RssFeeds", "Sport_Id", "dbo.Sports");
            DropIndex("dbo.RssFeeds", new[] { "Sport_Id" });
            DropColumn("dbo.RssFeeds", "Sport_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RssFeeds", "Sport_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.RssFeeds", "Sport_Id");
            AddForeignKey("dbo.RssFeeds", "Sport_Id", "dbo.Sports", "Id");
        }
    }
}
