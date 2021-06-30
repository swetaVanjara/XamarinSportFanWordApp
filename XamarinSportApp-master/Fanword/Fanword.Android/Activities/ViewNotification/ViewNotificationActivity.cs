
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mobile.Extensions.Android.Extensions;

namespace Fanword.Android.Activities.ViewNotification
{
    [Activity(Label = "ViewNotificationActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ViewNotificationActivity : BaseActivity
    {
        private ImageButton btnBack { get; set; }
        private TextView lblTitle { get; set; }
        private TextView lblMessage { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
			RequestWindowFeature(WindowFeatures.NoTitle);
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.ViewNotificationLayout);
			this.PopulateViewProperties();
			SetupViewBindings();
        }

        void SetupViewBindings()
        {
            btnBack.Click += (sender, e) => Finish();
            lblTitle.Text = Intent.GetStringExtra("Title");
            lblMessage.Text = Intent.GetStringExtra("Message");
        }
    }
}
