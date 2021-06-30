namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedInteractionFieldsToContentSource : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContentSources", "FacebookLink", c => c.String());
            AddColumn("dbo.ContentSources", "FacebookShow", c => c.Boolean(nullable: false));
            AddColumn("dbo.ContentSources", "TwitterLink", c => c.String());
            AddColumn("dbo.ContentSources", "TwitterShow", c => c.Boolean(nullable: false));
            AddColumn("dbo.ContentSources", "InstagramLink", c => c.String());
            AddColumn("dbo.ContentSources", "InstagramShow", c => c.Boolean(nullable: false));
            AddColumn("dbo.ContentSources", "ActionText", c => c.String());
            AddColumn("dbo.ContentSources", "ActionLink", c => c.String());
            AddColumn("dbo.ContentSources", "PrimaryColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContentSources", "PrimaryColor");
            DropColumn("dbo.ContentSources", "ActionLink");
            DropColumn("dbo.ContentSources", "ActionText");
            DropColumn("dbo.ContentSources", "InstagramShow");
            DropColumn("dbo.ContentSources", "InstagramLink");
            DropColumn("dbo.ContentSources", "TwitterShow");
            DropColumn("dbo.ContentSources", "TwitterLink");
            DropColumn("dbo.ContentSources", "FacebookShow");
            DropColumn("dbo.ContentSources", "FacebookLink");
        }
    }
}
