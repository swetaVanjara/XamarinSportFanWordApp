using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using Fanword.Data.Context;
using Fanword.Data.IdentityConfig.User;
using Fanword.Data.IdentityConfig.Roles;
using Fanword.Data.IdentityConfig.Stores;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
namespace Fanword.Data.IdentityConfig.Managers {
    public class ApplicationRoleManager : RoleManager<ApplicationRole>{
        public ApplicationRoleManager(ApplicationDbContext ctx) : base(new ApplicationRoleStore(ctx)){}
        /// <summary>
        /// return list of users in a role by name.
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="userManager"></param>
        /// <returns>List Application User</returns>
        public List<ApplicationUser> UsersInRole(string roleName, ApplicationUserManager userManager){
            if (userManager == null){
                throw new ArgumentNullException("userManager", "User Manager cannot be null");
            }
            var thisRole = Roles.Include(i=>i.Users).FirstOrDefault(m => m.Name == roleName);
            if (thisRole == null) {
                throw new ObjectNotFoundException("Role provided does not exist in current database");
            }
            var users = thisRole.Users.Select(i => i.UserId).ToList();
            return userManager.Users.Where(m => users.Contains(m.Id)).ToList();
        }

		public List<ApplicationRole> RolesForUser(string userId, ApplicationUserManager userManager) {
            var user = userManager.FindById(userId);
            var roles = user.Roles.Select(i => i.RoleId).ToList();
            return Roles.Where(m => roles.Contains(m.Id)).ToList();
        }

        public async Task<List<ApplicationRole>> RolesForUserAsync(string userId, ApplicationUserManager userManager) {
            var user = await userManager.FindByIdAsync(userId);
            var roles = user.Roles.Select(i => i.RoleId).ToList();
            return Roles.Where(m => roles.Contains(m.Id)).ToList();
        } 

        /// <summary>
        /// Returns users in a role by name.  Async Method, use this if you have a very large userbase.
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="userManager"></param>
        /// <returns>List of ApplicationUser</returns>
        public Task<List<ApplicationUser>> UsersInRoleAsync(string roleName, ApplicationUserManager userManager) {
            if (userManager == null) {
                throw new ArgumentNullException("userManager", "User Manager cannot be null");
            }
            var thisRole = Roles.FirstOrDefault(m => m.Name == roleName);
            if (thisRole == null){
                throw new ObjectNotFoundException("Role provided does not exist in current database");
            }
            var users = thisRole.Users.Select(i => i.UserId).ToList();
            return userManager.Users.Where(m => users.Contains(m.Id)).ToListAsync();
        }

        public static ApplicationRoleManager CreateOwin(IdentityFactoryOptions<ApplicationRoleManager> options,
            IOwinContext ctx){
            return new ApplicationRoleManager(ctx.Get<ApplicationDbContext>());
        }
    }
}
