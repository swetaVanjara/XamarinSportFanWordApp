namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20180119 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.NotificationRegistrations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.NotificationRegistrations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        RegistrationID = c.String(),
                        DateDeletedUtc = c.DateTime(),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
