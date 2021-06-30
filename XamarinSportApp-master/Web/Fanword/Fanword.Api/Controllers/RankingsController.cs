using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.Rankings;
using Fanword.Poco.Models;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/Rankings")]
    public class RankingsController : BaseApiController
    {
        public async Task<IHttpActionResult> Post(FollowingFilterModel filter)
        {
            var rankings = await new RankingsBuilder(_repo).BuildAsync(User.Identity.GetUserId(), filter);
            return Ok(rankings);
        }
    }
}
