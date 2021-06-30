namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUrlsFromSchool : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schools", "SecondaryColor", c => c.String(maxLength: 10));
            DropColumn("dbo.Schools", "RosterUrl");
            DropColumn("dbo.Schools", "ScheduleUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schools", "ScheduleUrl", c => c.String());
            AddColumn("dbo.Schools", "RosterUrl", c => c.String());
            DropColumn("dbo.Schools", "SecondaryColor");
        }
    }
}
