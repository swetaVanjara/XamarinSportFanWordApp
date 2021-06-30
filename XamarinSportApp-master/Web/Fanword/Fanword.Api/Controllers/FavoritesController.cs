using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.Favorites;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/Favorites")]
    public class FavoritesController : BaseApiController
    {
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await new FavoritesBuilder(_repo).BuildAsync(User.Identity.GetUserId()));
        }

    }
}
