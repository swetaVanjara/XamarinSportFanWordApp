    // This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;
using Fanword.Poco.Models;

namespace Fanword.iOS
{
	public partial class ContentSourceAboutViewController : UIViewController
	{
        ContentSourceProfile profile;
		public ContentSourceAboutViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            btnTwitter.Superview.Hidden = true;
            btnFacebook.Superview.Hidden = true;
            btnInstagram.Superview.Hidden = true;

			btnTwitter.TouchUpInside += (sender, e) => OpenUrl(profile?.TwitterLink);
			btnFacebook.TouchUpInside += (sender, e) => OpenUrl(profile?.FacebookLink);
			btnInstagram.TouchUpInside += (sender, e) => OpenUrl(profile?.InstagramLink);
			btnWebsite.TouchUpInside += (sender, e) => OpenUrl(profile?.WebsiteLink);
        }

        public void SetView(ContentSourceProfile profile)
        {
            this.profile = profile;
    
            if(!string.IsNullOrEmpty(profile.TwitterLink))
            {
                btnTwitter.Superview.Hidden = false;
            }
			if (!string.IsNullOrEmpty(profile.FacebookLink))
			{
                btnFacebook.Superview.Hidden = false;
			}
			if (!string.IsNullOrEmpty(profile.InstagramLink))
			{
                btnInstagram.Superview.Hidden = false;
			}
        }

        void OpenUrl(string link)
        {
            if (string.IsNullOrEmpty(link))
                return;
            try
            {
				if (!link.ToLower().StartsWith("http"))
				{
					link = "http://" + link;
				}
                UIApplication.SharedApplication.OpenUrl(new NSUrl(link));
            }
            catch { }
        }
	}
}