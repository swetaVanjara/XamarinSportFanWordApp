namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSportIdToPost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "SportId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Posts", "SportId");
            AddForeignKey("dbo.Posts", "SportId", "dbo.Sports", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "SportId", "dbo.Sports");
            DropIndex("dbo.Posts", new[] { "SportId" });
            DropColumn("dbo.Posts", "SportId");
        }
    }
}
