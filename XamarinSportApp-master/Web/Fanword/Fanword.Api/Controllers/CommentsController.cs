using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.Comments;
using Fanword.Business.Builders.Mobile.Likes;
using Fanword.Poco.Models;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/Comments")]
    public class CommentsController : BaseApiController
    {
        public async Task<IHttpActionResult> Get(string postId)
        {
            var comments = await new CommentsBuilder(_repo).BuildAsync(postId, ClaimsPrincipal.Current.Identity.GetUserId());
            return Ok(comments);
        }

        public async Task<IHttpActionResult> Post(Comment comment)
        {
            await new CommentsBuilder(_repo).SaveCommentAsync(comment, ClaimsPrincipal.Current.Identity.GetUserId());
            return Ok();
        }
    }
}
