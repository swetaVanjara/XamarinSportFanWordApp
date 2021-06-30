namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostShares : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostShares",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        CreatedById = c.String(nullable: false, maxLength: 128),
                        PostId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreatedById)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .Index(t => t.CreatedById)
                .Index(t => t.PostId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostShares", "PostId", "dbo.Posts");
            DropForeignKey("dbo.PostShares", "CreatedById", "dbo.ApplicationUsers");
            DropIndex("dbo.PostShares", new[] { "PostId" });
            DropIndex("dbo.PostShares", new[] { "CreatedById" });
            DropTable("dbo.PostShares");
        }
    }
}
