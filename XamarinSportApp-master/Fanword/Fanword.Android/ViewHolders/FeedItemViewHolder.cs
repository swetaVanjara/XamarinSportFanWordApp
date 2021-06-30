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
using Fanword.Android.TypeFaces;
using FFImageLoading.Views;
using Mobile.Extensions.Android.Extensions;

namespace Fanword.Android.ViewHolders
{
    public class FeedItemViewHolder : global::Android.Support.V7.Widget.RecyclerView.ViewHolder, IFeedCell
    {
        public ImageButton btnLike { get; set; }
        public ImageViewAsync imgProfile { get; set; }
        public TextView lblName { get; set; }
        public ImageButton btnOptions { get; set; }
        public TextView lblLikes { get; set; }
        public ImageButton btnComment { get; set; }
        public ImageButton btnShare { get; set; }
        public TextView lblContent { get; set; }
        public TextView lblTimeAgo { get; set; }
        public TextView lblComments { get; set; }
        public TextView lblShares { get; set; }
        public ImageLoaderHelper ProfileTask { get; set; }
        public TextView lblTags { get; set; }
        public TextView lblLinkHost { get; set; }
        public TextView lblLinkTitle { get; set; }
        public ImageLoaderHelper ImageTask { get; set; }
        public RelativeLayout rlMedia { get; set; }
        public LinearLayout llLinkDetails { get; set; }
        public ImageViewAsync imgImage { get; set; }
        public ImageButton btnPlay { get; set; }
        public ImageButton btnFacebook { get; set; }
        public ImageButton btnTwitter { get; set; }
        public ImageButton btnInstagram { get; set; }
        public ImageButton btnTag { get; set; }
        public LinearLayout llSharePost { get; set; }
        public TextView lblSharedFrom { get; set; }

        public FeedItemViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public FeedItemViewHolder(View itemView, Action<int> OnClick) : base(itemView)
        {
            // Locate and cache view references:
            itemView.Click += (sender, e) => OnClick(AdapterPosition);
            ItemView = itemView;
            ItemView.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            btnLike = itemView.FindViewById<ImageButton>(Resource.Id.btnLike);
            imgProfile = itemView.FindViewById<ImageViewAsync>(Resource.Id.imgProfile);
            lblName = itemView.FindViewById<TextView>(Resource.Id.lblName);
            btnOptions = itemView.FindViewById<ImageButton>(Resource.Id.btnOptions);
            lblLikes = itemView.FindViewById<TextView>(Resource.Id.lblLikes);
            btnComment = itemView.FindViewById<ImageButton>(Resource.Id.btnComment);
            btnTag = itemView.FindViewById<ImageButton>(Resource.Id.btnTag);
            btnShare = itemView.FindViewById<ImageButton>(Resource.Id.btnShare);
            lblContent = itemView.FindViewById<TextView>(Resource.Id.lblContent);
            lblTimeAgo = itemView.FindViewById<TextView>(Resource.Id.lblTimeAgo);
            lblComments = itemView.FindViewById<TextView>(Resource.Id.lblComments);
            lblShares = itemView.FindViewById<TextView>(Resource.Id.lblShares);
            lblTags = itemView.FindViewById<TextView>(Resource.Id.lblTags);
            lblLinkHost = itemView.FindViewById<TextView>(Resource.Id.lblLinkHost);
            lblLinkTitle = itemView.FindViewById<TextView>(Resource.Id.lblLinkTitle);
            imgImage = itemView.FindViewById<ImageViewAsync>(Resource.Id.imgImage);
            btnPlay = itemView.FindViewById<ImageButton>(Resource.Id.btnPlay);
            llLinkDetails = itemView.FindViewById<LinearLayout>(Resource.Id.llLinkDetails);
            rlMedia = itemView.FindViewById<RelativeLayout>(Resource.Id.rlMedia);
            btnFacebook = itemView.FindViewById<ImageButton>(Resource.Id.btnFacebook);
            btnTwitter = itemView.FindViewById<ImageButton>(Resource.Id.btnTwitter);
            btnInstagram = itemView.FindViewById<ImageButton>(Resource.Id.btnInstagram);
            llSharePost = itemView.FindViewById<LinearLayout>(Resource.Id.linearLayout0);
            lblSharedFrom = itemView.FindViewById<TextView>(Resource.Id.lblNameShares);

            var parameters = rlMedia.LayoutParameters as LinearLayout.LayoutParams;
            var height = Application.Context.Resources.DisplayMetrics.WidthPixels * 9 / 16f;
            parameters.Height = (int)height;
            rlMedia.LayoutParameters = parameters;

            lblName.Typeface = CustomTypefaces.RobotoBold;
            ActivityExtensions.SetViewFont(itemView as ViewGroup);
        }
    }
}