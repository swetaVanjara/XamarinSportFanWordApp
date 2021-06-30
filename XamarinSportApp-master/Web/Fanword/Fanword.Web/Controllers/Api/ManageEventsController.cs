using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.ViewModels.Events;
using System.Web.Http.Description;
using System.Data.Entity;

namespace Fanword.Web.Controllers.Api
{
    [AllowAnonymous, RoutePrefix("api/ManageEvents")]
    public class ManageEventsController : BaseApiController
    {
        [Route("SaveEvents"), HttpPost, ResponseType(typeof(List<EventViewModel>))]
        public async Task<IHttpActionResult> SaveEvents(List<EventViewModel> events)
        {
            ModelState.Clear();

            if (events.Any(m => string.IsNullOrEmpty(m.Location) && m.IsDeleted == false)) ModelState.AddModelError("EventLocation", "Each Event must have a location");
            if (events.Any(m => string.IsNullOrEmpty(m.TimezoneId) && m.IsDeleted == false)) ModelState.AddModelError("EventTimezone", "Each Event must have a Timezone");
            if (events.Any(m => (m.DateOfEventInTimezone == DateTime.MinValue || m.StringConversionDate == DateTime.MinValue) && m.IsDeleted == false)) ModelState.AddModelError("DateOfEventInTimezone", "Each Event must have a Date");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            foreach (var model in events)
            {
                if (!string.IsNullOrEmpty(model.TimezoneId))
                {
                    var utcDateFromTimezone = TimeZoneInfo.ConvertTimeToUtc(model.StringConversionDate, TimeZoneInfo.FindSystemTimeZoneById(model.TimezoneId));
                    model.DateOfEventUtc = utcDateFromTimezone;
                }
            }
            await _eventBuilder.AddOrUpdateAsync(events);
            return Ok();
        }

        [Route("GetManagementGridEvents"), HttpPost]
        public async Task<IHttpActionResult> GetManagementGridEvents(EventFilter model)
        {
            try
            {
                var events = (await _eventBuilder.BuildManagementGridAsync(model));
                return Ok(events);
            }
            catch (Exception ex)
            {

                throw;
            }
            //var events = (await _eventBuilder.BuildManagementGridAsync(model));
            //return Ok(events);
        }

        [Route("GetEventManagementPin"), HttpGet]
        public async Task<IHttpActionResult> GetEventManagementPin()
        {
            var model = (await _eventBuilder.BuildEventMangementPinAsync());
            return Ok(model);
        }

        [Route("SaveEventManagementPin"), HttpPost]
        public async Task<IHttpActionResult> SaveEventManagementPin(EventManagementPinViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            model = await _eventBuilder.AddOrUpdatePinAsync(model);
            return Ok(model);
        }

        [Route("ValidatePin"), HttpPost]
        public async Task<IHttpActionResult> ValidatePin(EventManagementPinViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var dbPin = _repo.EventManagementPins.Where(m => m.IsActive && m.PinNumber == model.PinNumber).FirstOrDefault();

            if (dbPin == null)
            {
                ModelState.AddModelError("PinNumber", "Pin number is invalid");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [Route("SelectSportControlList"), HttpGet]
        public async Task<IHttpActionResult> SelectControlList()
        {
            return Ok(await _repo.Sports.AsQueryable().Select(i => new { DisplayName = i.Name, i.Id }).OrderBy(i => i.DisplayName).ToListAsync());
        }

        [Route("SelectTeamControlList"), HttpGet]
        public async Task<IHttpActionResult> SelectList()
        {
            var query = _repo.Teams.Where(m => m.School.DateDeletedUtc == null && m.Sport.DateDeletedUtc == null);
            return Ok(await query.Select(i => new { DisplayName = i.School.Name + " - " + i.Sport.Name, i.Id, i.SportId }).OrderBy(i => i.DisplayName).ToListAsync());
        }

    }
}
