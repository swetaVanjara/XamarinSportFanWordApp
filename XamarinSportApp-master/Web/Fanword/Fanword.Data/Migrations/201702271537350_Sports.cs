namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sports : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sports",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateDeletedUtc = c.DateTime(),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Gender = c.Int(nullable: false),
                        IconContainer = c.String(nullable: false, maxLength: 500),
                        IconBlobName = c.String(nullable: false, maxLength: 500),
                        IconPublicUrl = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Sports");
        }
    }
}
