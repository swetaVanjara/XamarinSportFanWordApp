namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddANewFieldForAdvertisers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Advertisers", "WebsiteLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Advertisers", "WebsiteLink");
        }
    }
}
