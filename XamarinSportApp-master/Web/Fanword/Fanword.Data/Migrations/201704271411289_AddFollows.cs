namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFollows : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SchoolUsers",
                c => new
                    {
                        SchoolId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.SchoolId, t.UserId })
                .ForeignKey("dbo.Schools", t => t.SchoolId)
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId)
                .Index(t => t.SchoolId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SportUsers",
                c => new
                    {
                        SportId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.SportId, t.UserId })
                .ForeignKey("dbo.Sports", t => t.SportId)
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId)
                .Index(t => t.SportId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TeamUsers",
                c => new
                    {
                        TeamId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.TeamId, t.UserId })
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId)
                .Index(t => t.TeamId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamUsers", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.TeamUsers", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.SportUsers", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.SportUsers", "SportId", "dbo.Sports");
            DropForeignKey("dbo.SchoolUsers", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.SchoolUsers", "SchoolId", "dbo.Schools");
            DropIndex("dbo.TeamUsers", new[] { "UserId" });
            DropIndex("dbo.TeamUsers", new[] { "TeamId" });
            DropIndex("dbo.SportUsers", new[] { "UserId" });
            DropIndex("dbo.SportUsers", new[] { "SportId" });
            DropIndex("dbo.SchoolUsers", new[] { "UserId" });
            DropIndex("dbo.SchoolUsers", new[] { "SchoolId" });
            DropTable("dbo.TeamUsers");
            DropTable("dbo.SportUsers");
            DropTable("dbo.SchoolUsers");
        }
    }
}
