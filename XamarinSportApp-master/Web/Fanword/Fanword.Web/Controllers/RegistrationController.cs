using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EmailService;
using Fanword.Business.Builders.Advertisers;
using Fanword.Business.Builders.ContentSources;
using Fanword.Business.ViewModels.Registration;
using Fanword.Data.Entities;
using Fanword.Data.IdentityConfig.Roles;
using Fanword.Data.IdentityConfig.User;
using Microsoft.AspNet.Identity;

namespace Fanword.Web.Controllers {

    public class RegistrationController : BaseMvcController {
        // GET: Registration
        public ActionResult Advertisers() {
            return View();
        }

		public ActionResult ContentSources()
		{
			return View();
		} 

        [HttpPost]
        public async Task<ActionResult> Advertisers(RegistrationIndexViewModel model) {
            if (!ModelState.IsValid) return View();

            //create registration and send email
            var registration = await _repo.AdvertiserRegistrationRequests.AddOrUpdateAndSaveAsync(new AdvertiserRegistrationRequest() { Email = model.Email });

            var recipient = new EmailRecipient(model.Email);

            var content = new EmailContent()
                                    .Title("Fanword Advertising")
                                    .Paragraph("Thank you for your interest in Advertising on Fanword.  Click the link below to complete your registration.")
                                    .Button(Url.Action("AdvertiserRegistration", "Registration", new { id = registration.Id }, Request.Url.Scheme), "#32c5d2", "#FFF", "Register");

            var email = new Email(recipient, "Fanword Advertising Sign-Up", content.ToHtml());


            email.Send();

            return RedirectToAction("InitialAdvertisersRegistrationComplete");
        }
		public async Task<ActionResult> AdvertisersWeb()
		{
			var userId = User.Identity.GetUserId();
			var user = _repo.Users.Where(m => m.Id == userId).FirstOrDefault();

			if (!ModelState.IsValid) return View();

			var registration = await _repo.AdvertiserRegistrationRequests.AddOrUpdateAndSaveAsync(new AdvertiserRegistrationRequest() { Email = user.Email });

			return RedirectToAction("AdvertiserRegistration", "registration", new { id = registration.Id });
		}

		[HttpPost]
		public async Task<ActionResult> ContentSources(RegistrationIndexViewModel model)
		{
			if (!ModelState.IsValid) return View();

			var registration = await _repo.ContentSourceRegistrationRequests.AddOrUpdateAndSaveAsync(new ContentSourceRegistrationRequest() { Email = model.Email });

			var recipient = new EmailRecipient(model.Email);

			var content = new EmailContent()
									.Title("Fanword Content Sources")
									.Paragraph("Thank you for your interest in being a content source on Fanword.  Click the link below to complete your registration.")
									.Button(Url.Action("ContentSourceRegistration", "Registration", new { id = registration.Id }, Request.Url.Scheme), "#32c5d2", "#FFF", "Register");

			var email = new Email(recipient, "Fanword Content Source Sign-Up", content.ToHtml());

			email.Send();

			return RedirectToAction("InitialContentSourcesRegistrationComplete");
		}


        public ActionResult RegistrationError() {
            return View();
        }

		public async Task<ActionResult> ContentSourceRegistration(string id)
		{
			if (String.IsNullOrEmpty(id)) return RedirectToAction("RegistrationError");
			var registration = await _repo.ContentSourceRegistrationRequests.GetByIdAsync(id);
			if (registration == null) return RedirectToAction("RegistrationError");
			var existingUser = await _repo.UserManager.FindByEmailAsync(registration.Email);
			var vm = new ContentSourceRegistrationViewModel()
			{
				Email = registration.Email,
				IsAccountCreated = existingUser != null,
				FirstName = existingUser?.FirstName,
				LastName = existingUser?.LastName,
			};

			return View(vm);
		}

		[HttpPost]
		public async Task<ActionResult> ContentSourceRegistration(ContentSourceRegistrationViewModel model)
		{
			if (model.IsAccountCreated)
			{
				ModelState.Remove("Password");
				ModelState.Remove("ConfirmPassword");
				ModelState.Remove("FirstName");
				ModelState.Remove("LastName");
			}
			else
			{
				var passwordValid = await _repo.UserManager.PasswordValidator.ValidateAsync(model.Password);
				if (!passwordValid.Succeeded)
				{
					foreach (var error in passwordValid.Errors)
					{
						ModelState.AddModelError("Password", error);
					}
				}

				var validUser = await _repo.UserManager.UserValidator.ValidateAsync(new ApplicationUser()
				{
					Id = Guid.NewGuid().ToString(),
					DateCreatedUtc = DateTime.UtcNow,
					Email = model.Email,
					UserName = model.Email,
					EmailConfirmed = true,
					FirstName = model.FirstName,
					LastName = model.LastName,
				});
				if (!validUser.Succeeded)
				{
					foreach (var error in validUser.Errors)
					{
						ModelState.AddModelError("Email", error);
					}
				}
			}

			if (!ModelState.IsValid) return View(model);

			var builder = new ContentSourceBuilder(_repo);
			if (model.IsAccountCreated)
			{
				var id = await builder.AddAsync(model);
				var user = await _repo.UserManager.FindByEmailAsync(model.Email);
				user.ContentSourceId = id;
				await _repo.UserManager.UpdateAsync(user);
				if (!await _repo.RoleManager.RoleExistsAsync("ContentSource"))
				{
					await _repo.RoleManager.CreateAsync(new ApplicationRole("ContentSource") { DateCreatedUtc = DateTime.UtcNow });
				}
				//await _repo.UserManager.AddToRoleAsync(user.Id, "ContentSource");
				//await _repo.SignInManager.SignInAsync(user, false, false);

				var registrations = await _repo.ContentSourceRegistrationRequests.Where(m => m.Email.ToLower() == user.Email.ToLower()).ToListAsync();
				await _repo.ContentSourceRegistrationRequests.DeleteAndSaveAsync(registrations);

				return RedirectToAction("Index", "Home");
			}

			//create account
			var newUser = new ApplicationUser()
			{
				Id = Guid.NewGuid().ToString(),
				DateCreatedUtc = DateTime.UtcNow,
				Email = model.Email,
				UserName = model.Email,
				EmailConfirmed = true,
				IsActive = true,
				FirstName = model.FirstName,
				LastName = model.LastName,
			};

			var contentSourceId = await builder.AddAsync(model);
			newUser.ContentSourceId = contentSourceId;

			await _repo.UserManager.CreateAsync(newUser, model.Password);
			if (!await _repo.RoleManager.RoleExistsAsync("ContentSource"))
			{
				await _repo.RoleManager.CreateAsync(new ApplicationRole("ContentSource") { DateCreatedUtc = DateTime.UtcNow });
			}
			//await _repo.UserManager.AddToRoleAsync(newUser.Id, "ContentSource");
			//await _repo.SignInManager.SignInAsync(newUser, false, false);
			var otherRegistrations = await _repo.ContentSourceRegistrationRequests.Where(m => m.Email.ToLower() == newUser.Email.ToLower()).ToListAsync();
			await _repo.ContentSourceRegistrationRequests.DeleteAndSaveAsync(otherRegistrations);
			return RedirectToAction("Index", "Home");
		}

		public async Task<ActionResult> AdvertiserRegistration(string id) {
            if (String.IsNullOrEmpty(id)) return RedirectToAction("RegistrationError");
            var registration = await _repo.AdvertiserRegistrationRequests.GetByIdAsync(id);
            if (registration == null) return RedirectToAction("RegistrationError");
            var existingUser = await _repo.UserManager.FindByEmailAsync(registration.Email);
            var vm = new AdvertiserRegistrationViewModel() {
                Email = registration.Email,
                IsAccountCreated = existingUser != null,
                FirstName = existingUser?.FirstName,
                LastName = existingUser?.LastName,
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> AdvertiserRegistration(AdvertiserRegistrationViewModel model) {
            if (model.IsAccountCreated) {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
                ModelState.Remove("FirstName");
                ModelState.Remove("LastName");
            }
            else {
                var passwordValid = await _repo.UserManager.PasswordValidator.ValidateAsync(model.Password);
                if (!passwordValid.Succeeded) {
                    foreach (var error in passwordValid.Errors) {
                        ModelState.AddModelError("Password", error);
                    }
                }

                var validUser = await _repo.UserManager.UserValidator.ValidateAsync(new ApplicationUser() {
                    Id = Guid.NewGuid().ToString(),
                    DateCreatedUtc = DateTime.UtcNow,
                    Email = model.Email,
                    UserName = model.Email,
                    EmailConfirmed = true,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                });
                if (!validUser.Succeeded) {
                    foreach (var error in validUser.Errors) {
                        ModelState.AddModelError("Email", error);
                    }
                }
            }

            if (!ModelState.IsValid) return View(model);

            var builder = new AdvertiserBuilder(_repo);
            if (model.IsAccountCreated) {
                var id = await builder.AddAsync(model);
                var user = await _repo.UserManager.FindByEmailAsync(model.Email);
                user.AdvertiserId = id;
                await _repo.UserManager.UpdateAsync(user);
                if (!await _repo.RoleManager.RoleExistsAsync("Advertiser")) {
                    await _repo.RoleManager.CreateAsync(new ApplicationRole("Advertiser"){ DateCreatedUtc = DateTime.UtcNow });
                }
                await _repo.UserManager.AddToRoleAsync(user.Id, "Advertiser");
                await _repo.SignInManager.SignInAsync(user, false, false);

                var registrations = await _repo.AdvertiserRegistrationRequests.Where(m => m.Email.ToLower() == user.Email.ToLower()).ToListAsync();
                await _repo.AdvertiserRegistrationRequests.DeleteAndSaveAsync(registrations);

                return RedirectToAction("Index", "Home");
            }

            //create account
            var newUser = new ApplicationUser() {
                Id = Guid.NewGuid().ToString(),
                DateCreatedUtc = DateTime.UtcNow,
                Email = model.Email,
                UserName = model.Email,
                EmailConfirmed = true,
                IsActive = true,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var advertiserId = await builder.AddAsync(model);
            newUser.AdvertiserId = advertiserId;

            await _repo.UserManager.CreateAsync(newUser, model.Password);
            if (!await _repo.RoleManager.RoleExistsAsync("Advertiser")) {
                await _repo.RoleManager.CreateAsync(new ApplicationRole("Advertiser"){ DateCreatedUtc = DateTime.UtcNow });
            }
            await _repo.UserManager.AddToRoleAsync(newUser.Id, "Advertiser");
            await _repo.SignInManager.SignInAsync(newUser, false, false);
            var otherRegistrations = await _repo.AdvertiserRegistrationRequests.Where(m => m.Email.ToLower() == newUser.Email.ToLower()).ToListAsync();
            await _repo.AdvertiserRegistrationRequests.DeleteAndSaveAsync(otherRegistrations);
            return RedirectToAction("Index", "Home");
        }






        public ActionResult InitialAdvertisersRegistrationComplete() {
            return View();
        }

		public ActionResult InitialContentSourcesRegistrationComplete()
		{
			return View();
		}
    }
}