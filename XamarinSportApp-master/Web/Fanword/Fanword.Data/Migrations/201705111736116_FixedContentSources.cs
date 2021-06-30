namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedContentSources : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContentSources", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContentSources", "Email");
        }
    }
}
