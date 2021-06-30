namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewsNotificationChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NewsNotifications", "SchoolId", c => c.String(maxLength: 128));
            AddColumn("dbo.NewsNotifications", "TeamId", c => c.String(maxLength: 128));
            AddColumn("dbo.NewsNotifications", "SportId", c => c.String(maxLength: 128));
            AlterColumn("dbo.NewsNotifications", "Content", c => c.String(maxLength: 150));
            CreateIndex("dbo.NewsNotifications", "SchoolId");
            CreateIndex("dbo.NewsNotifications", "TeamId");
            CreateIndex("dbo.NewsNotifications", "SportId");
            AddForeignKey("dbo.NewsNotifications", "SchoolId", "dbo.Schools", "Id");
            AddForeignKey("dbo.NewsNotifications", "SportId", "dbo.Sports", "Id");
            AddForeignKey("dbo.NewsNotifications", "TeamId", "dbo.Teams", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NewsNotifications", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.NewsNotifications", "SportId", "dbo.Sports");
            DropForeignKey("dbo.NewsNotifications", "SchoolId", "dbo.Schools");
            DropIndex("dbo.NewsNotifications", new[] { "SportId" });
            DropIndex("dbo.NewsNotifications", new[] { "TeamId" });
            DropIndex("dbo.NewsNotifications", new[] { "SchoolId" });
            AlterColumn("dbo.NewsNotifications", "Content", c => c.String());
            DropColumn("dbo.NewsNotifications", "SportId");
            DropColumn("dbo.NewsNotifications", "TeamId");
            DropColumn("dbo.NewsNotifications", "SchoolId");
        }
    }
}
