using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fanword.Data.IdentityConfig.Managers;
using Fanword.Data.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Fanword.Web.Controllers
{
    public class BaseMvcController : Controller
    {
        protected ApplicationRepository _repo => Request.GetOwinContext().Get<ApplicationRepository>();
        protected ApplicationUserManager UserManager => _repo.UserManager;
        protected ApplicationSignInManager SignInManager => _repo.SignInManager;
        protected ApplicationRoleManager RoleManager => _repo.RoleManager;

        protected IAuthenticationManager AuthenticationManager => Request.GetOwinContext().Authentication;


        protected void AddErrors(IdentityResult result) {
            foreach (var error in result.Errors) {
                ModelState.AddModelError("", error);
            }
        }

        protected string GetCurrentUrl() {
            return Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
        }

        protected ActionResult RedirectToLocal(string returnUrl) {
            if (Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}