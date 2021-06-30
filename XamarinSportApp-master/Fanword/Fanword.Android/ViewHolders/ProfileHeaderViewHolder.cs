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

namespace Fanword.Android.ViewHolders
{
    public class ProfileHeaderViewHolder : global::Android.Support.V7.Widget.RecyclerView.ViewHolder
    {
        public ProfileHeaderViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference,
            transfer)
        {
        }

        public ProfileHeaderViewHolder(View itemView, Action<int> OnClick) : base(itemView)
        {
            ItemView.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

        }

    }
}