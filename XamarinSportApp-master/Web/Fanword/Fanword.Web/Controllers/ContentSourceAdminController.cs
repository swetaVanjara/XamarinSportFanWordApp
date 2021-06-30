using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fanword.Web.Controllers
{
	[Authorize(Roles ="System_Admin")]
    public class ContentSourceAdminController : Controller
    {
        // GET: ContentSourceAdmin
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult _IndexTemplate()
		{
			return PartialView("_IndexTemplate");
		}

		public ActionResult _ContentSourceTemplate()
		{
			return PartialView("_ContentSourceTemplate");
		}

		public ActionResult _BreadcrumbTemplate()
		{
			return PartialView("_BreadcrumbTemplate");
		}
	}
}