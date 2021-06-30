using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.ContentSources;
using Fanword.Business.Builders.Mobile.ContentSoureProfiles;
using Fanword.Business.Builders.Mobile.UserProfiles;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/ContentSourceProfile")]
    public class ContentSourceProfileController : BaseApiController
    {
        public async Task<IHttpActionResult> Get(string contentSourceId)
        {
            var posts = await new ContentSourceProfileBuilder(_repo).BuildAsync(ClaimsPrincipal.Current.Identity.GetUserId(), contentSourceId);
            return Ok(posts);
        }
    }
}
