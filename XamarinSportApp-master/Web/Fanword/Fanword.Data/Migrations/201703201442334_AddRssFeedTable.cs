namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRssFeedTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RssFeeds",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        DateLastModifiedUtc = c.DateTime(nullable: false),
                        CreatedById = c.String(nullable: false, maxLength: 128),
                        LastModifiedById = c.String(nullable: false, maxLength: 128),
                        MappedCaption = c.String(),
                        MappedTitle = c.String(),
                        MappedCreatedAt = c.String(),
                        TeamId = c.String(maxLength: 128),
                        SchoolId = c.String(maxLength: 128),
                        Sport_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreatedById)
                .ForeignKey("dbo.ApplicationUsers", t => t.LastModifiedById)
                .ForeignKey("dbo.Sports", t => t.Sport_Id)
                .ForeignKey("dbo.Schools", t => t.SchoolId)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .Index(t => t.CreatedById)
                .Index(t => t.LastModifiedById)
                .Index(t => t.TeamId)
                .Index(t => t.SchoolId)
                .Index(t => t.Sport_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RssFeeds", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.RssFeeds", "SchoolId", "dbo.Schools");
            DropForeignKey("dbo.RssFeeds", "Sport_Id", "dbo.Sports");
            DropForeignKey("dbo.RssFeeds", "LastModifiedById", "dbo.ApplicationUsers");
            DropForeignKey("dbo.RssFeeds", "CreatedById", "dbo.ApplicationUsers");
            DropIndex("dbo.RssFeeds", new[] { "Sport_Id" });
            DropIndex("dbo.RssFeeds", new[] { "SchoolId" });
            DropIndex("dbo.RssFeeds", new[] { "TeamId" });
            DropIndex("dbo.RssFeeds", new[] { "LastModifiedById" });
            DropIndex("dbo.RssFeeds", new[] { "CreatedById" });
            DropTable("dbo.RssFeeds");
        }
    }
}
