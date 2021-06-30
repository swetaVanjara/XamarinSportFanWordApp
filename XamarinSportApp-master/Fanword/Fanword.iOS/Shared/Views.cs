using System;
using UIKit;
namespace Fanword.iOS.Shared
{
    public class Views
    {
        public static void SetFollowed(UIButton btnFollow, bool isFollowing)
        {
			if (isFollowing)
			{
				btnFollow.SetTitle("Following", UIControlState.Normal);
				btnFollow.Layer.BorderWidth = 0;
				btnFollow.SetTitleColor(UIColor.White, UIControlState.Normal);
				btnFollow.BackgroundColor = UIColor.FromRGB(249, 95, 6);
			}
			else
			{
				btnFollow.SetTitle("Follow", UIControlState.Normal);

				btnFollow.Layer.BorderWidth = 1;
				btnFollow.SetTitleColor(UIColor.FromRGB(144, 144, 144), UIControlState.Normal);
				btnFollow.Layer.BorderColor = UIColor.FromRGB(144, 144, 144).CGColor;
				btnFollow.Layer.BorderColor = UIColor.FromRGB(144, 144, 144).CGColor;
				btnFollow.BackgroundColor = UIColor.White;
			}
        }
    }
}
