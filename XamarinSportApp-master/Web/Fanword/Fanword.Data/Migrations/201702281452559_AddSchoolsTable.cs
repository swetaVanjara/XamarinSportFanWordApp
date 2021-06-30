namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSchoolsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Schools",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        DateDeletedUtc = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Nickname = c.String(maxLength: 100),
                        PrimaryColor = c.String(nullable: false, maxLength: 10),
                        ProfileContainer = c.String(nullable: false, maxLength: 500),
                        ProfileBlob = c.String(nullable: false, maxLength: 500),
                        ProfilePublicUrl = c.String(nullable: false),
                        FacebookUrl = c.String(),
                        TwitterUrl = c.String(),
                        InstagramUrl = c.String(),
                        WebsiteUrl = c.String(),
                        RosterUrl = c.String(),
                        ScheduleUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Schools");
        }
    }
}
