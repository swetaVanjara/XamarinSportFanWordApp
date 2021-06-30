using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using ExtensionClasses.AutoMapper.Mappers;
using Facebook;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Fanword.Api.Models;
using Fanword.Api.Providers;
using Fanword.Api.Results;
using Fanword.Business.Builders.Mobile;
using Fanword.Business.Builders.Users;
using Fanword.Data.Context;
using Fanword.Data.IdentityConfig.Managers;
using Fanword.Data.IdentityConfig.User;
using Fanword.Data.Repository;
using Fanword.Poco.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fanword.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : BaseApiController
    {
        
       

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

       

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        /// <summary>
        /// Facebook endpoint
        /// </summary>
        /// <param name="facebookToken"></param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public async Task<IHttpActionResult> RegisterFacebook(string facebookToken)
        {
            if (String.IsNullOrEmpty(facebookToken))
            {
                return BadRequest("Facebook Token Cannot be empty");
            }
            var facebookId = "";
            var email = "";
            var first = "";
            var last = "";
            try
            {
                var client = new FacebookClient(facebookToken);
                var result = client.Get("/me?fields=id,first_name, last_name,email");
                var me = JsonConvert.DeserializeObject<JObject>(result.ToString());
                facebookId = me["id"].ToString();

                email = me["email"]?.ToString();
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("FanWord does not have access to your Facebook email. Please go into your Facebook settings and allow FanWord to access your email to continue");
                }
                first = me["first_name"].ToString();
                last = me["last_name"].ToString();
            }
            catch (FacebookOAuthException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Error " + ex.Message);
            }

            // Get user by facebook id
            var existing = UserManager.Users.FirstOrDefault(m => m.Logins.Any(j => j.ProviderKey == facebookId));
            if (existing == null)
            {
                // User not found by facebook id
                // Try e-mail (the user may already exist via regular login)
                existing = UserManager.Users.Include(m => m.ContentSource).FirstOrDefault(m => m.Email == email);

                if (existing == null)
                {
                    existing = new ApplicationUser();
                    existing.Email = email;
                    existing.UserName = email;
                    existing.DateCreatedUtc = DateTime.UtcNow;
                    existing.FirstName = first;
                    existing.LastName = last;
                    existing.IsActive = true;
                    var result = await UserManager.CreateAsync(existing);

                    if (!result.Succeeded)
                    {
                        return GetErrorResult(result);
                    }
                    try
                    {
                        await UserManager.AddLoginAsync(existing.Id, new UserLoginInfo("Facebook", facebookId));
                        await UserManager.AddClaimAsync(existing.Id, new Claim("FacebookAccessToken", facebookToken));


                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                }
                else
                {
                    return BadRequest("An account with your Facebook e-mail address already exists. Please login using your e-mail and password instead.");
                }
            }

            var tokenExpiration = TimeSpan.FromDays(14); //same as local accounts;

            var identity = await existing.GenerateUserIdentityAsync(_repo.UserManager, "JWT");

            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);
            var accessToken = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);

            Microsoft.Owin.Security.Infrastructure.AuthenticationTokenCreateContext context =
                new Microsoft.Owin.Security.Infrastructure.AuthenticationTokenCreateContext(
                    Request.GetOwinContext(),
                    Startup.OAuthOptions.AccessTokenFormat, ticket);

            await new RefreshTokenProvider().CreateAsync(context);
            
            var user = new UsersBuilder(_repo).GetUser(existing.Id);
            var tokenResponse = new
            {
                access_token = accessToken,
                refresh_token = context.Token,
                token_type = "bearer",
                expires_in = tokenExpiration.TotalSeconds.ToString(),
                user = JsonConvert.SerializeObject(user),
            };
            return Ok(tokenResponse);
        }

        [HttpGet]
        public async Task<IHttpActionResult> TokenRefreshSuccess(string oldToken)
        {
            var entities = new ApplicationDbContext();
            var repo = new ApplicationRepository(entities);
            var hashedToken = RefreshTokenProvider.GetHash(oldToken);
            await repo.RefreshTokens.DeleteAndSaveAsync(hashedToken);
            return Ok();
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, IsActive = true};

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var tokenExpiration = TimeSpan.FromDays(14); //same as local accounts;
            var identity = await user.GenerateUserIdentityAsync(_repo.UserManager, "JWT");

            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);
            var accessToken = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);

            Microsoft.Owin.Security.Infrastructure.AuthenticationTokenCreateContext context =
                new Microsoft.Owin.Security.Infrastructure.AuthenticationTokenCreateContext(
                    Request.GetOwinContext(),
                    Startup.OAuthOptions.AccessTokenFormat, ticket);

            await new RefreshTokenProvider().CreateAsync(context);
            
            var pocoUser = new UsersBuilder(_repo).GetUser(user.Id);

            var tokenResponse = new
            {
                access_token = accessToken,
                refresh_token = context.Token,
                token_type = "bearer",
                expires_in = tokenExpiration.TotalSeconds.ToString(),
                user = JsonConvert.SerializeObject(pocoUser),
            };

            return Ok(tokenResponse);
        }
        
        
        #region Helpers

       

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        #endregion
    }
}
