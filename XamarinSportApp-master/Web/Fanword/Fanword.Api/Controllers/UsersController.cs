using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.Builders.Mobile;
using Fanword.Poco.Models;
using Microsoft.AspNet.Identity;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/Users")]
    public class UsersController : BaseApiController
    {
        [Route("SaveUser")]
        public async Task<IHttpActionResult> SaveUser(User user)
        {
            return Ok(await new UsersBuilder(_repo).SaveUserAync(user, User.Identity.GetUserId()));
        }

        [Route("SaveAthleteUser")]
        public async Task<IHttpActionResult> SaveAthleteUser(AthleteUser athleteUser)
        {
            athleteUser.Athlete = await new AthletesBuilder(_repo).SaveAthleteAync(athleteUser.Athlete, User.Identity.GetUserId());
            athleteUser.User = await new UsersBuilder(_repo).SaveUserAync(athleteUser.User, User.Identity.GetUserId());
            
            return Ok(athleteUser);
        }

        [Route("DeleteAthlete"), HttpDelete]
        public async Task<IHttpActionResult> DeleteAthlete()
        {
            await new AthletesBuilder(_repo).DeleteAthleteAsync(User.Identity.GetUserId());
            return Ok();
        }

        [Route("MyUser")]
        public IHttpActionResult GetMyUser()
        {
            return Ok(new UsersBuilder(_repo).GetUser(User.Identity.GetUserId()));
        }
        [Route("GetUser")]
        public IHttpActionResult GetUser(string userId) {
            return Ok(new UsersBuilder(_repo).GetUser(userId));
        }
    }
}
