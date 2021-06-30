namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedContentSourceAndContentSourceRegistration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContentSources",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        ContentSourceName = c.String(nullable: false, maxLength: 500),
                        ContentSourceDescription = c.String(nullable: false),
                        WebsiteLink = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ApplicationUsers", "ContentSourceId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ApplicationUsers", "ContentSourceId");
            AddForeignKey("dbo.ApplicationUsers", "ContentSourceId", "dbo.ContentSources", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUsers", "ContentSourceId", "dbo.ContentSources");
            DropIndex("dbo.ApplicationUsers", new[] { "ContentSourceId" });
            DropColumn("dbo.ApplicationUsers", "ContentSourceId");
            DropTable("dbo.ContentSources");
        }
    }
}
