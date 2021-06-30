namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRefreshTokens : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RefreshTokens",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IssuedUtc = c.DateTime(nullable: false),
                        ExpirationDateUtc = c.DateTime(nullable: false),
                        ProtectedTicket = c.String(nullable: false),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RefreshTokens", "ApplicationUserId", "dbo.ApplicationUsers");
            DropIndex("dbo.RefreshTokens", new[] { "ApplicationUserId" });
            DropTable("dbo.RefreshTokens");
        }
    }
}
