using System;
using UIKit;
using Foundation;

namespace Fanword.iOS.Shared
{
    public class Links
    {
        public static void OpenUrl(string link)
        {
			try
			{
                var url = link;
				if (!url.ToLower().StartsWith("http"))
				{
					url = "http://" + url;
				}
				UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
			}
			catch { }
        }
    }
}
