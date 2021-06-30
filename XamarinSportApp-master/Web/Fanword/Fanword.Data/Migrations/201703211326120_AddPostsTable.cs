namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        FeedId = c.String(maxLength: 128),
                        DateLastModifiedUtc = c.DateTime(nullable: false),
                        CreatedById = c.String(maxLength: 128),
                        LastModifiedById = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreatedById)
                .ForeignKey("dbo.RssFeeds", t => t.FeedId)
                .ForeignKey("dbo.ApplicationUsers", t => t.LastModifiedById)
                .Index(t => t.FeedId)
                .Index(t => t.CreatedById)
                .Index(t => t.LastModifiedById);
            
            CreateTable(
                "dbo.SchoolPosts",
                c => new
                    {
                        SchoolId = c.String(nullable: false, maxLength: 128),
                        PostId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.SchoolId, t.PostId })
                .ForeignKey("dbo.Schools", t => t.SchoolId)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .Index(t => t.SchoolId)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.TeamPosts",
                c => new
                    {
                        TeamId = c.String(nullable: false, maxLength: 128),
                        PostId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.TeamId, t.PostId })
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .Index(t => t.TeamId)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.SportPosts",
                c => new
                    {
                        SportId = c.String(nullable: false, maxLength: 128),
                        PostId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.SportId, t.PostId })
                .ForeignKey("dbo.Sports", t => t.SportId)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .Index(t => t.SportId)
                .Index(t => t.PostId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "LastModifiedById", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Posts", "FeedId", "dbo.RssFeeds");
            DropForeignKey("dbo.SportPosts", "PostId", "dbo.Posts");
            DropForeignKey("dbo.SportPosts", "SportId", "dbo.Sports");
            DropForeignKey("dbo.TeamPosts", "PostId", "dbo.Posts");
            DropForeignKey("dbo.TeamPosts", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.SchoolPosts", "PostId", "dbo.Posts");
            DropForeignKey("dbo.SchoolPosts", "SchoolId", "dbo.Schools");
            DropForeignKey("dbo.Posts", "CreatedById", "dbo.ApplicationUsers");
            DropIndex("dbo.SportPosts", new[] { "PostId" });
            DropIndex("dbo.SportPosts", new[] { "SportId" });
            DropIndex("dbo.TeamPosts", new[] { "PostId" });
            DropIndex("dbo.TeamPosts", new[] { "TeamId" });
            DropIndex("dbo.SchoolPosts", new[] { "PostId" });
            DropIndex("dbo.SchoolPosts", new[] { "SchoolId" });
            DropIndex("dbo.Posts", new[] { "LastModifiedById" });
            DropIndex("dbo.Posts", new[] { "CreatedById" });
            DropIndex("dbo.Posts", new[] { "FeedId" });
            DropTable("dbo.SportPosts");
            DropTable("dbo.TeamPosts");
            DropTable("dbo.SchoolPosts");
            DropTable("dbo.Posts");
        }
    }
}
