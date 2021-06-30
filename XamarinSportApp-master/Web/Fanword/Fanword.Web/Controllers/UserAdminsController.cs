using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fanword.Web.Controllers
{
    [Authorize(Roles ="System_Admin")]
    public class UserAdminsController : BaseMvcController
    {
        // GET: UserAdmins
        public ActionResult Index()
        {
            return View();
        }
		public ActionResult _IndexTemplate()
		{
			return PartialView("_IndexTemplate");
		}

		public ActionResult _ViewTeamTemplate()
		{
			return PartialView("_ViewTeamTemplate");
		}

		public ActionResult _ViewSchoolTemplate()
		{
			return PartialView("_ViewSchoolTemplate");
		}

		public ActionResult _AddTemplate()
		{
			return PartialView("_AddTemplate");
		}
	}
}