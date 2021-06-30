namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostShareData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "SharedFromPostId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Posts", "SharedFromPostId");
            AddForeignKey("dbo.Posts", "SharedFromPostId", "dbo.Posts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "SharedFromPostId", "dbo.Posts");
            DropIndex("dbo.Posts", new[] { "SharedFromPostId" });
            DropColumn("dbo.Posts", "SharedFromPostId");
        }
    }
}
