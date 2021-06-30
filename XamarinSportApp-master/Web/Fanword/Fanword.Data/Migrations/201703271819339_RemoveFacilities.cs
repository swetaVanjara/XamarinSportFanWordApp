namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFacilities : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "FacilityId", "dbo.Facilities");
            DropIndex("dbo.Events", new[] { "FacilityId" });
            DropColumn("dbo.Events", "FacilityId");
            DropTable("dbo.Facilities");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Facilities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Events", "FacilityId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Events", "FacilityId");
            AddForeignKey("dbo.Events", "FacilityId", "dbo.Facilities", "Id");
        }
    }
}
