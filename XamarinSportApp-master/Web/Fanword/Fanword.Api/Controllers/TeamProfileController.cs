using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.ContentSoureProfiles;
using Fanword.Business.Builders.Mobile.TeamProfile;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/TeamProfile")]
    public class TeamProfileController : BaseApiController
    {
        public async Task<IHttpActionResult> Get(string teamId)
        {
            var posts = await new TeamProfileBuilder(_repo).BuildAsync(ClaimsPrincipal.Current.Identity.GetUserId(), teamId);
            return Ok(posts);
        }

        [Route("GetBySport")]
        public async Task<IHttpActionResult> GetBySport(string sportId)
        {
            var posts = await new TeamProfileBuilder(_repo).BuildBySportAsync(ClaimsPrincipal.Current.Identity.GetUserId(), sportId);
            return Ok(posts);
        }

        [Route("GetBySchool")]
        public async Task<IHttpActionResult> GetBySchool(string schoolId)
        {
            var posts = await new TeamProfileBuilder(_repo).BuildBySchoolAsync(ClaimsPrincipal.Current.Identity.GetUserId(), schoolId);
            return Ok(posts);
        }

        [HttpGet, Route("RequestAdmin")]
        public async Task<IHttpActionResult> RequestAdmin(string teamId)
        {
            await new TeamProfileBuilder(_repo).RequestAdminAsync(ClaimsPrincipal.Current.Identity.GetUserId(), teamId);
            return Ok();
        }
    }
}
