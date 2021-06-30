namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedContentSourceToPosts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "ContentSourceId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Posts", "ContentSourceId");
            AddForeignKey("dbo.Posts", "ContentSourceId", "dbo.ContentSources", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "ContentSourceId", "dbo.ContentSources");
            DropIndex("dbo.Posts", new[] { "ContentSourceId" });
            DropColumn("dbo.Posts", "ContentSourceId");
        }
    }
}
