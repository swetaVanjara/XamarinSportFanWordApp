using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.ViewModels.Schools;
using Fanword.Business.ViewModels.Teams;
using Fanword.Data.Enums;
using Microsoft.AspNet.Identity;

namespace Fanword.Web.Controllers.Api {
    [Authorize, RoutePrefix("api/Teams")]
    public class TeamsController : BaseApiController {
        [Route("Grid"), HttpGet]
        public async Task<IHttpActionResult> Grid(bool showInactive) {
            return Ok(await _teamsBuilder.BuildGridAsync(showInactive));
        }

        [Route("SelectControlList"), HttpGet]
        public async Task<IHttpActionResult> SelectList() {
            var query = _repo.Teams.Where(m => m.School.DateDeletedUtc == null && m.Sport.DateDeletedUtc == null);
            //if (!User.IsInRole(AppRoles.SystemAdmin)) {
            //    var userId = User.Identity.GetUserId();
            //    query = query.Where(m=>m.Admins.Any(i=>i.AdminStatus==AdminStatus.Approved && i.UserId== userId));
                    
            //}
            return Ok(await query.Select(i => new { DisplayName = i.School.Name + " - " + i.Sport.Name, i.Id, i.SportId }).OrderBy(i => i.DisplayName).ToListAsync());
        }

        [Route("SelectTeamsBySport"), HttpGet]
        public async Task<IHttpActionResult> SelectListBySport(string id = "")
        {
            var query = _repo.Teams.Where(m => m.School.DateDeletedUtc == null && m.Sport.DateDeletedUtc == null && m.SportId == id );
            //if (!User.IsInRole(AppRoles.SystemAdmin)) {
            //    var userId = User.Identity.GetUserId();
            //    query = query.Where(m=>m.Admins.Any(i=>i.AdminStatus==AdminStatus.Approved && i.UserId== userId));

            //}
            return Ok(await query.Select(i => new { DisplayName = i.School.Name + " - " + i.Sport.Name, i.Id, i.SportId }).ToListAsync());
        }

        [Route("SportsNotAssignedToTeam"), HttpGet]
        public async Task<IHttpActionResult> SportsNotAssignedToTeams(string schoolId) {
            var allSports = await _repo.Sports.AsQueryable().Select(i => i.Id).ToListAsync();
            var unAssignedSports = await _repo.Sports.Where(m => m.Teams.Any(i => i.SchoolId == schoolId && i.DateDeletedUtc == null)).Select(i => i.Id).ToListAsync();
            var leftover = allSports.Except(unAssignedSports);
            return Ok(await _repo.Sports.Where(m => leftover.Contains(m.Id)).Select(i => new { DisplayName = i.Name, i.Id }).ToListAsync());
        }

        [Route("AddFromSportAndSchool"), HttpPost]
        public async Task<IHttpActionResult> AddFromSportAndSchool(AddFromSportAndSchoolViewModel model) {
            await _teamsBuilder.AddFromSportAndSchoolAsync(model);
            return Ok();
        }

        public async Task<IHttpActionResult> Get(string id) {
            return Ok(await _teamsBuilder.BuildSingleAsync(id));
        }

        public async Task<IHttpActionResult> Put(TeamViewModel model) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _teamsBuilder.UpdateAsync(model);
            return Ok();
        }

        public async Task<IHttpActionResult> Post(TeamViewModel model) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _teamsBuilder.AddAsync(model);
            return Ok();
        }

        public async Task<IHttpActionResult> Delete(string id) {

			await _teamsBuilder.DeleteAsync(id);

			return Ok();
        }
    }
}
