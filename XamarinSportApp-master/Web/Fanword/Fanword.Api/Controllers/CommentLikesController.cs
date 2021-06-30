using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.CommentLikes;
using Fanword.Business.Builders.Mobile.Likes;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/CommentLikes")]
    public class CommentLikesController : BaseApiController
    {
        [HttpGet, Route("Like/")]
        public async Task<IHttpActionResult> Like(string commentId)
        {
            await new CommentLikesBuilder(_repo).LikeCommentAsync(commentId, ClaimsPrincipal.Current.Identity.GetUserId());
            return Ok();
        }

        [HttpGet, Route("Unlike/")]
        public async Task<IHttpActionResult> Unlike(string commentId)
        {
            await new CommentLikesBuilder(_repo).UnlikeCommentAsync(commentId, ClaimsPrincipal.Current.Identity.GetUserId());
            return Ok();
        }
    }
}
