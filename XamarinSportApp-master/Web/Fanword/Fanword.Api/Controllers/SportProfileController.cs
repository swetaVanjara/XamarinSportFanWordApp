using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.SportProfile;
using Fanword.Business.Builders.Mobile.TeamProfile;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/SportProfile")]
    public class SportProfileController : BaseApiController
    {
        public async Task<IHttpActionResult> Get(string sportId)
        {
            var posts = await new SportProfileBuilder(_repo).BuildAsync(ClaimsPrincipal.Current.Identity.GetUserId(), sportId);
            return Ok(posts);
        }
    }
}
