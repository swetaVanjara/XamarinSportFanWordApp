namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentDateCreatedField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "DateCreatedUtc", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "DateCreatedUtc");
        }
    }
}
