namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForgotToMakeDateDeletedNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RssFeeds", "DateDeletedUtc", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RssFeeds", "DateDeletedUtc", c => c.DateTime(nullable: false));
        }
    }
}
