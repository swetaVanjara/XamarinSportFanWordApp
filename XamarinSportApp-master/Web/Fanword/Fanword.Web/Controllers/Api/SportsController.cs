using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.ViewModels.Sports;

namespace Fanword.Web.Controllers.Api {
    [Authorize, RoutePrefix("api/Sports")]
    public class SportsController : BaseApiController {
        [Route("Grid"), HttpGet]
        public async Task<IHttpActionResult> Grid(bool showInactive) {
            return Ok(await _sportsBuilder.BuildGridAsync(showInactive));
        }

        [Route("SelectControlList"), HttpGet]
        public async Task<IHttpActionResult> SelectControlList() {
            return Ok(await _repo.Sports.AsQueryable().Select(i=>new {DisplayName = i.Name, i.Id}).OrderBy(i => i.DisplayName).ToListAsync());
        }

        


        public async Task<IHttpActionResult> Get(string id) {
            return Ok(await _sportsBuilder.BuildSingleAsync(id));
        }

        public async Task<IHttpActionResult> Put(SportViewModel model) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _sportsBuilder.UpdateAsync(model);
            return Ok();
        }

        public async Task<IHttpActionResult> Post(SportViewModel model) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _sportsBuilder.AddAsync(model);
            return Ok();
        }

        public async Task<IHttpActionResult> Delete(string id) {
			await _sportsBuilder.DeleteAsync(id);
            return Ok();
        }
    }
}
