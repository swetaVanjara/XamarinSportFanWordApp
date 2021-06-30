namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDisplayOrderFromEventTeams : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.EventTeams", "DisplayOrder");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventTeams", "DisplayOrder", c => c.Int(nullable: false));
        }
    }
}
