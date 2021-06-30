using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fanword.Web.Controllers.Api {
    [Authorize, RoutePrefix("api/Likes")]
    public class LikesController : BaseApiController {
        [Route("Grid"), HttpGet]
        public async Task<IHttpActionResult> Grid() {
            return Ok(await _likeBuilder.BuildGridAsync());
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(string id) {
            //id could be a post like or comment like
            if (await _repo.CommentLikes.ExistsAsync(id)) {
                await _repo.CommentLikes.DeleteAndSaveAsync(id);
            }
            if (await _repo.PostLikes.ExistsAsync(id)) {
                await _repo.PostLikes.DeleteAndSaveAsync(id);
            }
            return Ok();
        }
    }
}
