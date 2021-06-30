namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationRules",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        Qualifier = c.String(nullable: false),
                        ParentRuleId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationRules", t => t.ParentRuleId)
                .Index(t => t.ParentRuleId);
            
            CreateTable(
                "dbo.ApplicationRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        DateModifiedUtc = c.DateTime(),
                        CreatedById = c.String(maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreatedById)
                .Index(t => t.CreatedById)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        IsEnabled = c.Boolean(nullable: false),
                        LastLoginDateUtc = c.DateTime(),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.ApplicationUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ApplicationUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ApplicationUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId)
                .ForeignKey("dbo.ApplicationRoles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ApplicationRuleApplicationRoles",
                c => new
                    {
                        ApplicationRuleId = c.String(nullable: false, maxLength: 128),
                        ApplicationRoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ApplicationRuleId, t.ApplicationRoleId })
                .ForeignKey("dbo.ApplicationRules", t => t.ApplicationRuleId)
                .ForeignKey("dbo.ApplicationRoles", t => t.ApplicationRoleId)
                .Index(t => t.ApplicationRuleId)
                .Index(t => t.ApplicationRoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationRules", "ParentRuleId", "dbo.ApplicationRules");
            DropForeignKey("dbo.ApplicationRuleApplicationRoles", "ApplicationRoleId", "dbo.ApplicationRoles");
            DropForeignKey("dbo.ApplicationRuleApplicationRoles", "ApplicationRuleId", "dbo.ApplicationRules");
            DropForeignKey("dbo.ApplicationUserRoles", "RoleId", "dbo.ApplicationRoles");
            DropForeignKey("dbo.ApplicationRoles", "CreatedById", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserRoles", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserLogins", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserClaims", "UserId", "dbo.ApplicationUsers");
            DropIndex("dbo.ApplicationRuleApplicationRoles", new[] { "ApplicationRoleId" });
            DropIndex("dbo.ApplicationRuleApplicationRoles", new[] { "ApplicationRuleId" });
            DropIndex("dbo.ApplicationUserRoles", new[] { "RoleId" });
            DropIndex("dbo.ApplicationUserRoles", new[] { "UserId" });
            DropIndex("dbo.ApplicationUserLogins", new[] { "UserId" });
            DropIndex("dbo.ApplicationUserClaims", new[] { "UserId" });
            DropIndex("dbo.ApplicationUsers", "UserNameIndex");
            DropIndex("dbo.ApplicationRoles", "RoleNameIndex");
            DropIndex("dbo.ApplicationRoles", new[] { "CreatedById" });
            DropIndex("dbo.ApplicationRules", new[] { "ParentRuleId" });
            DropTable("dbo.ApplicationRuleApplicationRoles");
            DropTable("dbo.ApplicationUserRoles");
            DropTable("dbo.ApplicationUserLogins");
            DropTable("dbo.ApplicationUserClaims");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.ApplicationRoles");
            DropTable("dbo.ApplicationRules");
        }
    }
}
