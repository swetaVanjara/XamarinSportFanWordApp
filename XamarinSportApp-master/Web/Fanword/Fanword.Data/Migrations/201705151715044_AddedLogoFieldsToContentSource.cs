namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLogoFieldsToContentSource : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContentSources", "LogoUrl", c => c.String(nullable: false));
            AddColumn("dbo.ContentSources", "LogoContainer", c => c.String(nullable: false, maxLength: 500));
            AddColumn("dbo.ContentSources", "LogoBlob", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContentSources", "LogoBlob");
            DropColumn("dbo.ContentSources", "LogoContainer");
            DropColumn("dbo.ContentSources", "LogoUrl");
        }
    }
}
