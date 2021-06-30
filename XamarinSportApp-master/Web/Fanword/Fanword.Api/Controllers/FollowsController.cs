using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.Follows;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/Follows")]
    public class FollowsController : BaseApiController
    {
        [HttpGet, Route("FollowTeam")]
        public async Task<IHttpActionResult> FollowTeam(string teamId)
        {
            await new FollowsBuilder(_repo).FollowTeamAsync(User.Identity.GetUserId(), teamId);
            return Ok();
        }

        [HttpGet, Route("UnfollowTeam")]
        public async Task<IHttpActionResult> UnfollowTeam(string teamId)
        {
            await new FollowsBuilder(_repo).UnfollowTeamAsync(User.Identity.GetUserId(), teamId);
            return Ok();
        }

        [HttpGet, Route("FollowUser")]
        public async Task<IHttpActionResult> FollowUser(string userId)
        {
            await new FollowsBuilder(_repo).FollowUserAsync(User.Identity.GetUserId(), userId);
            return Ok();
        }

        [HttpGet, Route("UnfollowUser")]
        public async Task<IHttpActionResult> UnfollowUser(string userId)
        {
            await new FollowsBuilder(_repo).UnfollowUserAsync(User.Identity.GetUserId(), userId);
            return Ok();
        }

        [HttpGet, Route("FollowContentSource")]
        public async Task<IHttpActionResult> FollowContentSource(string contentSourceId)
        {
            await new FollowsBuilder(_repo).FollowContentSourceAsync(User.Identity.GetUserId(), contentSourceId);
            return Ok();
        }

        [HttpGet, Route("UnfollowContentSource")]
        public async Task<IHttpActionResult> UnfollowContentSource(string contentSourceId)
        {
            await new FollowsBuilder(_repo).UnfollowContentSourceAsync(User.Identity.GetUserId(), contentSourceId);
            return Ok();
        }

        [HttpGet, Route("FollowSport")]
        public async Task<IHttpActionResult> FollowSport(string sportId)
        {
            await new FollowsBuilder(_repo).FollowSportAsync(User.Identity.GetUserId(), sportId);
            return Ok();
        }

        [HttpGet, Route("UnfollowSport")]
        public async Task<IHttpActionResult> UnfollowSport(string sportId)
        {
            await new FollowsBuilder(_repo).UnfollowSportAsync(User.Identity.GetUserId(), sportId);
            return Ok();
        }

        [HttpGet, Route("FollowSchool")]
        public async Task<IHttpActionResult> FollowSchool(string schoolId)
        {
            await new FollowsBuilder(_repo).FollowSchoolAsync(User.Identity.GetUserId(), schoolId);
            return Ok();
        }

        [HttpGet, Route("UnfollowSchool")]
        public async Task<IHttpActionResult> UnfollowSchool(string schoolId)
        {
            await new FollowsBuilder(_repo).UnfollowSchoolAsync(User.Identity.GetUserId(), schoolId);
            return Ok();
        }
    }
}
