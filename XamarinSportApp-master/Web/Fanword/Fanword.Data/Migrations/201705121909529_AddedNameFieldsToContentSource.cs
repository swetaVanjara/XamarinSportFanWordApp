namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNameFieldsToContentSource : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContentSources", "FirstName", c => c.String());
            AddColumn("dbo.ContentSources", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContentSources", "LastName");
            DropColumn("dbo.ContentSources", "FirstName");
        }
    }
}
