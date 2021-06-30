using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fanword.Web.Controllers
{
    [Authorize(Roles ="System_Admin, Advertiser,DataAdv")]
    public class CampaignsController : BaseMvcController
    {
        // GET: Campaigns
        public ActionResult Index()
        {
            return View();
        }
        

        public ActionResult _CampaignTemplate() {
            return PartialView("_CampaignTemplate");
        }

        public ActionResult _CampaignEditTemplate() {
            return PartialView("_CampaignEditTemplate");
        }

        public ActionResult _BreadcrumbTemplate() {
            return PartialView("_BreadcrumbTemplate");
        }
    }
}