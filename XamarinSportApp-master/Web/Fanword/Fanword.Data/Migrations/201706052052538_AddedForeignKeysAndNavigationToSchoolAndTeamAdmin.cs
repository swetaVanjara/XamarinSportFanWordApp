namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedForeignKeysAndNavigationToSchoolAndTeamAdmin : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SchoolAdmins", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.SchoolAdmins", "SchoolId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.TeamAdmins", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.TeamAdmins", "TeamId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.SchoolAdmins", "UserId");
            CreateIndex("dbo.SchoolAdmins", "SchoolId");
            CreateIndex("dbo.TeamAdmins", "UserId");
            CreateIndex("dbo.TeamAdmins", "TeamId");
            AddForeignKey("dbo.SchoolAdmins", "SchoolId", "dbo.Schools", "Id");
            AddForeignKey("dbo.SchoolAdmins", "UserId", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.TeamAdmins", "TeamId", "dbo.Teams", "Id");
            AddForeignKey("dbo.TeamAdmins", "UserId", "dbo.ApplicationUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamAdmins", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.TeamAdmins", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.SchoolAdmins", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.SchoolAdmins", "SchoolId", "dbo.Schools");
            DropIndex("dbo.TeamAdmins", new[] { "TeamId" });
            DropIndex("dbo.TeamAdmins", new[] { "UserId" });
            DropIndex("dbo.SchoolAdmins", new[] { "SchoolId" });
            DropIndex("dbo.SchoolAdmins", new[] { "UserId" });
            AlterColumn("dbo.TeamAdmins", "TeamId", c => c.String(nullable: false));
            AlterColumn("dbo.TeamAdmins", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.SchoolAdmins", "SchoolId", c => c.String(nullable: false));
            AlterColumn("dbo.SchoolAdmins", "UserId", c => c.String(nullable: false));
        }
    }
}
