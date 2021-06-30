using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fanword.Android.Shared;
using Fanword.Poco.Models;
using Mobile.Extensions.Android.Extensions;

namespace Fanword.Android.Fragments
{
    public class ContentSourceAboutFragment : BaseFragment
    {
        private Button btnWebsite { get; set; }
        private Button btnFacebook { get; set; }
        private Button btnInstagram { get; set; }
        private Button btnTwitter { get; set; }
        ContentSourceProfile profile;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ContentSourceAboutFragment, null);
            this.PopulateViewProperties(view);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            (btnFacebook.Parent.Parent as ViewGroup).Visibility = ViewStates.Gone;
            (btnInstagram.Parent.Parent as ViewGroup).Visibility = ViewStates.Gone;
            (btnTwitter.Parent.Parent as ViewGroup).Visibility = ViewStates.Gone;

			btnWebsite.Click += (sender, e) => Links.OpenUrl(profile?.WebsiteLink);
			btnFacebook.Click += (sender, e) => Links.OpenUrl(profile?.FacebookLink);
			btnTwitter.Click += (sender, e) => Links.OpenUrl(profile?.TwitterLink);
			btnInstagram.Click += (sender, e) => Links.OpenUrl(profile?.InstagramLink);

        }

        public void SetData(ContentSourceProfile profile)
        {
            this.profile = profile;

            if (!string.IsNullOrEmpty(profile.FacebookLink))
            {
                (btnFacebook.Parent.Parent as ViewGroup).Visibility = ViewStates.Visible;
            }
            if (!string.IsNullOrEmpty(profile.TwitterLink))
            {
                (btnTwitter.Parent.Parent as ViewGroup).Visibility = ViewStates.Visible;
            }
            if (!string.IsNullOrEmpty(profile.InstagramLink))
            {
                (btnInstagram.Parent.Parent as ViewGroup).Visibility = ViewStates.Visible;
            }
        }
    }
}