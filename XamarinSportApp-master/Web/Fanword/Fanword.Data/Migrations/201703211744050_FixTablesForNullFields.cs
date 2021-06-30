namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixTablesForNullFields : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RankingTeams", new[] { "TeamId" });
            AddColumn("dbo.RankingTeams", "RankingNumber", c => c.Int(nullable: false));
            AddColumn("dbo.RankingTeams", "RankingId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.RankingTeams", "TeamId", c => c.String(maxLength: 128));
            CreateIndex("dbo.RankingTeams", "TeamId");
            CreateIndex("dbo.RankingTeams", "RankingId");
            AddForeignKey("dbo.RankingTeams", "RankingId", "dbo.Rankings", "Id");
            DropColumn("dbo.RankingTeams", "Ranking");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RankingTeams", "Ranking", c => c.Int(nullable: false));
            DropForeignKey("dbo.RankingTeams", "RankingId", "dbo.Rankings");
            DropIndex("dbo.RankingTeams", new[] { "RankingId" });
            DropIndex("dbo.RankingTeams", new[] { "TeamId" });
            AlterColumn("dbo.RankingTeams", "TeamId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.RankingTeams", "RankingId");
            DropColumn("dbo.RankingTeams", "RankingNumber");
            CreateIndex("dbo.RankingTeams", "TeamId");
        }
    }
}
