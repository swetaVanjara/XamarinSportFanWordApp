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
    public class DateViewHolder : global::Android.Support.V7.Widget.RecyclerView.ViewHolder
    {
        public TextView lblDate { get; set; }
        public DateViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public DateViewHolder(View itemView, Action<int> OnClick = null) : base(itemView)
        {
            // Locate and cache view references:
            itemView.Click += (sender, e) => OnClick?.Invoke(AdapterPosition);
            ItemView = itemView;
            ItemView.LayoutParameters = new ViewGroup.LayoutParams((int)(Application.Context.Resources.DisplayMetrics.WidthPixels / 5f), ViewGroup.LayoutParams.MatchParent);

            lblDate = itemView.FindViewById<TextView>(Resource.Id.lblDate);

            ActivityExtensions.SetViewFont(itemView as ViewGroup);
        }
    }
}