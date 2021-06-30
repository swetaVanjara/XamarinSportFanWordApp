namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredKeywordType : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RssKeywords", new[] { "RssKeywordTypeId" });
            AlterColumn("dbo.RssKeywords", "RssKeywordTypeId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.RssKeywords", "RssKeywordTypeId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RssKeywords", new[] { "RssKeywordTypeId" });
            AlterColumn("dbo.RssKeywords", "RssKeywordTypeId", c => c.String(maxLength: 128));
            CreateIndex("dbo.RssKeywords", "RssKeywordTypeId");
        }
    }
}
