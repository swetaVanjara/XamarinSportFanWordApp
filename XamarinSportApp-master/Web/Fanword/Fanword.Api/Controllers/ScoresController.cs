using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.Scores;
using Fanword.Poco.Models;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/Scores")]
    public class ScoresController : BaseApiController
    {
        public async Task<IHttpActionResult> Post(ScoresFilterModel filter)
        {
            var scores = await new ScoresBuilder(_repo).BuildAsync(User.Identity.GetUserId(), filter);
            return Ok(scores);
        }
    }
}
