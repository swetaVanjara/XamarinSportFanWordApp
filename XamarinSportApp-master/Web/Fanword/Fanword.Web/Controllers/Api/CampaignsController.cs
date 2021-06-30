using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.ViewModels.Campaigns;
using Fanword.Data.Enums;
using System.Data.Entity;

namespace Fanword.Web.Controllers.Api
{
    [Authorize(Roles = "System_Admin, Advertiser,DataAdv"), RoutePrefix("api/Campaigns")]
    public class CampaignsController : BaseApiController
    {
        [Route("Grid"),HttpGet]
        public async Task<IHttpActionResult> Grid() {
            return Ok(await _campaignBuilder.BuildGridAsync());
        }

        [Route("AdminGrid"), HttpGet]
        public async Task<IHttpActionResult> AdminGrid(string advertiserId) {
            return Ok(await _campaignBuilder.BuildGridAsync(advertiserId));
        }


        [Authorize(Roles = "System_Admin"), HttpGet, Route("{id}/Approve")]
        public async Task<IHttpActionResult> Approve(string id) {
            var campaign = await _repo.Campaigns.GetByIdAsync(id);
            campaign.CampaignStatus = CampaignStatus.Approved;
            await _repo.SaveAsync();
            return Ok();
        }

        [Authorize(Roles = "System_Admin"), HttpGet, Route("{id}/Deny")]
        public async Task<IHttpActionResult> Deny(string id) {
            var campaign = await _repo.Campaigns.GetByIdAsync(id);
            campaign.CampaignStatus = CampaignStatus.Denied;
            await _repo.SaveAsync();
            return Ok();
        }
        public async Task<IHttpActionResult> Get(string id) {
            return Ok(await _campaignBuilder.BuildSingleAsync(id));
        }

        public async Task<IHttpActionResult> Put(CampaignViewModel model) {
            if (model.StartUtc.HasValue && model.StartUtc >= model.EndUtc) {
                ModelState.AddModelError("model.EndUtc", "End must come after start");
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _campaignBuilder.UpdateAsync(model);
            return Ok();
        }

        public async Task<IHttpActionResult> Post(CampaignViewModel model) {
            if (model.StartUtc.HasValue && model.StartUtc >= model.EndUtc) {
                ModelState.AddModelError("model.EndUtc", "End must come after start");
            }
			if(model.Weight < 1 || model.Weight > 16)
			{
				ModelState.AddModelError("model.Weight", "Please select a frequency");
			}
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _campaignBuilder.AddAsync(model);
            return Ok();
        }

        public async Task<IHttpActionResult> Delete(string id) {
            await _repo.Campaigns.DeleteAndSaveAsync(id);
            return Ok();
        }

		[Route("Count"), HttpGet]
		public async Task<IHttpActionResult> Count()
		{
			return Ok(await _repo.Campaigns.Where(m => m.CampaignStatus == CampaignStatus.Approved).CountAsync());
		}

		[Route("PendingCount"), HttpGet]
		public async Task<IHttpActionResult> PendingCount()
		{
			return Ok(await _repo.Campaigns.Where(m => m.CampaignStatus == CampaignStatus.Pending).CountAsync());
		}
    }
}
