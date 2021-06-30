namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedCreatedByToCreatedById : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.NewsNotifications", name: "CreatedBy", newName: "CreatedById");
            RenameIndex(table: "dbo.NewsNotifications", name: "IX_CreatedBy", newName: "IX_CreatedById");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.NewsNotifications", name: "IX_CreatedById", newName: "IX_CreatedBy");
            RenameColumn(table: "dbo.NewsNotifications", name: "CreatedById", newName: "CreatedBy");
        }
    }
}
