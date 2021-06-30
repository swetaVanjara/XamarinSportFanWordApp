using System;
using Fanword.Data.IdentityConfig.Roles;
using Fanword.Data.IdentityConfig.User;
using Microsoft.AspNet.Identity.EntityFramework;
using Fanword.Data.Context;

namespace Fanword.Data.IdentityConfig.Stores {
    public  class ApplicationUserStore : UserStore<ApplicationUser,ApplicationRole,string,IdentityUserLogin,IdentityUserRole,IdentityUserClaim>{
        public ApplicationUserStore(ApplicationDbContext ctx) : base(ctx) {}
    }
}
