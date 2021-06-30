using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.SearchProfiles;
using Fanword.Poco.Models;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/ProfileSearch")]
    public class ProfileSearchController : BaseApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> Search(string text, string id, FeedType type)
        {
            var search = await new SearchProfileBuilder(_repo).SearchAsync(text, User.Identity.GetUserId(), id, type);
            return Ok(search);
        }
    }
}
