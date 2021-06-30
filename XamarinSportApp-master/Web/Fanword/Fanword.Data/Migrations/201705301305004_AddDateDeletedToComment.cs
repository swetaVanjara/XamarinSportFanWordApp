namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateDeletedToComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "DateDeletedUtc", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "DateDeletedUtc");
        }
    }
}
