using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.Rankings;
using Fanword.Business.Builders.Mobile.TeamRankings;
using Fanword.Poco.Models;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/TeamRankings")]
    public class TeamRankingsController : BaseApiController
    {
        public async Task<IHttpActionResult> Get(string teamId)
        {
            var rankings = await new TeamRankingsBuilder(_repo).BuildAsync(User.Identity.GetUserId(), teamId);
            return Ok(rankings);
        }
    }
}
