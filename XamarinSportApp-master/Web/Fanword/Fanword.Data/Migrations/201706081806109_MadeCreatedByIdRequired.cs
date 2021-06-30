namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeCreatedByIdRequired : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.NewsNotifications", new[] { "CreatedById" });
            AlterColumn("dbo.NewsNotifications", "CreatedById", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.NewsNotifications", "CreatedById");
        }
        
        public override void Down()
        {
            DropIndex("dbo.NewsNotifications", new[] { "CreatedById" });
            AlterColumn("dbo.NewsNotifications", "CreatedById", c => c.String(maxLength: 128));
            CreateIndex("dbo.NewsNotifications", "CreatedById");
        }
    }
}
