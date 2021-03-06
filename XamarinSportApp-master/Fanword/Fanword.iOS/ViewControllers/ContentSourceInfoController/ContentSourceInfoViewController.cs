// This file has been autogenerated from a class added in the UI designer.

using System;
using Fanword.Shared.Helpers;
using Fanword.Shared.Service;
using Foundation;
using UIKit;

namespace Fanword.iOS
{
	public partial class ContentSourceInfoViewController : BaseViewController
	{
		public ContentSourceInfoViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			ShowHelpIfNecessary(TutorialHelper.ContentSourceCreation);
			btnBack.TouchUpInside += (sender, e) => NavigationController.PopViewController(true);


			btnNext.TouchUpInside += (sender, e) => NextClicked();

			btnContact.TouchUpInside += (sender, e) =>
			{
				UIApplication.SharedApplication.OpenUrl(new NSUrl("mailto:" + FanwordConstants.Email));
			};
		}

		void NextClicked()
		{
			UIApplication.SharedApplication.OpenUrl(new NSUrl(ServiceApiBase.MvcPortalURL + "/Registration/ContentSources"));
		}
	}
}
