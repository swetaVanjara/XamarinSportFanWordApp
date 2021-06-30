using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fanword.Web.Controllers
{
	public class AdvertiserRegistrationController : BaseMvcController
	{
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