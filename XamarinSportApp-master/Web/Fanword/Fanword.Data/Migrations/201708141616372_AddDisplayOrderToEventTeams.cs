namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDisplayOrderToEventTeams : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventTeams", "DisplayOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventTeams", "DisplayOrder");
        }
    }
}
