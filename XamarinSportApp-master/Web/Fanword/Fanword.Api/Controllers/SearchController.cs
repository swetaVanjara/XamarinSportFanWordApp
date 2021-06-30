using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.Scores;
using Fanword.Business.Builders.Mobile.Search;
using Fanword.Poco.Models;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/Search")]
    public class SearchController : BaseApiController
    {
        public async Task<IHttpActionResult> Get(string filter)
        {
            var scores = await new SearchBuilder(_repo).BuildAsync(User.Identity.GetUserId(), filter);
            return Ok(scores);
        }

        public async Task<IHttpActionResult> Get(string filter, FeedType type)
        {
            var scores = await new SearchBuilder(_repo).BuildAsync(User.Identity.GetUserId(), filter, type);
            return Ok(scores);
        }
    }
}
