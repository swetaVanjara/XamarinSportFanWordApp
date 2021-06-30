using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileUploads.Azure;
using FileUploads.Azure.Models;

namespace Fanword.Web.Controllers {
    [Authorize]
    public class UploadsController : BaseMvcController {
        public ActionResult UploadSportIcon() {
            return Json(UploadToContainer("sporticons"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SchoolProfilePhoto() {
            return Json(UploadToContainer("schoolprofiles"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult TeamProfilePhoto() {
            return Json(UploadToContainer("teamprofiles"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserProfilePhoto() {
            return Json(UploadToContainer("userprofiles"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CampaignImage() {
            return Json(UploadToContainer("campaignimages"), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult UploadAdvertiserLogo() {
            return Json(UploadToContainer("advertisers"), JsonRequestBehavior.AllowGet);
        }
		[AllowAnonymous]
		public ActionResult UploadContentSourceLogo()
		{
			return Json(UploadToContainer("contentsources"), JsonRequestBehavior.AllowGet);
		}
        private AzureFileData UploadToContainer(string containerName) {
            var datas = new List<AzureFileData>();
            foreach (string fileName in Request.Files) {
                datas.Add(new AzureStorage(containerName, true).SaveFile(Request.Files[fileName]));
            }
            return datas.FirstOrDefault();
        }
    }
}