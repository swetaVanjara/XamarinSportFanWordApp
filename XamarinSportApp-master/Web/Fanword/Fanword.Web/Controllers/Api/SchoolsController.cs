using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.ViewModels.Schools;
using Fanword.Business.ViewModels.Sports;
using Fanword.Data.Enums;
using Microsoft.AspNet.Identity;

namespace Fanword.Web.Controllers.Api
{
    [Authorize,RoutePrefix("api/Schools")]
    public class SchoolsController : BaseApiController
    {
        [Route("Grid"), HttpGet]
        public async Task<IHttpActionResult> Grid(bool showInactive) {
            return Ok(await _schoolsBuilder.BuildGridAsync(showInactive));
        }

        [Route("SelectControlList"), HttpGet]
        public async Task<IHttpActionResult> SelectControlList() {
            var query = _repo.Schools.AsQueryable();
            //if (!User.IsInRole(AppRoles.SystemAdmin)) {
            //    var userId = User.Identity.GetUserId();
            //    query = query.Where(m => m.Admins.Any(i => i.UserId == userId && i.AdminStatus==AdminStatus.Approved));
            //}
            return Ok(await query.Select(i => new { DisplayName = i.Name, i.Id }).OrderBy(i => i.DisplayName).ToListAsync());
        }

        [Route("Validate"), HttpPost]
        public IHttpActionResult Validate(SchoolViewModel model) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok();
        }

        public async Task<IHttpActionResult> Get(string id) {
            return Ok(await _schoolsBuilder.BuildSingleAsync(id));
        }

        public async Task<IHttpActionResult> Put(SchoolViewModel model) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _schoolsBuilder.UpdateAsync(model);
            return Ok();
        }

        public async Task<IHttpActionResult> Post(SchoolViewModel model) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            return Ok(await _schoolsBuilder.AddAsync(model));
        }

        public async Task<IHttpActionResult> Delete(string id) {
			await _schoolsBuilder.DeleteAsync(id);
			return Ok();
        }
    }
}
