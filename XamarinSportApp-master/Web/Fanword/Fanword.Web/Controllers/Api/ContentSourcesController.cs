using Fanword.Business.ViewModels.ContentSources;
using Microsoft.AspNet.Identity;
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
	[Authorize(Roles = "System_Admin, ContentSource"), RoutePrefix("api/ContentSources")]
	public class ContentSourcesController : BaseApiController
    {
		[Route("Grid"), HttpGet]
		public async Task<IHttpActionResult> Grid()
		{
			return Ok(await _contentSourceBuilder.BuildGridAsync());
		}
		[Route("SelectControlList"), HttpGet]
		public async Task<IHttpActionResult> SelectList()
		{
			return Ok(await _repo.ContentSources.AsQueryable().ToListAsync());
		}
		[Route("GetById"), HttpGet]
		public async Task<IHttpActionResult> GetById(string id)
		{
			return Ok(await _contentSourceBuilder.BuildSingleAdminAsync(id));
		}
		public async Task<IHttpActionResult> Get()
		{
			string id = User.Identity.GetUserId();
			return Ok(await _contentSourceBuilder.BuildSingleAsync(id));
		}

		public async Task<IHttpActionResult> Put(ContentSourceViewModel model)
		{
			await _contentSourceBuilder.UpdateAsync(model);
            if (model.IsApproved) {
                var userIds = _repo.UserManager.Users.Where(m => m.ContentSourceId == model.Id).Select(i => i.Id).ToList();
                foreach (var userId in userIds) {
                    await _repo.UserManager.AddToRoleAsync(userId, "ContentSource");
                }
            }
            else {
                var userIds = _repo.UserManager.Users.Where(m => m.ContentSourceId == model.Id).Select(i => i.Id).ToList();
                foreach (var userId in userIds) {
                    await _repo.UserManager.RemoveFromRoleAsync(userId, "ContentSource");
                }
            }
            return Ok();
		}

		[Route("Count"), HttpGet]
		public async Task<IHttpActionResult> Count()
		{
			return Ok(await _repo.ContentSources.Where(m => m.IsApproved == true).CountAsync());
		}

		[Route("PendingCount"), HttpGet]
		public async Task<IHttpActionResult> PendingCount()
		{
			return Ok(await _repo.ContentSources.Where(m => m.IsApproved == false).CountAsync());
		}
	}
}
