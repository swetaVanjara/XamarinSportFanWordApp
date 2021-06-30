using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fanword.Web.Controllers {

   
    public class EventsController : BaseMvcController {
        // GET: Events
        [Authorize(Roles = "System_Admin,Data,DataAdv")]
        public ActionResult Index() {
            return View();
        }

        [Authorize(Roles = "System_Admin,Data,DataAdv")]
        public ActionResult _IndexTemplate() {
            return PartialView("_IndexTemplate");
        }

        [Authorize(Roles = "System_Admin,Data,DataAdv")]
        public ActionResult _BreadcrumbTemplate() {
            return PartialView("_BreadcrumbTemplate");
        }

        [Authorize(Roles = "System_Admin,Data,DataAdv")]
        public ActionResult _EditTemplate() {
            return PartialView("_EditTemplate");
        }

        [AllowAnonymous]
        public ActionResult Manage()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult _ManageTemplate()
        {
            return PartialView("_ManageTemplate");
        }
    }
}