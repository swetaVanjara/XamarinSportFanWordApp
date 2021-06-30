using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fanword.Web.Controllers.Api {
    [Authorize, RoutePrefix("api/Facilities")]
    public class FacilitiesController : BaseApiController {
    //    [Route("SelectControlList"), HttpGet]
    //    public async Task<IHttpActionResult> SelectControlList() {
    //        return Ok(await _repo.Facilities.AsQueryable().Select(i => new { DisplayName = i.Name, i.Id }).ToListAsync());
    //    }
    }
}
