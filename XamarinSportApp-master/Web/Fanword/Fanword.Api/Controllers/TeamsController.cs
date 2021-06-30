using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Api.Models;
using Fanword.Business.Builders.Mobile.Teams;
using Fanword.Data.IdentityConfig.User;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/Teams")]
    public class TeamsController : BaseApiController
    {
        [Route("AthleteTeams")]
        public async Task<IHttpActionResult> GetAthleteTeams(string search)
        {
            var teamsSearch = await new TeamsBuilder(_repo).TeamsForAthleteSearchAsync(search);
            return Ok(teamsSearch);
        }
    }
}
