
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fanword.Web.Controllers
{
    [Authorize(Roles = "System_Admin")]
    public class ContentManagementController : Controller
    {
        // GET: ContentManagement
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _IndexTemplate() {
            return PartialView("_IndexTemplate");
        }

        public ActionResult _EditTemplate() {
            return PartialView("_EditTemplate");
        }

        public ActionResult _BreadcrumbTemplate() {
            return PartialView("_BreadcrumbTemplate");
        }




    }
}