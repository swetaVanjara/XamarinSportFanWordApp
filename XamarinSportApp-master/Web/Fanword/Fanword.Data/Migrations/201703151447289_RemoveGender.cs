namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveGender : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Sports", "Gender");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sports", "Gender", c => c.Int(nullable: false));
        }
    }
}
