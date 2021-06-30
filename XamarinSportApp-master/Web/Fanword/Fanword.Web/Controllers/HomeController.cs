using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fanword.Data.Enums;

namespace Fanword.Web.Controllers {
    [Authorize]
    public class HomeController : BaseMvcController {
        public ActionResult Index() {
            //return RedirectToAction("Index", User.IsInRole("Advertiser") ? "Campaigns" : "NewsNotifications");
            //return RedirectToAction("Index", "NewsNotifications");

            if (User.IsInRole(AppRoles.Advertiser) && !User.IsInRole(AppRoles.SystemAdmin))
            {
                return RedirectToAction("Index", "Campaigns");
            }else if (User.IsInRole(AppRoles.ContentSource) && !User.IsInRole(AppRoles.SystemAdmin))
            {
                return RedirectToAction("Index", "NewsNotifications");
            }
            return View();
        }
		public ActionResult _IndexTemplate()
		{
			return PartialView("_IndexTemplate");
		}
	}
}