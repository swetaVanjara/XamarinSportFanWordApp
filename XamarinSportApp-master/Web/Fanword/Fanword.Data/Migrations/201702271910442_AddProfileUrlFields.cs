namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProfileUrlFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUsers", "ProfileContainer", c => c.String());
            AddColumn("dbo.ApplicationUsers", "ProfileBlob", c => c.String());
            AddColumn("dbo.ApplicationUsers", "ProfileUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationUsers", "ProfileUrl");
            DropColumn("dbo.ApplicationUsers", "ProfileBlob");
            DropColumn("dbo.ApplicationUsers", "ProfileContainer");
        }
    }
}
