namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForgotUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RssFeeds", "Url", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RssFeeds", "Url");
        }
    }
}
