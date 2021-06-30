using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FFImageLoading.Views;

namespace Fanword.Android.ViewHolders
{
    public interface IFeedCell
    {
        View ItemView { get; set; }
        ImageButton btnLike { get; set; }
        ImageViewAsync imgProfile { get; set; }
        TextView lblName { get; set; }
        ImageButton btnOptions { get; set; }
        TextView lblLikes { get; set; }
        ImageButton btnComment { get; set; }
        ImageButton btnShare { get; set; }
        TextView lblContent { get; set; }
        TextView lblTimeAgo { get; set; }
        TextView lblComments { get; set; }
        TextView lblShares { get; set; }
        ImageLoaderHelper ProfileTask { get; set; }
        TextView lblTags { get; set; }
        ImageLoaderHelper ImageTask { get; set; }
        RelativeLayout rlMedia { get; set; }
        TextView lblLinkHost { get; set; }
        TextView lblLinkTitle { get; set; }
        LinearLayout llLinkDetails { get; set; }
        ImageViewAsync imgImage { get; set; }
        ImageButton btnPlay { get; set; }
        ImageButton btnFacebook { get; set; }
        ImageButton btnTwitter { get; set; }
        ImageButton btnInstagram { get; set; }
        LinearLayout llSharePost { get; set; }
        TextView lblSharedFrom { get; set; }
    }
}