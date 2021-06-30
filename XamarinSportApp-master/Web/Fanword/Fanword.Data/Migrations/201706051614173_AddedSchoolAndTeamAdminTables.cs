namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSchoolAndTeamAdminTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SchoolAdmins",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false),
                        SchoolId = c.String(nullable: false),
                        AdminStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TeamAdmins",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false),
                        TeamId = c.String(nullable: false),
                        AdminStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TeamAdmins");
            DropTable("dbo.SchoolAdmins");
        }
    }
}
