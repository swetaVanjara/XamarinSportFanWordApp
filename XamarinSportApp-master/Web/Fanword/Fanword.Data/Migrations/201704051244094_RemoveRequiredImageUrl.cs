namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequiredImageUrl : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PostLinks", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PostLinks", "ImageUrl", c => c.String(nullable: false));
        }
    }
}
