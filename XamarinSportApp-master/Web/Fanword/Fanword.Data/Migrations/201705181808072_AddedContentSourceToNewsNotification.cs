namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedContentSourceToNewsNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NewsNotifications", "ContentSourceId", c => c.String(maxLength: 128));
            CreateIndex("dbo.NewsNotifications", "ContentSourceId");
            AddForeignKey("dbo.NewsNotifications", "ContentSourceId", "dbo.ContentSources", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NewsNotifications", "ContentSourceId", "dbo.ContentSources");
            DropIndex("dbo.NewsNotifications", new[] { "ContentSourceId" });
            DropColumn("dbo.NewsNotifications", "ContentSourceId");
        }
    }
}
