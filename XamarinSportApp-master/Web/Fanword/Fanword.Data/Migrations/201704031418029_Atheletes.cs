namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Atheletes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Atheletes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        StartUtc = c.DateTime(nullable: false),
                        EndUtc = c.DateTime(),
                        TeamId = c.String(nullable: false, maxLength: 128),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .Index(t => t.TeamId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Atheletes", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Atheletes", "ApplicationUserId", "dbo.ApplicationUsers");
            DropIndex("dbo.Atheletes", new[] { "ApplicationUserId" });
            DropIndex("dbo.Atheletes", new[] { "TeamId" });
            DropTable("dbo.Atheletes");
        }
    }
}
