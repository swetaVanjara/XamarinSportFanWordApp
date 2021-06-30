using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.SchoolProfiles;
using Fanword.Business.Builders.Mobile.TeamProfile;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{

    [Authorize, RoutePrefix("api/SchoolProfile")]
    public class SchoolProfileController : BaseApiController
    {
        public async Task<IHttpActionResult> Get(string schoolId)
        {
            var proflie = await new SchoolProfileBuilder(_repo).BuildAsync(User.Identity.GetUserId(), schoolId);
            return Ok(proflie);
        }

        [HttpGet, Route("RequestAdmin")]
        public async Task<IHttpActionResult> RequestAdmin(string schoolId)
        {
            await new SchoolProfileBuilder(_repo).RequestAdminAsync(ClaimsPrincipal.Current.Identity.GetUserId(), schoolId);
            return Ok();
        }
    }
}
