namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostImages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostImages",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Container = c.String(),
                        Blob = c.String(),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        Url = c.String(nullable: false),
                        PostId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .Index(t => t.PostId);
            
            AddColumn("dbo.Posts", "Content", c => c.String());
            AddColumn("dbo.Posts", "Title", c => c.String());
            AddColumn("dbo.RssFeeds", "MappedBody", c => c.String(nullable: false, maxLength: 500));
            DropColumn("dbo.RssFeeds", "MappedCaption");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RssFeeds", "MappedCaption", c => c.String(nullable: false, maxLength: 500));
            DropForeignKey("dbo.PostImages", "PostId", "dbo.Posts");
            DropIndex("dbo.PostImages", new[] { "PostId" });
            DropColumn("dbo.RssFeeds", "MappedBody");
            DropColumn("dbo.Posts", "Title");
            DropColumn("dbo.Posts", "Content");
            DropTable("dbo.PostImages");
        }
    }
}
