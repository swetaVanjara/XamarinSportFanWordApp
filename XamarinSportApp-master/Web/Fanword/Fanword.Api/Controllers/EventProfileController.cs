using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile.EventProfile;
using Fanword.Business.Builders.Mobile.SchoolProfiles;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/EventProfile")]
    public class EventProfileController : BaseApiController
    {
        public async Task<IHttpActionResult> Get(string eventId)
        {
            var proflie = await new EventProfileBuilder(_repo).BuildAsync(User.Identity.GetUserId(), eventId);
            return Ok(proflie);
        }   
    }
}
