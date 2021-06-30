using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.Feed;
using Fanword.Poco.Models;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("/api/Feed")]
    public class FeedController : BaseApiController
    {

        public async Task<IHttpActionResult> Get(DateTime lastPostCreatedAt, string id, FeedType type)
        {
            var posts = await new FeedBuilder(_repo).Build(lastPostCreatedAt, ClaimsPrincipal.Current.Identity.GetUserId(), id, type);
            return Ok(posts);
        }

        public async Task<IHttpActionResult> Delete(string postId)
        {
            var post = await _repo.Posts.FirstOrDefaultAsync(m => m.Id == postId);
            if (post.CreatedById != ClaimsPrincipal.Current.Identity.GetUserId())
                return Unauthorized();
            await new FeedBuilder(_repo).DeletePostAsync(postId);
            return Ok();
        }

        public async Task<IHttpActionResult> GetPost(string postId)
        {
            var post = await new FeedBuilder(_repo).BuildSingle(postId, ClaimsPrincipal.Current.Identity.GetUserId());
            return Ok(post);
        }
    }
}
