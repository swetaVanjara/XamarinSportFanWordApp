namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEventManagementPin : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventManagementPins",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        PinNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EventManagementPins");
        }
    }
}
