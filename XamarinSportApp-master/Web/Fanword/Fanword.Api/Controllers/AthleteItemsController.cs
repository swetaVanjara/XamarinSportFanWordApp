using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.AthleteItem;
using Fanword.Business.Builders.Mobile.TeamProfile;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/AthleteItems")]
    public class AthleteItemsController : BaseApiController
    {
        [Route("GetBySport")]
        public async Task<IHttpActionResult> GetBySport(string sportId)
        {
            var posts = await new AthleteItemBuilder(_repo).BuildBySportAsync(ClaimsPrincipal.Current.Identity.GetUserId(), sportId);
            return Ok(posts);
        }

        [Route("GetBySchool")]
        public async Task<IHttpActionResult> GetBySchool(string schoolId)
        {
            var posts = await new AthleteItemBuilder(_repo).BuildBySchoolAsync(ClaimsPrincipal.Current.Identity.GetUserId(), schoolId);
            return Ok(posts);
        }

        [Route("GetByTeam")]
        public async Task<IHttpActionResult> GetByTeam(string teamId)
        {
            var posts = await new AthleteItemBuilder(_repo).BuildByTeamAsync(ClaimsPrincipal.Current.Identity.GetUserId(), teamId);
            return Ok(posts);
        }
    }
}
