using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.ViewModels.Advertisers;
using System.Data.Entity;

namespace Fanword.Web.Controllers.Api
{
    [Authorize(Roles = "System_Admin,DataAdv"),RoutePrefix("api/Advertisers")]
    public class AdvertisersController : BaseApiController
    {
        [Route("Grid"), HttpGet]
        public async Task<IHttpActionResult> Grid() {
            return Ok(await _advertiserBuilder.BuildGridAsync());
        }

        public async Task<IHttpActionResult> Get(string id) {
            return Ok(await _advertiserBuilder.BuildSingleAsync(id));
        }

        public async Task<IHttpActionResult> Put(AdvertiserViewModel model) {
            await _advertiserBuilder.UpdateAsync(model);
            return Ok();
        }

		[Route("Count"), HttpGet]
		public async Task<IHttpActionResult> Count()
		{
			return Ok(await _repo.Advertisers.Where(m => m.Id != null).CountAsync());
		}
    }
}
