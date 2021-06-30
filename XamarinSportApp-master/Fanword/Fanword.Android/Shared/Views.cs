using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Fanword.Android.Shared
{
    public class Views
    {
        public static void SetFollowed(Button btnFollow, bool isFollowing)
        {
            if (isFollowing)
            {
                btnFollow.SetTextColor(Color.White);
                btnFollow.Text = "Following";
                (btnFollow.Parent as RelativeLayout).Background = Application.Context.Resources.GetDrawable(Resource.Drawable.FollowingRoundedBackground);
            }
            else
            {
                btnFollow.SetTextColor(new Color(144, 144, 144));
                btnFollow.Text = "Follow";
                (btnFollow.Parent as RelativeLayout).Background = Application.Context.Resources.GetDrawable(Resource.Drawable.FollowRoundedBackground);
            }
        }
    }
}