namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowPostingAsTeamOrSchool : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "TeamId", c => c.String(maxLength: 128));
            AddColumn("dbo.Posts", "SchoolId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Posts", "TeamId");
            CreateIndex("dbo.Posts", "SchoolId");
            AddForeignKey("dbo.Posts", "SchoolId", "dbo.Schools", "Id");
            AddForeignKey("dbo.Posts", "TeamId", "dbo.Teams", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Posts", "SchoolId", "dbo.Schools");
            DropIndex("dbo.Posts", new[] { "SchoolId" });
            DropIndex("dbo.Posts", new[] { "TeamId" });
            DropColumn("dbo.Posts", "SchoolId");
            DropColumn("dbo.Posts", "TeamId");
        }
    }
}
