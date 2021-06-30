namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEvents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        DateOfEventUtc = c.DateTime(nullable: false),
                        TimezoneId = c.String(nullable: false),
                        DateOfEventInTimeZone = c.DateTime(nullable: false),
                        Name = c.String(),
                        FacilityId = c.String(nullable: false, maxLength: 128),
                        SportId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Facilities", t => t.FacilityId)
                .ForeignKey("dbo.Sports", t => t.SportId)
                .Index(t => t.FacilityId)
                .Index(t => t.SportId);
            
            CreateTable(
                "dbo.EventTeams",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TeamId = c.String(nullable: false, maxLength: 128),
                        EventId = c.String(nullable: false, maxLength: 128),
                        WinLossTie = c.Int(),
                        ScorePointsOrPlace = c.String(maxLength: 15),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.EventId)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .Index(t => t.TeamId)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.Facilities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "SportId", "dbo.Sports");
            DropForeignKey("dbo.Events", "FacilityId", "dbo.Facilities");
            DropForeignKey("dbo.EventTeams", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.EventTeams", "EventId", "dbo.Events");
            DropIndex("dbo.EventTeams", new[] { "EventId" });
            DropIndex("dbo.EventTeams", new[] { "TeamId" });
            DropIndex("dbo.Events", new[] { "SportId" });
            DropIndex("dbo.Events", new[] { "FacilityId" });
            DropTable("dbo.Facilities");
            DropTable("dbo.EventTeams");
            DropTable("dbo.Events");
        }
    }
}
