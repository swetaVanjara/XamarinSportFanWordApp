namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVerification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Atheletes", "Verified", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Atheletes", "Verified");
        }
    }
}
