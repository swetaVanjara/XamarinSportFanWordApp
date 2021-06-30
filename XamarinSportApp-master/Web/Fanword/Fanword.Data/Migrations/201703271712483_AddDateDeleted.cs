namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateDeleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "DateDeletedUtc", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "DateDeletedUtc");
        }
    }
}
