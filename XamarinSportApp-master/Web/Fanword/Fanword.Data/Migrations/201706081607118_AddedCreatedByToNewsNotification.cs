namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCreatedByToNewsNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NewsNotifications", "CreatedBy", c => c.String(maxLength: 128));
            CreateIndex("dbo.NewsNotifications", "CreatedBy");
            AddForeignKey("dbo.NewsNotifications", "CreatedBy", "dbo.ApplicationUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NewsNotifications", "CreatedBy", "dbo.ApplicationUsers");
            DropIndex("dbo.NewsNotifications", new[] { "CreatedBy" });
            DropColumn("dbo.NewsNotifications", "CreatedBy");
        }
    }
}
