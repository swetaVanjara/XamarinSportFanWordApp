namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SoftDeleteForPostObjects : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "DateDeletedUtc", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "DateDeletedUtc");
        }
    }
}
