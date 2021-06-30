using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.MyProfileDetails;
using Fanword.Business.Builders.Mobile.UserProfiles;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/UserProfile")]

    public class UserProfileController : BaseApiController
    {
        public async Task<IHttpActionResult> Get(string userId)
        {
            var posts = await new UserProfileBuilder(_repo).BuildAsync(ClaimsPrincipal.Current.Identity.GetUserId(), userId);
            return Ok(posts);
        }
    }
}
