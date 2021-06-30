using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fanword.Business.ViewModels.Users;
using Fanword.Data.IdentityConfig.User;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace Fanword.Web.Controllers.Api
{
    [Authorize,RoutePrefix("api/Users")]
    public class UsersController : BaseApiController
    {

        [Route("Grid"), HttpGet]
        public async Task<IHttpActionResult> Grid(bool showDeleted, bool showInactive, bool showPending) {
            return Ok(await _userBuilder.BuildGridAsync(showDeleted, showInactive, showPending));
        }
		[Route("Dropdown"), HttpGet]
		public async Task<IHttpActionResult> Dropdown()
		{
			return Ok(await _repo.Users.AsQueryable().Select(i => new { DisplayName = i.Email, i.Id }).ToListAsync());
		}
        [Route("{id}/AthleteYears"), HttpGet]
        public async Task<IHttpActionResult> AtheleteYears(string id) {
            return Ok(await _userBuilder.AthleteYearsAsync(id));
        }

        [Route("FlipVerification/{athleteId}"), HttpGet]
        public async Task<IHttpActionResult> FlipVerification(string athleteId) {
            await _userBuilder.FlipVerificationAsync(athleteId);
            return Ok();
        }
        public async Task<IHttpActionResult> Get(string id) {
            return Ok(await _userBuilder.BuildSingleAsync(id));
        }
		[Route("GetCurrent"), HttpGet]
		public async Task<IHttpActionResult> GetCurrent()
		{
			var userId = User.Identity.GetUserId();
			return Ok(await _userBuilder.BuildSingleAsync(userId));
		}

        public async Task<IHttpActionResult> Put(UserViewModel model) {
            if (String.IsNullOrEmpty(model.Password)) {
                ModelState.Remove("model.Password");
                ModelState.Remove("model.ConfirmPassword");
            }
            ModelState.Remove("model.ProfileUrl");
            if (!ModelState.IsValid) return BadRequest(ModelState);
			var contentSource = _repo.ContentSources.Where(m => m.Id == model.ContentSourceId).FirstOrDefault();
			var validUser = await _repo.UserManager.UserValidator.ValidateAsync(new ApplicationUser() {
				Id = model.Id,
				Email = model.Email,
				UserName = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				ContentSource = contentSource,
            });
            if (!validUser.Succeeded) {
                foreach (var error in validUser.Errors) {
                    ModelState.AddModelError("", error);
                }
                return BadRequest(ModelState);
            }
            if (!String.IsNullOrEmpty(model.Password)) {
                var validPassword = await _repo.UserManager.PasswordValidator.ValidateAsync(model.Password);
                if (!validPassword.Succeeded) {
                    foreach (var error in validPassword.Errors) {
                        ModelState.AddModelError("model.Password", error);
                    }
                    return BadRequest(ModelState);
                }
            }
            await _userBuilder.UpdateAsync(model);
            return Ok();
        }

        public async Task<IHttpActionResult> Post(UserViewModel model) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
			var contentSource = _repo.ContentSources.Where(m => m.Id == model.ContentSourceId).FirstOrDefault();
            var validUser = await _repo.UserManager.UserValidator.ValidateAsync(new ApplicationUser() {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
				ContentSource = contentSource
            });
            if (!validUser.Succeeded) {
                foreach (var error in validUser.Errors) {
                    ModelState.AddModelError("", error);
                }
                return BadRequest(ModelState);
            }
            if (!String.IsNullOrEmpty(model.Password)) {
                var validPassword = await _repo.UserManager.PasswordValidator.ValidateAsync(model.Password);
                if (!validPassword.Succeeded) {
                    foreach (var error in validPassword.Errors) {
                        ModelState.AddModelError("model.Password", error);
                    }
                    return BadRequest(ModelState);
                }
            }
            await _userBuilder.AddAsync(model);
            return Ok();
        }


        public async Task<IHttpActionResult> Delete(string id) {
            await _userBuilder.DeleteAsync(id);
            return Ok();
        }

        [Route("Reinstate"), HttpGet]
        public async Task<IHttpActionResult> Reinstate(string id) {
            var user = await _repo.UserManager.FindByIdAsync(id);
            if (user == null) return Ok();
            user.DateDeletedUtc = null;
            await _repo.UserManager.UpdateAsync(user);
            return Ok();
        }

		[Route("Count/{time}"), HttpGet]
		public async Task<IHttpActionResult> Count(int time)
		{
			//time: 0 = total, 1 = today, 2 = this week, 3 = this month
			if(time == 1)
			{
				DateTime yesterday = DateTime.UtcNow.AddHours(-24);
				return Ok(await _repo.Users.Where(m => m.DateDeletedUtc == null && m.IsActive == true && m.DateCreatedUtc >= yesterday).CountAsync());
			}
			else if(time == 2)
			{
				DateTime weekEarlier = DateTime.UtcNow.AddDays(-7);
				return Ok(await _repo.Users.Where(m => m.DateDeletedUtc == null && m.IsActive == true && m.DateCreatedUtc >= weekEarlier).CountAsync());
			}
			else if(time == 3)
			{
				DateTime monthEarlier = DateTime.UtcNow.AddDays(-30);
				return Ok(await _repo.Users.Where(m => m.DateDeletedUtc == null && m.IsActive == true && m.DateCreatedUtc >= monthEarlier).CountAsync());
			}
			else{
				return Ok(await _repo.Users.Where(m => m.DateDeletedUtc == null && m.IsActive == true).CountAsync());
			}
		}

		[Route("CountAthletes"), HttpGet]
		public async Task<IHttpActionResult> CountAthletes()
		{
			return Ok(await _repo.Atheletes.Where(m => m.Verified == true && m.ApplicationUser.IsActive == true && m.ApplicationUser.DateDeletedUtc == null).CountAsync());
		}
		
		[Route("PendingCountAthletes"), HttpGet]
		public async Task<IHttpActionResult> PendingCountAthletes()
		{
			return Ok(await _repo.Atheletes.Where(m => m.Verified == false).CountAsync());
		}
    }
}
