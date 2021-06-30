using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fanword.Web.Controllers.Api
{
    [RoutePrefix("api/Timezones")]
    public class TimezonesController : BaseApiController
    {
        [Route("SelectControlList"),HttpGet]
        public IHttpActionResult SelectControlList() {
            var timeZones = TimeZoneInfo.GetSystemTimeZones()
                .Select(i => new {DisplayName = i.DisplayName, Id = i.Id})
                .ToList();
            return Ok(timeZones);
        }

        [Route("CurrentTimeZoneId"), HttpGet]
        public async Task<IHttpActionResult> PopularTimeZone()
        {
            var timezoneId = _repo.Events.AsQueryable().GroupBy(m => m.TimezoneId).OrderByDescending(m => m.Count()).Select(m => m.FirstOrDefault().TimezoneId).FirstOrDefault();

            return Ok(timezoneId);

        }

    }
}
