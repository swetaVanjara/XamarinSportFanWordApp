namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAspectRatio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "ImageAspectRatio", c => c.Single(nullable: false, defaultValue: 0.5626f));
            AddColumn("dbo.PostImages", "ImageAspectRatio", c => c.Single(nullable: false, defaultValue: 0.5626f));
            AddColumn("dbo.PostLinks", "ImageAspectRatio", c => c.Single(nullable: false, defaultValue: 0.5626f));
            AddColumn("dbo.PostVideos", "ImageAspectRatio", c => c.Single(nullable: false, defaultValue: 0.5626f));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PostVideos", "ImageAspectRatio");
            DropColumn("dbo.PostLinks", "ImageAspectRatio");
            DropColumn("dbo.PostImages", "ImageAspectRatio");
            DropColumn("dbo.Campaigns", "ImageAspectRatio");
        }
    }
}
