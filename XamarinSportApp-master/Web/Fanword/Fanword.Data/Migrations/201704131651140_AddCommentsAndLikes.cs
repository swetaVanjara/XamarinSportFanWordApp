namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentsAndLikes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Content = c.String(),
                        ParentCommentId = c.String(maxLength: 128),
                        CreatedById = c.String(nullable: false, maxLength: 128),
                        PostId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreatedById)
                .ForeignKey("dbo.Comments", t => t.ParentCommentId)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .Index(t => t.ParentCommentId)
                .Index(t => t.CreatedById)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.PostLikes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        CreatedById = c.String(nullable: false, maxLength: 128),
                        PostId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreatedById)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .Index(t => t.CreatedById)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.CommentLikes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        CreatedById = c.String(nullable: false, maxLength: 128),
                        CommentId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comments", t => t.CommentId)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreatedById)
                .Index(t => t.CreatedById)
                .Index(t => t.CommentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentLikes", "CreatedById", "dbo.ApplicationUsers");
            DropForeignKey("dbo.CommentLikes", "CommentId", "dbo.Comments");
            DropForeignKey("dbo.PostLikes", "PostId", "dbo.Posts");
            DropForeignKey("dbo.PostLikes", "CreatedById", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Comments", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Comments", "ParentCommentId", "dbo.Comments");
            DropForeignKey("dbo.Comments", "CreatedById", "dbo.ApplicationUsers");
            DropIndex("dbo.CommentLikes", new[] { "CommentId" });
            DropIndex("dbo.CommentLikes", new[] { "CreatedById" });
            DropIndex("dbo.PostLikes", new[] { "PostId" });
            DropIndex("dbo.PostLikes", new[] { "CreatedById" });
            DropIndex("dbo.Comments", new[] { "PostId" });
            DropIndex("dbo.Comments", new[] { "CreatedById" });
            DropIndex("dbo.Comments", new[] { "ParentCommentId" });
            DropTable("dbo.CommentLikes");
            DropTable("dbo.PostLikes");
            DropTable("dbo.Comments");
        }
    }
}
