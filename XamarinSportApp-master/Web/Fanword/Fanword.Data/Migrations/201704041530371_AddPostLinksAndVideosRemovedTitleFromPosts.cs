namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostLinksAndVideosRemovedTitleFromPosts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostLinks",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        LinkUrl = c.String(nullable: false),
                        ImageUrl = c.String(nullable: false),
                        Title = c.String(nullable: false),
                        Content = c.String(),
                        PostId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.PostVideos",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        CreatedById = c.String(maxLength: 128),
                        Url = c.String(nullable: false),
                        Container = c.String(nullable: false),
                        Blob = c.String(nullable: false),
                        PostId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreatedById)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .Index(t => t.CreatedById)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.EventPosts",
                c => new
                    {
                        EventId = c.String(nullable: false, maxLength: 128),
                        PostId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.EventId, t.PostId })
                .ForeignKey("dbo.Events", t => t.EventId)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .Index(t => t.EventId)
                .Index(t => t.PostId);
            
            DropColumn("dbo.Posts", "Title");
            DropColumn("dbo.RssFeeds", "MappedTitle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RssFeeds", "MappedTitle", c => c.String(nullable: false, maxLength: 500));
            AddColumn("dbo.Posts", "Title", c => c.String());
            DropForeignKey("dbo.EventPosts", "PostId", "dbo.Posts");
            DropForeignKey("dbo.EventPosts", "EventId", "dbo.Events");
            DropForeignKey("dbo.PostVideos", "PostId", "dbo.Posts");
            DropForeignKey("dbo.PostVideos", "CreatedById", "dbo.ApplicationUsers");
            DropForeignKey("dbo.PostLinks", "PostId", "dbo.Posts");
            DropIndex("dbo.EventPosts", new[] { "PostId" });
            DropIndex("dbo.EventPosts", new[] { "EventId" });
            DropIndex("dbo.PostVideos", new[] { "PostId" });
            DropIndex("dbo.PostVideos", new[] { "CreatedById" });
            DropIndex("dbo.PostLinks", new[] { "PostId" });
            DropTable("dbo.EventPosts");
            DropTable("dbo.PostVideos");
            DropTable("dbo.PostLinks");
        }
    }
}
