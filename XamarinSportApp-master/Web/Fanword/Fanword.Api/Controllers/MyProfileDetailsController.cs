using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.Feed;
using Fanword.Business.Builders.Mobile.MyProfileDetails;
using Fanword.Poco.Models;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/MyProfileDetails")]
    public class MyProfileDetailsController : BaseApiController
    {
        public async Task<IHttpActionResult> Get()
        {
            var posts = await new MyProfileDetailsBuilder(_repo).BuildAsync(ClaimsPrincipal.Current.Identity.GetUserId());
            return Ok(posts);
        }
    }
}
