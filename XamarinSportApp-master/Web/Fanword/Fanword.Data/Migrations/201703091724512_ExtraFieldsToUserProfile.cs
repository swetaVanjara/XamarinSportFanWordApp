namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtraFieldsToUserProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUsers", "DateDeletedUtc", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationUsers", "DateDeletedUtc");
        }
    }
}
