namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRssKeywordTypeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RssKeywordTypes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.RssKeywords", "Keyword", c => c.String(nullable: false));
            AddColumn("dbo.RssKeywords", "RssKeywordTypeId", c => c.String(maxLength: 128));
            CreateIndex("dbo.RssKeywords", "RssKeywordTypeId");
            AddForeignKey("dbo.RssKeywords", "RssKeywordTypeId", "dbo.RssKeywordTypes", "Id");
            DropColumn("dbo.RssKeywords", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RssKeywords", "Name", c => c.String(nullable: false));
            DropForeignKey("dbo.RssKeywords", "RssKeywordTypeId", "dbo.RssKeywordTypes");
            DropIndex("dbo.RssKeywords", new[] { "RssKeywordTypeId" });
            DropColumn("dbo.RssKeywords", "RssKeywordTypeId");
            DropColumn("dbo.RssKeywords", "Keyword");
            DropTable("dbo.RssKeywordTypes");
        }
    }
}
