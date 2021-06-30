namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTeamsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        SchoolId = c.String(nullable: false, maxLength: 128),
                        SportId = c.String(nullable: false, maxLength: 128),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        DateDeletedUtc = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        Nickname = c.String(maxLength: 100),
                        PrimaryColor = c.String(nullable: false, maxLength: 10),
                        ProfileContainer = c.String(nullable: false, maxLength: 500),
                        ProfileBlob = c.String(nullable: false, maxLength: 500),
                        ProfilePublicUrl = c.String(nullable: false),
                        FacebookUrl = c.String(),
                        TwitterUrl = c.String(),
                        InstagramUrl = c.String(),
                        WebsiteUrl = c.String(),
                        RosterUrl = c.String(),
                        ScheduleUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schools", t => t.SchoolId)
                .ForeignKey("dbo.Sports", t => t.SportId)
                .Index(t => t.SchoolId)
                .Index(t => t.SportId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Teams", "SportId", "dbo.Sports");
            DropForeignKey("dbo.Teams", "SchoolId", "dbo.Schools");
            DropIndex("dbo.Teams", new[] { "SportId" });
            DropIndex("dbo.Teams", new[] { "SchoolId" });
            DropTable("dbo.Teams");
        }
    }
}
