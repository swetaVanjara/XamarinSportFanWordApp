using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fanword.Web.Controllers
{
    [Authorize(Roles = "ContentSource, System_Admin")]
    public class ContentSourcesController : Controller
    {
        // GET: ContentSources
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult _IndexTemplate()
		{
			return PartialView("_IndexTemplate");
		}
	}
}