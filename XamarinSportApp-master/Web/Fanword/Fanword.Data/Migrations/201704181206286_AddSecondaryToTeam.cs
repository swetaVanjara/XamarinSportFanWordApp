namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSecondaryToTeam : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teams", "SecondaryColor", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teams", "SecondaryColor");
        }
    }
}
