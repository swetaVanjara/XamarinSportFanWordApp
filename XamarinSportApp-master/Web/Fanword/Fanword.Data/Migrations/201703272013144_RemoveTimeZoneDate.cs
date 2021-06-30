namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTimeZoneDate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Events", "DateOfEventInTimeZone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "DateOfEventInTimeZone", c => c.DateTime(nullable: false));
        }
    }
}
