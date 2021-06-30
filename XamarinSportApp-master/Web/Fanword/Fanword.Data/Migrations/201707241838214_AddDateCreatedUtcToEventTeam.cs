namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateCreatedUtcToEventTeam : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventTeams", "DateCreatedUtc", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventTeams", "DateCreatedUtc");
        }
    }
}
