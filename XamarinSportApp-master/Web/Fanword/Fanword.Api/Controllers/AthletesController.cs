using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile;
using Fanword.Poco.Models;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/Athlete")]
    public class AthletesController : BaseApiController
    {
        [Route("SaveAthlete")]
        public async Task<IHttpActionResult> SaveAthlete(Athlete athlete)
        {
            return Ok(await new AthletesBuilder(_repo).SaveAthleteAync(athlete, User.Identity.GetUserId()));
        }
    }
}
