namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedVideoFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PostVideos", "ImageUrl", c => c.String(nullable: false));
            AddColumn("dbo.PostVideos", "ImageContainer", c => c.String(nullable: false));
            AddColumn("dbo.PostVideos", "ImageBlob", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PostVideos", "ImageBlob");
            DropColumn("dbo.PostVideos", "ImageContainer");
            DropColumn("dbo.PostVideos", "ImageUrl");
        }
    }
}
