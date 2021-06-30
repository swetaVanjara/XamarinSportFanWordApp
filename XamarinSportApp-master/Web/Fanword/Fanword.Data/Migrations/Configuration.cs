using Fanword.Data.Entities;
using Fanword.Data.IdentityConfig.Roles;

namespace Fanword.Data.Migrations
{
    using Fanword.Data.Enums;
    using Fanword.Data.IdentityConfig.Managers;
    using Fanword.Data.IdentityConfig.User;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<Fanword.Data.Context.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Fanword.Data.Context.ApplicationDbContext context)
        {


            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var roleManager = new ApplicationRoleManager(context);
            var adminRole = roleManager.Roles.FirstOrDefault(i => i.Name == AppRoles.SystemAdmin);
            if (adminRole == null)
            {
                adminRole = new ApplicationRole(AppRoles.SystemAdmin);
                roleManager.Create(adminRole);
            }

            var contentSourceRole = roleManager.Roles.Include(m => m.ApplicationRules).FirstOrDefault(i => i.Name == AppRoles.ContentSource);
            if (contentSourceRole == null)
            {
                contentSourceRole = new ApplicationRole(AppRoles.ContentSource);

                roleManager.Create(contentSourceRole);
                contentSourceRole = roleManager.Roles.Include(m => m.ApplicationRules).FirstOrDefault(m => m.Id == contentSourceRole.Id);
            }

            var advertiserRole = roleManager.Roles.Include(m => m.ApplicationRules).FirstOrDefault(i => i.Name == AppRoles.Advertiser);
            if (advertiserRole == null)
            {
                advertiserRole = new ApplicationRole(AppRoles.Advertiser);

                roleManager.Create(advertiserRole);
                advertiserRole = roleManager.Roles.Include(m => m.ApplicationRules).FirstOrDefault(m => m.Id == advertiserRole.Id);
            }

            var teamAdminRole = roleManager.Roles.Include(m => m.ApplicationRules).FirstOrDefault(i => i.Name == AppRoles.TeamAdmin);
            if (teamAdminRole == null)
            {
                teamAdminRole = new ApplicationRole(AppRoles.TeamAdmin);

                roleManager.Create(teamAdminRole);
                teamAdminRole = roleManager.Roles.Include(m => m.ApplicationRules).FirstOrDefault(m => m.Id == teamAdminRole.Id);
            }

            var schoolAdminRole = roleManager.Roles.Include(m => m.ApplicationRules).FirstOrDefault(i => i.Name == AppRoles.SchoolAdmin);
            if (schoolAdminRole == null)
            {
                schoolAdminRole = new ApplicationRole(AppRoles.SchoolAdmin);

                roleManager.Create(schoolAdminRole);
                schoolAdminRole = roleManager.Roles.Include(m => m.ApplicationRules).FirstOrDefault(m => m.Id == schoolAdminRole.Id);
            }

            // Seed users
            var manager = new ApplicationUserManager(context);
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            var supportAccount = context.Users.FirstOrDefault(i => i.Email == "support@agilx.com");
            if (supportAccount == null)
            {

                supportAccount = new ApplicationUser("support@agilx.com", "support@agilx.com", "Agilx", "Support");
                manager.Create(supportAccount, "Password$1");

                manager.AddToRole(supportAccount.Id, adminRole.Name);
            }
            var fanWordAccount = context.Users.FirstOrDefault(i => i.Email == "caumueller@fanword.com");
            if (fanWordAccount == null)
            {
                fanWordAccount = new ApplicationUser("caumueller@fanword.com", "caumueller@fanword.com", "Christopher", "Aumueller");
                manager.Create(fanWordAccount, "!FanWord22");

                manager.AddToRole(fanWordAccount.Id, contentSourceRole.Name);
            }


            //Seed FanWord Content Source
            var fanWordContentSource = context.ContentSources.FirstOrDefault(i => i.Id == "348743ef-abe9-4125-83f1-b59f2adc7bd8");
            if (fanWordContentSource == null)
            {
                fanWordContentSource = new ContentSource()
                {
                    Id = "348743ef-abe9-4125-83f1-b59f2adc7bd8",
                    ContentSourceName = "FanWord",
                    IsApproved = true,
                    DateCreatedUtc = DateTime.UtcNow,
                    ContactName = "Christopher Aumueller",
                    Email = "caumueller@fanword.com",
                    LogoUrl = "https://fanword.blob.core.windows.net/contentsources/FanwordIcon.jpg",
                    LogoContainer = "contentsources",
                    LogoBlob = "FanwordIcon.jpg",
                    ContentSourceDescription = "The Offical FanWord Content Source",
                    ContentSourceUsers = new[] { fanWordAccount },
                    WebsiteLink = "fanword.com",
                };
                context.ContentSources.Add(fanWordContentSource);

            }

        }
    }
}
