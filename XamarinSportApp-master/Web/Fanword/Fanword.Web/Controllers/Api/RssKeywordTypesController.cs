using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.ViewModels.RssKeywordTypes;

namespace Fanword.Web.Controllers.Api
{
	[Authorize, RoutePrefix("api/RssKeywordTypes")]
    public class RssKeywordTypesController : BaseApiController
    {
		[Route("Grid"), HttpGet]
		public async Task<IHttpActionResult> GetAll()
		{
			return Ok(await _rssKeywordTypeBuilder.BuildGridAsync(true));
		}
		public async Task<IHttpActionResult> Get(string id)
		{
			return Ok(await _rssKeywordTypeBuilder.BuildSingleAsync(id));
		}
		public async Task<IHttpActionResult> Put(RssKeywordTypeViewModel model)
		{
			if (String.IsNullOrEmpty(model.Name))
			{
				ModelState.AddModelError("model.Name", "Name is Required");
			}
			if (!ModelState.IsValid) return BadRequest(ModelState);
			await _rssKeywordTypeBuilder.UpdateAsync(model);
			return Ok();
		}

		public async Task<IHttpActionResult> Post(RssKeywordTypeViewModel model)
		{
			if (String.IsNullOrEmpty(model.Name))
			{
				ModelState.AddModelError("model.Name", "Name is Required");
			}
			if (!ModelState.IsValid) return BadRequest(ModelState);
			await _rssKeywordTypeBuilder.AddAsync(model);
			return Ok();
		}


		[Route("{id}/EnableDisable"), HttpGet]
		public async Task<IHttpActionResult> EnableDisable(string id)
		{
			var feedItem = await _repo.RssKeywordTypes.GetByIdAsync(id);
			if (feedItem == null) return Ok();
			feedItem.IsActive = !feedItem.IsActive;
			await _repo.SaveAsync();
			return Ok();
		}

	}
}
