using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.Posts;
using Fanword.Poco.Models;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/Posts")]
    public class PostsController : BaseApiController
    {
        public async Task<IHttpActionResult> Get(string postId)
        {
            return Ok(await new PostsBuilder(_repo).BuildAsync(postId));
        }

        public async Task<IHttpActionResult> Post(Post post)
        {
            var userId = User.Identity.GetUserId();
            var userContentSourceId =  await _repo.Users.Where(m => m.Id == userId).Select(m => m.ContentSourceId).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(post.ContentSourceId) && userContentSourceId != post.ContentSourceId)
            {
                return Unauthorized();
            }
            await new PostsBuilder(_repo).SaveAsync(post, User.Identity.GetUserId());
            return Ok();
        }

        [HttpGet, Route("Clone")]
        public async Task Clone(string postId)
        {
            await new PostsBuilder(_repo).CloneAsync(postId, User.Identity.GetUserId());
        }
    }
}
