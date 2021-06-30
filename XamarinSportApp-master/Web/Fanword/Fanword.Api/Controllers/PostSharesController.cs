using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.PostShares;
using Fanword.Business.Builders.Mobile.PostTags;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/PostShares")]
    public class PostSharesController : BaseApiController
    {
        public async Task<IHttpActionResult> Get(string postId)
        {
            return Ok(await new PostSharesBuilder(_repo).BuildAsync(postId));
        }

        [HttpGet, Route("SaveShare")]
        public async Task<IHttpActionResult> SaveShare(string postId)
        {
            await new PostSharesBuilder(_repo).SaveShareAsync(postId, User.Identity.GetUserId());
            return Ok();
        }
    }
}
