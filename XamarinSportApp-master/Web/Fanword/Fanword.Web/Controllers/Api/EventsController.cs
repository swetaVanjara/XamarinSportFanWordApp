using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.ViewModels.Events;
using System.Web.Http.Description;

namespace Fanword.Web.Controllers.Api
{
    [Authorize(Roles = "System_Admin,Data,DataAdv"), RoutePrefix("api/Events")]
    public class EventsController : BaseApiController
    {
        [Route("Grid"), HttpGet]
        public async Task<IHttpActionResult> Grid() {
            return Ok(await _eventBuilder.BuildGridAsync());
        }

        public async Task<IHttpActionResult> Get(string id) {
            return Ok(await _eventBuilder.BuildSingleAsync(id));
        }

        public async Task<IHttpActionResult> Post(EventViewModel model) {
            ModelState.Remove("model.DateOfEventInTimezone");
            if (model.DateOfEventInTimezone == DateTime.MinValue || model.StringConversionDate == DateTime.MinValue) ModelState.AddModelError("model.DateOfEventInTimezone", "Event date is required");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var utcDateFromTimezone = TimeZoneInfo.ConvertTimeToUtc(model.StringConversionDate, TimeZoneInfo.FindSystemTimeZoneById(model.TimezoneId));
            model.DateOfEventUtc = utcDateFromTimezone;
            await _eventBuilder.AddAsync(model);
            return Ok();
        }

        public async Task<IHttpActionResult> Put(EventViewModel model) {
            ModelState.Remove("model.DateOfEventInTimezone");
            if (model.DateOfEventInTimezone == DateTime.MinValue || model.StringConversionDate == DateTime.MinValue) ModelState.AddModelError("model.DateOfEventInTimezone", "Event date is required");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var utcDateFromTimezone = TimeZoneInfo.ConvertTimeToUtc(model.StringConversionDate, TimeZoneInfo.FindSystemTimeZoneById(model.TimezoneId));
            model.DateOfEventUtc = utcDateFromTimezone;
            await _eventBuilder.UpdateAsync(model);
            return Ok();
        }

        //[Route("SaveEvents"), HttpPost, ResponseType(typeof(List<EventViewModel>))]
        //public async Task<IHttpActionResult> SaveEvents(List<EventViewModel> events)
        //{
        //    if (events.Any(m => m.EventTeams.Count < 2)) ModelState.AddModelError("EventTeams", "Each Event must have at least two teams");
        //    if (events.Any(m => m.StringConversionDate == DateTime.MinValue)) ModelState.AddModelError("EventDate", "Each Event must have a date");
        //    if (!ModelState.IsValid) return BadRequest(ModelState);
        //    foreach (var model in events)
        //    {
        //        if (!string.IsNullOrEmpty(model.TimezoneId))
        //        {
        //            var utcDateFromTimezone = TimeZoneInfo.ConvertTimeToUtc(model.StringConversionDate, TimeZoneInfo.FindSystemTimeZoneById(model.TimezoneId));
        //            model.DateOfEventUtc = utcDateFromTimezone;
        //        }
        //    }
        //    await _eventBuilder.AddAsync(events);
        //    return Ok();
        //}

		public async Task<IHttpActionResult> Delete(string id)
		{
			await _repo.Events.DeleteAndSaveAsync(id);
			var eventTeams = _repo.EventTeams.Where(m => m.EventId == id).ToList();
			await _repo.EventTeams.DeleteAndSaveAsync(eventTeams);
			return Ok();
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

            if(dbPin == null)
            {
                ModelState.AddModelError("PinNumber", "Pin number is invalid");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [Route("ValidateTicketsLink"), HttpPost]
        public async Task<IHttpActionResult> ValidateTicketsLink(PurchaseTicketUrlViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok();
        }


    }
}
