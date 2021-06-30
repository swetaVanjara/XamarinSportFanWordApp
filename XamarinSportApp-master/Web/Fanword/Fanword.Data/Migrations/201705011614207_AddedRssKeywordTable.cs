namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRssKeywordTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RssKeywords",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RssKeywords");
        }
    }
}
