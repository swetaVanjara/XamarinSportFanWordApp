namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationsAndRankingTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewsNotifications",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        Title = c.String(nullable: false),
                        Content = c.String(),
                        HangfireTaskId = c.String(),
                        PushDateUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ForUserId = c.String(nullable: false, maxLength: 128),
                        ByUserId = c.String(maxLength: 128),
                        Title = c.String(nullable: false),
                        Content = c.String(),
                        DateReadUtc = c.DateTime(),
                        PostId = c.String(maxLength: 128),
                        TeamId = c.String(maxLength: 128),
                        SchoolId = c.String(maxLength: 128),
                        SportId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ByUserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ForUserId)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .ForeignKey("dbo.Schools", t => t.SchoolId)
                .ForeignKey("dbo.Sports", t => t.SportId)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .Index(t => t.ForUserId)
                .Index(t => t.ByUserId)
                .Index(t => t.PostId)
                .Index(t => t.TeamId)
                .Index(t => t.SchoolId)
                .Index(t => t.SportId);
            
            CreateTable(
                "dbo.Rankings",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        SportId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sports", t => t.SportId)
                .Index(t => t.SportId);
            
            CreateTable(
                "dbo.RankingTeams",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        Ranking = c.Int(nullable: false),
                        TeamId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .Index(t => t.TeamId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RankingTeams", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Rankings", "SportId", "dbo.Sports");
            DropForeignKey("dbo.UserNotifications", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.UserNotifications", "SportId", "dbo.Sports");
            DropForeignKey("dbo.UserNotifications", "SchoolId", "dbo.Schools");
            DropForeignKey("dbo.UserNotifications", "PostId", "dbo.Posts");
            DropForeignKey("dbo.UserNotifications", "ForUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.UserNotifications", "ByUserId", "dbo.ApplicationUsers");
            DropIndex("dbo.RankingTeams", new[] { "TeamId" });
            DropIndex("dbo.Rankings", new[] { "SportId" });
            DropIndex("dbo.UserNotifications", new[] { "SportId" });
            DropIndex("dbo.UserNotifications", new[] { "SchoolId" });
            DropIndex("dbo.UserNotifications", new[] { "TeamId" });
            DropIndex("dbo.UserNotifications", new[] { "PostId" });
            DropIndex("dbo.UserNotifications", new[] { "ByUserId" });
            DropIndex("dbo.UserNotifications", new[] { "ForUserId" });
            DropTable("dbo.RankingTeams");
            DropTable("dbo.Rankings");
            DropTable("dbo.UserNotifications");
            DropTable("dbo.NewsNotifications");
        }
    }
}
