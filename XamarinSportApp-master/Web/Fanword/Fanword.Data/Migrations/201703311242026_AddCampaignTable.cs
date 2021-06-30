namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCampaignTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Campaigns",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        StartUtc = c.DateTime(nullable: false),
                        EndUtc = c.DateTime(nullable: false),
                        CampaignStatus = c.Int(nullable: false),
                        ImageUrl = c.String(nullable: false),
                        ImageBlob = c.String(nullable: false),
                        ImageContainer = c.String(nullable: false),
                        Title = c.String(nullable: false, maxLength: 200),
                        Description = c.String(),
                        Weight = c.Int(nullable: false),
                        AdvertiserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisers", t => t.AdvertiserId)
                .Index(t => t.AdvertiserId);
            
            CreateTable(
                "dbo.SchoolCampaigns",
                c => new
                    {
                        CampaignId = c.String(nullable: false, maxLength: 128),
                        SchoolId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CampaignId, t.SchoolId })
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .ForeignKey("dbo.Schools", t => t.SchoolId)
                .Index(t => t.CampaignId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.SportCampaigns",
                c => new
                    {
                        CampaignId = c.String(nullable: false, maxLength: 128),
                        SportId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CampaignId, t.SportId })
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .ForeignKey("dbo.Sports", t => t.SportId)
                .Index(t => t.CampaignId)
                .Index(t => t.SportId);
            
            CreateTable(
                "dbo.TeamCampaigns",
                c => new
                    {
                        CampaignId = c.String(nullable: false, maxLength: 128),
                        TeamId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CampaignId, t.TeamId })
                .ForeignKey("dbo.Campaigns", t => t.CampaignId)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .Index(t => t.CampaignId)
                .Index(t => t.TeamId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamCampaigns", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.TeamCampaigns", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.SportCampaigns", "SportId", "dbo.Sports");
            DropForeignKey("dbo.SportCampaigns", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.SchoolCampaigns", "SchoolId", "dbo.Schools");
            DropForeignKey("dbo.SchoolCampaigns", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.Campaigns", "AdvertiserId", "dbo.Advertisers");
            DropIndex("dbo.TeamCampaigns", new[] { "TeamId" });
            DropIndex("dbo.TeamCampaigns", new[] { "CampaignId" });
            DropIndex("dbo.SportCampaigns", new[] { "SportId" });
            DropIndex("dbo.SportCampaigns", new[] { "CampaignId" });
            DropIndex("dbo.SchoolCampaigns", new[] { "SchoolId" });
            DropIndex("dbo.SchoolCampaigns", new[] { "CampaignId" });
            DropIndex("dbo.Campaigns", new[] { "AdvertiserId" });
            DropTable("dbo.TeamCampaigns");
            DropTable("dbo.SportCampaigns");
            DropTable("dbo.SchoolCampaigns");
            DropTable("dbo.Campaigns");
        }
    }
}
