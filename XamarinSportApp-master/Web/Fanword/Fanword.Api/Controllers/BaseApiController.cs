using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fanword.Data.IdentityConfig.Managers;
using Fanword.Data.Repository;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Fanword.Api.Controllers
{
    public class BaseApiController : ApiController {
        protected ApplicationRepository _repo => Request.GetOwinContext().Get<ApplicationRepository>();
        protected ApplicationUserManager UserManager => _repo.UserManager;
        protected IAuthenticationManager Authentication => Request.GetOwinContext().Authentication;

    }
}
