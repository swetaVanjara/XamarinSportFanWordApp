using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.RssFeeds;
using Fanword.Business.Hangfire.RssFeeds;
using Fanword.Business.ViewModels.RssFeeds;
using Hangfire;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using Fanword.Data.Entities;
using System.Security.Claims;
using Fanword.Data.Enums;

namespace Fanword.Web.Controllers.Api {
    [Authorize(Roles = "System_Admin, ContentSource, TeamAdmin, SchoolAdmin"), RoutePrefix("api/RssFeeds")]
    public class RssFeedsController : BaseApiController {
        [Route("Grid"), HttpGet]
        public async Task<IHttpActionResult> Grid(bool showInactive, bool showPending) {
            bool isAdmin = false;
            string createdById = User.Identity.GetUserId();
            if (User.IsInRole("System_Admin")) {
                isAdmin = true;
            }
            return Ok(await _rssFeedBuilder.BuildGridAsync(showInactive, createdById, isAdmin, showPending));
        }
        [Route("SingleGrid/{contentSourceId}"), HttpGet]
        public async Task<IHttpActionResult> SingleGrid(string contentSourceId) {
            return Ok(await _rssFeedBuilder.BuildSingleGridAsync(contentSourceId));
        }

        [Route("MappingOptions"), HttpGet]
        public IHttpActionResult MappingOptions(string url) {
            return Ok(new RssFeedXmlGenerator(url).MappingOptions());
        }

        [Route("{id}/Sync"), HttpGet]
        public IHttpActionResult Sync(string id) {
            //sync by id
            BackgroundJob.Enqueue(() => new RssFeedWorker().SyncRssFeed(id));
            return Ok();
        }

        public async Task<IHttpActionResult> Get(string id) {
            return Ok(await _rssFeedBuilder.BuildSingleAsync(id));
        }
        public async Task<IHttpActionResult> Put(RssFeedViewModel model) {

            if (String.IsNullOrEmpty(model.ContentSourceId) && !User.IsInRole(AppRoles.SystemAdmin)) {
                if (String.IsNullOrEmpty(model.TeamId)) {
                    ModelState.AddModelError("model.TeamId", "Team is Required");
                }
                if (String.IsNullOrEmpty(model.SchoolId)) {
                    ModelState.AddModelError("model.SchoolId", "School is Required");
                }
                if (String.IsNullOrEmpty(model.SportId)) {
                    ModelState.AddModelError("model.SportId", "Sport is Required");
                }

                if (!String.IsNullOrEmpty(model.TeamId)) {
                    ModelState.Remove("model.SchoolId");
                    ModelState.Remove("model.SportId");
                }
                if (!String.IsNullOrEmpty(model.SchoolId)) {
                    ModelState.Remove("model.TeamId");
                    ModelState.Remove("model.SportId");
                }
                if (!String.IsNullOrEmpty(model.SportId)) {
                    ModelState.Remove("model.SchoolId");
                    ModelState.Remove("model.TeamId");
                }
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _rssFeedBuilder.UpdateAsync(model);
            return Ok();
        }

        public async Task<IHttpActionResult> Post(RssFeedViewModel model) {
            if (User.IsInRole("System_Admin")) {
                if (String.IsNullOrEmpty(model.TeamId)) {
                    ModelState.AddModelError("model.TeamId", "Team is Required");
                }
                if (String.IsNullOrEmpty(model.SchoolId)) {
                    ModelState.AddModelError("model.SchoolId", "School is Required");
                }
                if (String.IsNullOrEmpty(model.SportId)) {
                    ModelState.AddModelError("model.SportId", "Sport is Required");
                }

                if (!String.IsNullOrEmpty(model.TeamId) || ClaimsPrincipal.Current.IsInRole("ContentSource")) {
                    ModelState.Remove("model.SchoolId");
                    ModelState.Remove("model.SportId");
                }
                if (!String.IsNullOrEmpty(model.SchoolId) || ClaimsPrincipal.Current.IsInRole("ContentSource")) {
                    ModelState.Remove("model.TeamId");
                    ModelState.Remove("model.SportId");
                }
                if (!String.IsNullOrEmpty(model.SportId) || ClaimsPrincipal.Current.IsInRole("ContentSource")) {
                    ModelState.Remove("model.SchoolId");
                    ModelState.Remove("model.TeamId");
                }
            }

            var userId = User.Identity.GetUserId();

            if (User.IsInRole("TeamAdmin")) {
                var teamId = _repo.TeamAdmins.Where(m => m.UserId == userId).FirstOrDefault().TeamId;
                model.TeamId = teamId;
            }
            if (User.IsInRole("SchoolAdmin")) {
                var schoolId = _repo.SchoolAdmins.Where(m => m.UserId == userId).FirstOrDefault().SchoolId;
                model.SchoolId = schoolId;
            }
            //try
            //{
            //	var currentUser = _repo.Users.Where(m => m.Id == userId).Include("ContentSource").FirstOrDefault();
            //	var name = currentUser.ContentSource.ContentSourceName;
            //	model.CreatedBy = name;

            //}
            //catch (Exception ex)
            //{

            //	throw;
            //}


            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _rssFeedBuilder.AddAsync(model, userId);
            return Ok();
        }

        public async Task<IHttpActionResult> Delete(RssFeedViewModel model) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _rssFeedBuilder.UpdateAsync(model);
            return Ok();
        }

        [Route("{id}/EnableDisable"), HttpGet]
        public async Task<IHttpActionResult> EnableDisable(string id) {
            var feedItem = await _repo.RssFeeds.GetByIdAsync(id);
            if (feedItem == null)
                return Ok();
            feedItem.IsActive = !feedItem.IsActive;
            await _repo.SaveAsync();
            return Ok();
        }

		[Route("PendingCount"), HttpGet]
		public async Task<IHttpActionResult> PendingCount()
		{
			return Ok(await _repo.RssFeeds.Where(m => m.RssFeedStatus == RssFeedStatus.Pending && m.DateDeletedUtc == null && (m.Team.DateDeletedUtc == null && m.Sport.DateDeletedUtc == null && m.School.DateDeletedUtc == null)).CountAsync());
		}
	}
}
