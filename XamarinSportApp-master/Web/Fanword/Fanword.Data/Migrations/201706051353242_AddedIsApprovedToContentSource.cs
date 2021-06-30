namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIsApprovedToContentSource : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContentSources", "IsApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContentSources", "IsApproved");
        }
    }
}
