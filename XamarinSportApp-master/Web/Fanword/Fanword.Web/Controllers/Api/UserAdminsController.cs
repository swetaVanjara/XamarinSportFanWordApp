using Fanword.Business.ViewModels.UserAdmins;
using Fanword.Data.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fanword.Web.Controllers.Api
{
	[Authorize(Roles ="System_Admin, TeamAdmin, SchoolAdmin"), RoutePrefix("api/UserAdmins")]
	public class UserAdminsController : BaseApiController
	{
		[Route("TeamGrid"), HttpGet]
		public async Task<IHttpActionResult> TeamGrid(bool showPending)
		{
			return Ok(await _teamAdminBuilder.BuildGridAsync(showPending));
		}

		[Route("SchoolGrid"), HttpGet]
		public async Task<IHttpActionResult> SchoolGrid(bool showPending)
		{
			return Ok(await _schoolAdminBuilder.BuildGridAsync(showPending));
		}
		[Route("TeamSingle"), HttpGet]
		public async Task<IHttpActionResult> GetTeam(string id)
		{
			return Ok(await _teamAdminBuilder.BuildSingleAsync(id));
		}

		[Route("SchoolSingle"), HttpGet]
		public async Task<IHttpActionResult> GetSchool(string id)
		{
			return Ok(await _schoolAdminBuilder.BuildSingleAsync(id));
		}

		[Route("GetCurrentTeam"), HttpGet]
		public async Task<IHttpActionResult> GetCurrentTeam(string id)
		{
			var teamId = _repo.TeamAdmins.Where(m => m.UserId == id).Select(i => i.Id).FirstOrDefault();
			return Ok(await _teamAdminBuilder.BuildSingleAsync(teamId));
		}
		[Route("GetCurrentSchool"), HttpGet]
		public async Task<IHttpActionResult> GetCurrentSchool(string id)
		{
			var schoolId = _repo.SchoolAdmins.Where(m => m.UserId == id).Select(i => i.Id).FirstOrDefault();
			return Ok(await _schoolAdminBuilder.BuildSingleAsync(schoolId));
		}
		[Authorize(Roles = "System_Admin"),Route("NewTeam"), HttpPost]
		public async Task<IHttpActionResult> NewTeam(TeamAdminViewModel model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			await _teamAdminBuilder.AddAsync(model);
			return Ok();
		}
		[Authorize(Roles = "System_Admin"),Route("NewSchool"), HttpPost]
		public async Task<IHttpActionResult> NewSchool(SchoolAdminViewModel model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			await _schoolAdminBuilder.AddAsync(model);
			return Ok();
		}
		[Authorize(Roles = "System_Admin"), Route("Team"), HttpPut]
		public async Task<IHttpActionResult> Team(TeamAdminViewModel model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			await _teamAdminBuilder.UpdateAsync(model);
			return Ok();
		}

		[Authorize(Roles = "System_Admin"), Route("School"), HttpPut]
		public async Task<IHttpActionResult> School(SchoolAdminViewModel model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			await _schoolAdminBuilder.UpdateAsync(model);
			return Ok();
		}

		[Route("CountSchool"), HttpGet]
		public async Task<IHttpActionResult> CountSchool()
		{
			return Ok(await _repo.SchoolAdmins.Where(m => m.AdminStatus == AdminStatus.Approved).CountAsync());
		}

		[Route("CountTeam"), HttpGet]
		public async Task<IHttpActionResult> CountTeam()
		{
			return Ok(await _repo.TeamAdmins.Where(m => m.AdminStatus == AdminStatus.Approved).CountAsync());
		}

		[Route("PendingCount"), HttpGet]
		public async Task<IHttpActionResult> PendingCount()
		{
			var schoolCount = await _repo.SchoolAdmins.Where(m => m.AdminStatus == AdminStatus.Pending).CountAsync();
			var teamCount = await _repo.TeamAdmins.Where(m => m.AdminStatus == AdminStatus.Pending).CountAsync();
			return Ok(schoolCount + teamCount);
		}

		//[Route("DeleteTeam")]
		//public async Task<IHttpActionResult> DeleteTeam(string id)
		//{
		//	await _teamAdminBuilder.DeleteAsync(id);
		//	return Ok();
		//}
		//[Route("DeleteSchool")]
		//public async Task<IHttpActionResult> DeleteSchool(string id)
		//{
		//	await _schoolAdminBuilder.DeleteAsync(id);
		//	return Ok();
		//}

	}
}
