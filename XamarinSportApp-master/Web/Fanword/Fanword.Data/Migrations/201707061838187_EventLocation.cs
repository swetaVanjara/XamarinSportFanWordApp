namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventLocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Location", c => c.String(nullable: false, maxLength: 255, defaultValue: ""));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Location");
        }
    }
}
