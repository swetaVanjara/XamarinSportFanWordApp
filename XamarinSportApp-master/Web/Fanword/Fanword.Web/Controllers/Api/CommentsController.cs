using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fanword.Web.Controllers.Api
{
    [Authorize,RoutePrefix("api/Comments")]
    public class CommentsController : BaseApiController
    {
        [Route("Grid"),HttpGet]
        public async Task<IHttpActionResult> Grid() {
            return Ok(await _commentBuilder.BuildGridAsync());
        }

        public async Task<IHttpActionResult> Delete(string id) {
            //var dbItem = await _repo.Comments.GetByIdAsync(id);
           // if (dbItem == null) return Ok();
            await _repo.Comments.DeleteAndSaveAsync(id);
            return Ok();
        }
    }
}
