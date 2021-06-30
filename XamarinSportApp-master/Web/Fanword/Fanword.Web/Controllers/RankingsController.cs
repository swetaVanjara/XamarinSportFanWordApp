
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fanword.Web.Controllers
{
   
    [Authorize(Roles = "System_Admin,Data,DataAdv")]
    public class RankingsController : BaseMvcController
    {
        // GET: Rankings
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _IndexTemplate() {
            return PartialView("_IndexTemplate");
        }

        public ActionResult _BreadcrumbTemplate() {
            return PartialView("_BreadcrumbTemplate");
        }

        public ActionResult _EditTemplate() {
            return PartialView("_EditTemplate");
        }

        public ActionResult _MenuTemplate() {
            return PartialView("_MenuTemplate");
        }
    }
}