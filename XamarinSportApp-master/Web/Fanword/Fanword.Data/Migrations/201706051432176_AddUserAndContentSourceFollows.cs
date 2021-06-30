namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserAndContentSourceFollows : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContentSourceUsers",
                c => new
                    {
                        ContentSourceId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ContentSourceId, t.UserId })
                .ForeignKey("dbo.ContentSources", t => t.ContentSourceId)
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId)
                .Index(t => t.ContentSourceId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserFollows",
                c => new
                    {
                        UserBeingFollowedId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserBeingFollowedId, t.UserId })
                .ForeignKey("dbo.ApplicationUsers", t => t.UserBeingFollowedId)
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId)
                .Index(t => t.UserBeingFollowedId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserFollows", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.UserFollows", "UserBeingFollowedId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ContentSourceUsers", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ContentSourceUsers", "ContentSourceId", "dbo.ContentSources");
            DropIndex("dbo.UserFollows", new[] { "UserId" });
            DropIndex("dbo.UserFollows", new[] { "UserBeingFollowedId" });
            DropIndex("dbo.ContentSourceUsers", new[] { "UserId" });
            DropIndex("dbo.ContentSourceUsers", new[] { "ContentSourceId" });
            DropTable("dbo.UserFollows");
            DropTable("dbo.ContentSourceUsers");
        }
    }
}
