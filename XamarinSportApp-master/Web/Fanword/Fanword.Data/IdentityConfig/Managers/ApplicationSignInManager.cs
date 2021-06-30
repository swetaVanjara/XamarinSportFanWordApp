using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Fanword.Data.IdentityConfig.User;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
namespace Fanword.Data.IdentityConfig.Managers {
    public class ApplicationSignInManager :SignInManager<ApplicationUser,string>{
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager){}

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user) {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager CreateOwin(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext ctx) {
            return new ApplicationSignInManager(ctx.Get<ApplicationUserManager>(), ctx.Authentication);
        }
		public override Task SignInAsync(ApplicationUser user, bool isPersistent, bool rememberBrowser) {
            var dbUser = UserManager.FindById(user.Id);
            dbUser.LastLoginDateUtc = DateTime.UtcNow;
            UserManager.Update(dbUser);
            return base.SignInAsync(user, isPersistent, rememberBrowser);
        }

        public override async Task<Microsoft.AspNet.Identity.Owin.SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout) {
            var isValid = await base.PasswordSignInAsync(userName, password, isPersistent, shouldLockout);
            if (isValid != Microsoft.AspNet.Identity.Owin.SignInStatus.Success) return isValid;
            var dbUser = await UserManager.FindByNameAsync(userName);
            dbUser.LastLoginDateUtc = DateTime.UtcNow;
            await UserManager.UpdateAsync(dbUser);
            return isValid;
        }
    }
}
