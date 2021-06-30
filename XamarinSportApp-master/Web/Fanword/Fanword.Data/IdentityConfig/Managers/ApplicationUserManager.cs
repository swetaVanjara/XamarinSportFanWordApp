using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Fanword.Data.Context;
using Fanword.Data.IdentityConfig.Services;
using Fanword.Data.IdentityConfig.Stores;
using Fanword.Data.IdentityConfig.Roles;
using Fanword.Data.IdentityConfig.User;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Fanword.Data.IdentityConfig.Managers {
    public class ApplicationUserManager : UserManager<ApplicationUser,string> {

        public ApplicationUserManager(ApplicationDbContext ctx)
            : base(new ApplicationUserStore(ctx)) {
        }

       

        public static ApplicationUserManager CreateOwinVersionWithoutUniqueEmail(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext ctx) {
            var manager = new ApplicationUserManager(ctx.Get<ApplicationDbContext>());
            manager.UserValidator = new UserValidator<ApplicationUser>(manager) {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false,
            };
            manager.PasswordValidator = new PasswordValidator() {
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonLetterOrDigit = false,
                RequireUppercase = true,
                RequiredLength = 6,
            };
            //account lockouts
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(10);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            //two factor auth
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>() {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>() {
                BodyFormat = "Your security code is {0}",
                Subject = "Security Code",
            });

            //services
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProvider = options.DataProtectionProvider;
            if (dataProvider != null){
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public static ApplicationUserManager CreateOwinVersionWithUniqueEmail(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext ctx) {
            var manager = new ApplicationUserManager(ctx.Get<ApplicationDbContext>());
            manager.UserValidator = new UserValidator<ApplicationUser>(manager) {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true,
            };
            //manager.PasswordHasher = new AgilxPasswordHasher();
            manager.PasswordValidator = new PasswordValidator() {
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonLetterOrDigit = false,
                RequireUppercase = true,
                RequiredLength = 6,
            };
            //account lockouts
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(10);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            //two factor auth
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>() {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>() {
                BodyFormat = "Your security code is {0}",
                Subject = "Security Code",
            });

            //services
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProvider = options.DataProtectionProvider;
            if (dataProvider != null) {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public void SendEmail(List<string> userIds, string subject, string htmlEmailContent) {
            var emails =
                Users.Where(m => userIds.Contains(m.Id))
                    .Select(i => i.Email)
                    .Where(m => !String.IsNullOrEmpty(m))
                    .ToList();
            var service = new EmailService();
            service.SendAsync(emails, new IdentityMessage() {Body = htmlEmailContent, Subject = subject});
        }

        public Claim GetClaim(string typeName, string userId) {
            var claims = this.GetClaims(userId);
            return claims.FirstOrDefault(m => m.Type == typeName);
        }

        public async Task<Claim> GetClaimAsync(string typeName, string userId) {
            var claims = await GetClaimsAsync(userId);
            return claims.FirstOrDefault(m => m.Type == typeName);
        }

		public void CreateIt(){
			//var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<SproutEntities>()));
            // Configure validation logic for usernames
            UserValidator = new UserValidator<ApplicationUser>(this) {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser> {
                MessageFormat = "Your security code is {0}"
            });
            RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser> {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            EmailService = new EmailService();
            SmsService = new SmsService();
		}


		/// <summary>
        /// return list of users in a role by name.
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="roleManager"></param>
        /// <returns>List Application User</returns>
        public List<ApplicationUser> UsersInRole(string roleName, ApplicationRoleManager roleManager) {
            if (roleManager == null) {
                throw new ArgumentNullException("roleManager", "User Manager cannot be null");
            }
            var thisRole = roleManager.Roles.Include(i => i.Users).FirstOrDefault(m => m.Name == roleName);
            if (thisRole == null) {
                throw new Exception("Role provided does not exist in current database");
            }
            var users = thisRole.Users.Select(i => i.UserId).ToList();
            return Users.Where(m => users.Contains(m.Id)).ToList();
        }

        /// <summary>
        /// Returns users in a role by name.  Async Method, use this if you have a very large userbase.
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="roleManager"></param>
        /// <returns>List of ApplicationUser</returns>
        public Task<List<ApplicationUser>> UsersInRoleAsync(string roleName, ApplicationRoleManager roleManager) {
            if (roleManager == null) {
                throw new ArgumentNullException("roleManager", "User Manager cannot be null");
            }
            var thisRole = roleManager.Roles.Include(i => i.Users).FirstOrDefault(m => m.Name == roleName);
            if (thisRole == null) {
                throw new Exception("Role provided does not exist in current database");
            }
            var users = thisRole.Users.Select(i => i.UserId).ToList();
            return Users.Where(m => users.Contains(m.Id)).ToListAsync();
        }
    }
}
