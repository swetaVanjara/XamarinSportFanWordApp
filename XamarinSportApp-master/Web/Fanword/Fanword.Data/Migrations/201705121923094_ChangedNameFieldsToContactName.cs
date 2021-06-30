namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedNameFieldsToContactName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContentSources", "ContactName", c => c.String());
            DropColumn("dbo.ContentSources", "FirstName");
            DropColumn("dbo.ContentSources", "LastName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContentSources", "LastName", c => c.String());
            AddColumn("dbo.ContentSources", "FirstName", c => c.String());
            DropColumn("dbo.ContentSources", "ContactName");
        }
    }
}
