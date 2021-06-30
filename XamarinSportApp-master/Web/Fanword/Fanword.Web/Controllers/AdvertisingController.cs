
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fanword.Web.Controllers
{
    [Authorize(Roles = "System_Admin,DataAdv")]
    public class AdvertisingController : BaseMvcController
    {
        // GET: Advertising
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _IndexTemplate() {
            return PartialView("Admins/_IndexTemplate");
        }

        public ActionResult _AdvertiserTemplate() {
            return PartialView("Admins/_AdvertiserTemplate");
        }

        public ActionResult _BreadcrumbTemplate() {
            return PartialView("_BreadcrumbTemplate");
        }


     

        


    }
}