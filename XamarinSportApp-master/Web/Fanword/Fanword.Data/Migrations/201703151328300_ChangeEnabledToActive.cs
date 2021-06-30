namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeEnabledToActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUsers", "IsActive", c => c.Boolean(nullable: false,defaultValue:true));
            DropColumn("dbo.ApplicationUsers", "IsEnabled");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationUsers", "IsEnabled", c => c.Boolean(nullable: false));
            DropColumn("dbo.ApplicationUsers", "IsActive");
        }
    }
}
