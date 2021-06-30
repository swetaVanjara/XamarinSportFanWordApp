namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdvertiserTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Advertisers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        CompanyName = c.String(nullable: false, maxLength: 500),
                        ContactName = c.String(nullable: false, maxLength: 500),
                        CompanyDescription = c.String(nullable: false),
                        LogoUrl = c.String(nullable: false),
                        LogoContainer = c.String(nullable: false, maxLength: 500),
                        LogoBlob = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ApplicationUsers", "AdvertiserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ApplicationUsers", "AdvertiserId");
            AddForeignKey("dbo.ApplicationUsers", "AdvertiserId", "dbo.Advertisers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUsers", "AdvertiserId", "dbo.Advertisers");
            DropIndex("dbo.ApplicationUsers", new[] { "AdvertiserId" });
            DropColumn("dbo.ApplicationUsers", "AdvertiserId");
            DropTable("dbo.Advertisers");
        }
    }
}
