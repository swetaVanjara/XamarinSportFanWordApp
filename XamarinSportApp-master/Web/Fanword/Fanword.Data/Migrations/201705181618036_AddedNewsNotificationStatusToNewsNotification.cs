namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNewsNotificationStatusToNewsNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NewsNotifications", "NewsNotificationStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NewsNotifications", "NewsNotificationStatus");
        }
    }
}
