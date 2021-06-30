namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowNullLinkTitle : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PostLinks", "Title", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PostLinks", "Title", c => c.String(nullable: false));
        }
    }
}
