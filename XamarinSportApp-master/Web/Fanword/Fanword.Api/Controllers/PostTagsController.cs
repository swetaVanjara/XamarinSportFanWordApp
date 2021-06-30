using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.PostTags;

namespace Fanword.Api.Controllers
{

    [Authorize, RoutePrefix("api/PostTags")]
    public class PostTagsController : BaseApiController
    {
        public async Task<IHttpActionResult> Get(string postId)
        {
            return Ok(await new PostTagsBuilder(_repo).BuildAsync(postId));
        }
    }
}
