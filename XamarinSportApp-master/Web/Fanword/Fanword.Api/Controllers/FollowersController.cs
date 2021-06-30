using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.Feed;
using Fanword.Business.Builders.Mobile.Followers;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/Followers")]
    public class FollowersController : BaseApiController
    {
        [HttpGet, Route("User")]
        public async Task<IHttpActionResult> User(string userId)
        {
            var followers = await new FollowersBuilder(_repo).BuildForUserAsync(ClaimsPrincipal.Current.Identity.GetUserId(), userId);
            return Ok(followers);
        }

        [HttpGet, Route("Team")]
        public async Task<IHttpActionResult> Team(string teamId)
        {
            var followers = await new FollowersBuilder(_repo).BuildForTeamAsync(ClaimsPrincipal.Current.Identity.GetUserId(), teamId);
            return Ok(followers);
        }

        [HttpGet, Route("School")]
        public async Task<IHttpActionResult> School(string schoolId)
        {
            var followers = await new FollowersBuilder(_repo).BuildForSchoolAsync(ClaimsPrincipal.Current.Identity.GetUserId(), schoolId);
            return Ok(followers);
        }

        [HttpGet, Route("Sport")]
        public async Task<IHttpActionResult> Sport(string sportId)
        {
            var followers = await new FollowersBuilder(_repo).BuildForSportAsync(ClaimsPrincipal.Current.Identity.GetUserId(), sportId);
            return Ok(followers);
        }

        [HttpGet, Route("ContentSource")]
        public async Task<IHttpActionResult> ContentSource(string contentSourceId)
        {
            var followers = await new FollowersBuilder(_repo).BuildForContentSourceAsync(ClaimsPrincipal.Current.Identity.GetUserId(), contentSourceId);
            return Ok(followers);
        }
    }
}
