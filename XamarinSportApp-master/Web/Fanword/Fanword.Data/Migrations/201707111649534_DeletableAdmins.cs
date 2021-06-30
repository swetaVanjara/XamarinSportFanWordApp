namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletableAdmins : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeamAdmins", "DateDeletedUtc", c => c.DateTime());
            AddColumn("dbo.SchoolAdmins", "DateDeletedUtc", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SchoolAdmins", "DateDeletedUtc");
            DropColumn("dbo.TeamAdmins", "DateDeletedUtc");
        }
    }
}
