using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.ViewModels.NewsNotifications;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using Fanword.Data.Enums;

namespace Fanword.Web.Controllers.Api {
    [Authorize(Roles = "System_Admin, ContentSource, TeamAdmin, SchoolAdmin"), RoutePrefix("api/NewsNotifications")]
    public class NewsNotificationsController : BaseApiController {

        [Route("Grid"),HttpGet]
        public async Task<IHttpActionResult> Grid() {
            return Ok(await _newsNotificationBuilder.BuildGridAsync());
        }

		public async Task<IHttpActionResult> Get(string id)
		{
			return Ok(await _newsNotificationBuilder.BuildSingleAsync(id));
		}
		[HttpPost]
        public async Task<IHttpActionResult> Post(NewsNotificationViewModel model) {
            ModelState.AddModelError("model.TeamId", "Team is Required");
            ModelState.AddModelError("model.SchoolId", "School is Required");
            ModelState.AddModelError("model.SportId", "Sport is Required");
            if (!String.IsNullOrEmpty(model.SportId) || !String.IsNullOrEmpty(model.TeamId) || !String.IsNullOrEmpty(model.SchoolId) || User.IsInRole("ContentSource")) {
                ModelState.Remove("model.SportId");
                ModelState.Remove("model.TeamId");
                ModelState.Remove("model.SchoolId");
            }

		    if (User.IsInRole("TeamAdmin") && User.IsInRole("SchoolAdmin") && String.IsNullOrEmpty(model.SchoolId) && String.IsNullOrEmpty(model.TeamId)) {
		        ModelState.Remove("model.TeamId");
		        ModelState.Remove("model.SportId");
                ModelState.Remove("model.SchoolId");
		        ModelState.AddModelError("model.TeamId", "Either Team or School is required");
		        ModelState.AddModelError("model.SchoolId", "Either Team or School is required");
            }

			if (User.IsInRole("TeamAdmin") && !User.IsInRole("SchoolAdmin"))
			{
				ModelState.Remove("model.SportId");
				ModelState.Remove("model.SchoolId");
			}
			if (User.IsInRole("SchoolAdmin") && !User.IsInRole("TeamAdmin"))
			{
				ModelState.Remove("model.TeamId");
				ModelState.Remove("model.SportId");
			}
			var userId = User.Identity.GetUserId();

			model.ContentSourceId = _repo.Users.Where(m => m.Id == userId).FirstOrDefault()?.ContentSourceId ?? null;
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _newsNotificationBuilder.AddAsync(model);
            
            return Ok();
        }

		[HttpPut]
		public async Task<IHttpActionResult> Put(NewsNotificationViewModel model)
		{
			ModelState.AddModelError("model.TeamId", "Team is Required");
			ModelState.AddModelError("model.SchoolId", "School is Required");
			ModelState.AddModelError("model.SportId", "Sport is Required");
			if (!String.IsNullOrEmpty(model.SportId) || !String.IsNullOrEmpty(model.TeamId) || !String.IsNullOrEmpty(model.SchoolId) || User.IsInRole("ContentSource"))
			{
				ModelState.Remove("model.SportId");
				ModelState.Remove("model.TeamId");
				ModelState.Remove("model.SchoolId");
			}
		    if (User.IsInRole("TeamAdmin") && User.IsInRole("SchoolAdmin") && String.IsNullOrEmpty(model.SchoolId) && String.IsNullOrEmpty(model.TeamId)) {
		        ModelState.Remove("model.TeamId");
		        ModelState.Remove("model.SportId");
		        ModelState.Remove("model.SchoolId");
		        ModelState.AddModelError("model.TeamId", "Either Team or School is required");
		        ModelState.AddModelError("model.SchoolId", "Either Team or School is required");
		    }

		    if (User.IsInRole("TeamAdmin") && !User.IsInRole("SchoolAdmin")) {
		        ModelState.Remove("model.SportId");
		        ModelState.Remove("model.SchoolId");
		    }
		    if (User.IsInRole("SchoolAdmin") && !User.IsInRole("TeamAdmin")) {
		        ModelState.Remove("model.TeamId");
		        ModelState.Remove("model.SportId");
		    }
			if (User.IsInRole("System_Admin"))
			{
				ModelState.Remove("model.SportId");
				ModelState.Remove("model.SchoolId");
				ModelState.Remove("model.TeamId");
			}
            if (!ModelState.IsValid) return BadRequest(ModelState);
			await _newsNotificationBuilder.UpdateAsync(model);
			return Ok();
		}

        public async Task<IHttpActionResult> Delete(string id) {
            await _repo.NewsNotifications.DeleteAndSaveAsync(id);
            return Ok();
        }

		[Route("PendingCount"), HttpGet]
		public async Task<IHttpActionResult> PendingCount()
		{
			return Ok(await _repo.NewsNotifications.Where(m => m.NewsNotificationStatus == NewsNotificationStatus.Pending).CountAsync());
		}
	}
}
