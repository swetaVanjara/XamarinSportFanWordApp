using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.Feed;
using Fanword.Business.Builders.Mobile.Likes;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/PostLikes")]
    public class PostLikesController : BaseApiController
    {
        public async Task<IHttpActionResult> Get(string postId)
        {
            var likes = await new PostLikesBuilder(_repo).BuildAsync(postId);
            return Ok(likes);
        }

        [HttpGet, Route("Like/")]
        public async Task<IHttpActionResult> Like(string postId)
        {
            await new PostLikesBuilder(_repo).LikePostAsync(postId, ClaimsPrincipal.Current.Identity.GetUserId());
            return Ok();
        }

        [HttpGet, Route("Unlike/")]
        public async Task<IHttpActionResult> Unlike(string postId)
        {
            await new PostLikesBuilder(_repo).UnlikePostAsync(postId, ClaimsPrincipal.Current.Identity.GetUserId());
            return Ok();
        }
    }
}
