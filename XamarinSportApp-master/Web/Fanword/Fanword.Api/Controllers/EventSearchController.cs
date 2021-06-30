using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.SearchEvents;
using Fanword.Business.Builders.Mobile.SearchProfiles;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/EventSearch")]
    public class EventSearchController : BaseApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> Search(DateTime time)
        {
            var search = await new SearchEventsBuilder(_repo).SearchAsync(time, User.Identity.GetUserId());
            return Ok(search);
        }
    }
}
