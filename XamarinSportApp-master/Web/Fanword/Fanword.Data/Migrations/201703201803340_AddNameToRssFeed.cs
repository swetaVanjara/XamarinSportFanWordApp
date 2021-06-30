namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNameToRssFeed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RssFeeds", "Name", c => c.String(nullable: false, maxLength: 300));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RssFeeds", "Name");
        }
    }
}
