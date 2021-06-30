using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.EventProfile;
using Fanword.Business.Builders.Mobile.EventTeams;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/EventTeams")]
    public class EventTeamsController : BaseApiController
    {
        public async Task<IHttpActionResult> Get(string eventId)
        {
            var teams = await new EventTeamsBuilder(_repo).BuildAsync(eventId);
            return Ok(teams);
        }
    }
}
