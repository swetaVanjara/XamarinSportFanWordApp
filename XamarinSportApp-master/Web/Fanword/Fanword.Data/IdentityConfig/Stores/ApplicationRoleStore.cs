using System;
using Fanword.Data.IdentityConfig.User; 
using Microsoft.AspNet.Identity.EntityFramework;
using Fanword.Data.IdentityConfig.Roles;
using Fanword.Data.Context;

namespace Fanword.Data.IdentityConfig.Stores {
   public class ApplicationRoleStore : RoleStore<ApplicationRole,string,IdentityUserRole> {


        public ApplicationRoleStore(ApplicationDbContext ctx) : base(ctx) {

        }
    }
}
