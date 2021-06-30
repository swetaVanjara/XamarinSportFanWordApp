using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fanword.Web.Controllers {
    [Authorize]
    public class CommentsController : BaseMvcController {
        // GET: Comments
        public ActionResult Index() {
            return View();
        }

        public ActionResult _IndexTemplate() {
            return PartialView("_IndexTemplate");
        }

        public ActionResult _BreadcrumbTemplate() {
            return PartialView("_BreadcrumbTemplate");
        }
    }
}