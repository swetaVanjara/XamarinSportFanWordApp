using Fanword.Business.ViewModels.RssKeywords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fanword.Web.Controllers.Api
{
	//[Authorize, RoutePrefix("api/RssKeywords")]
	//public class RssKeywordsController : BaseApiController
 //   {
	//	[Route("Grid"), HttpGet]
	//	public async Task<IHttpActionResult> Grid()
	//	{
	//		var result = await _rssKeywordBuilder.BuildGridAsync(id, true);
	//		return Ok(result);
	//	}

	//	public async Task<IHttpActionResult> Get(string id)
	//	{
	//		return Ok(await _rssKeywordBuilder.BuildSingleAsync(id));
	//	}

	//	public async Task<IHttpActionResult> Put(RssKeywordViewModel model)
	//	{
	//		if (String.IsNullOrEmpty(model.Keyword))
	//		{
	//			ModelState.AddModelError("model.Keyword", "Keyword is Required");
	//		}
	//		if (!ModelState.IsValid) return BadRequest(ModelState);
	//		await _rssKeywordBuilder.UpdateAsync(model);
	//		return Ok();
	//	}

	//	[Route("Add")]
	//	public async Task<IHttpActionResult> Post(RssKeywordViewModel model)
	//	{
	//		if (String.IsNullOrEmpty(model.Keyword))
	//		{
	//			ModelState.AddModelError("model.Keyword", "Keyword is Required");
	//		}
	//		if (!ModelState.IsValid) return BadRequest(ModelState);
	//		await _rssKeywordBuilder.AddAsync(model);
	//		return Ok();
	//	}


	//	[Route("{id}/EnableDisable"), HttpGet]
	//	public async Task<IHttpActionResult> EnableDisable(string id)
	//	{
	//		var feedItem = await _repo.RssKeywords.GetByIdAsync(id);
	//		if (feedItem == null) return Ok();
	//		feedItem.IsActive = !feedItem.IsActive;
	//		await _repo.SaveAsync();
	//		return Ok();
	//	}


	//}
}
