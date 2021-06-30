namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDateDeletedToRssFeedAndNavPropsToPosts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RssFeeds", "DateDeletedUtc", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RssFeeds", "DateDeletedUtc");
        }
    }
}
