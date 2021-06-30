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
    public class AdvertisementViewHolder : global::Android.Support.V7.Widget.RecyclerView.ViewHolder
    {
        public ImageViewAsync imgProfile { get; set; }
        public TextView lblName { get; set; }
        public ImageLoaderHelper ProfileTask { get; set; }
        public ImageLoaderHelper ImageTask { get; set; }
        public TextView lblContent { get; set; }

        public ImageViewAsync imgImage { get; set; }
        public AdvertisementViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public AdvertisementViewHolder(View itemView, Action<int> OnClick) : base(itemView)
        {
            
            ItemView.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            imgProfile = itemView.FindViewById<ImageViewAsync>(Resource.Id.imgProfile);
            lblName = itemView.FindViewById<TextView>(Resource.Id.lblName);
            lblContent = itemView.FindViewById<TextView>(Resource.Id.lblContent);
            imgImage = itemView.FindViewById<ImageViewAsync>(Resource.Id.imgImage);


            
            var parameters = imgImage.LayoutParameters as LinearLayout.LayoutParams;
            var height = Application.Context.Resources.DisplayMetrics.WidthPixels * 9 / 16f;
            parameters.Height = (int)height;
            imgImage.LayoutParameters = parameters;
        }
    }
}