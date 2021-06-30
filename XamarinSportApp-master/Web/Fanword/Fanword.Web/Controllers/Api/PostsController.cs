using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Fanword.Business.ViewModels.ContentManagement;

namespace Fanword.Web.Controllers.Api
{
    [Authorize,RoutePrefix("api/Posts")]
    public class PostsController : BaseApiController
    {
        [Route("Grid"), HttpGet, ResponseType(typeof(List<PostRecord>))]
        public async Task<IHttpActionResult> Grid() {
            return Ok(await _postsBuilder.BuildGridAsync());
        }

        public async Task<IHttpActionResult> Get(string id) {
            return Ok(await _postsBuilder.BuildSingleAsync(id));
        }

        public async Task<IHttpActionResult> Put(PostViewModel model) {
            await _postsBuilder.UpdateAsync(model);
            return Ok();
        }

        public async Task<IHttpActionResult> Delete(string id) {
            await _repo.Posts.DeleteAndSaveAsync(id);
            return Ok();
        }
    }
}
