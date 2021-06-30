namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredFieldsToRssFeed : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RssFeeds", "MappedCaption", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.RssFeeds", "MappedTitle", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.RssFeeds", "MappedCreatedAt", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RssFeeds", "MappedCreatedAt", c => c.String());
            AlterColumn("dbo.RssFeeds", "MappedTitle", c => c.String());
            AlterColumn("dbo.RssFeeds", "MappedCaption", c => c.String());
        }
    }
}
