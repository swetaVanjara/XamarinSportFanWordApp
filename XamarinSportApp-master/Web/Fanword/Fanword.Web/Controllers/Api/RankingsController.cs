using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.ViewModels.Rankings;

namespace Fanword.Web.Controllers.Api
{
    [Authorize,RoutePrefix("api/Rankings")]
    public class RankingsController : BaseApiController
    {
        [Route("BySport/{sportId}"),HttpGet]
        public async Task<IHttpActionResult> BySport(string sportId) {
            if (String.IsNullOrEmpty(sportId)) return Ok();
            return Ok(await _rankingsBuilder.BuildSingleAsync(sportId));
        }

        public async Task<IHttpActionResult> Put(RankingViewModel model) {
            if (model.RankingTeams.Select(i => i.TeamId).Where(m=>!String.IsNullOrEmpty(m)).Distinct().Count() != model.RankingTeams
                    .Where(m => !String.IsNullOrEmpty(m.TeamId))
                    .Select(i => i.TeamId)
                    .Count()) {
                ModelState.AddModelError("invalidTeams", "You cannot set the same team more than once in a ranking");
                return BadRequest(ModelState);
            }
            await _rankingsBuilder.UpdateAsync(model);
            return Ok();
        }
    }
}
