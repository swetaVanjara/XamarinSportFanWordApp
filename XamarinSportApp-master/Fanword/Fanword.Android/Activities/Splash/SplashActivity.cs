
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fanword.Shared;
using Mobile.Extensions.Android.Extensions;

namespace Fanword.Android
{
	[Activity (Label = "Fanword", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, Theme = "@android:style/Theme.NoTitleBar", NoHistory = true)]
	public class SplashActivity : BaseActivity
	{
        private ImageView mFanword;

		protected override async void OnCreate (Bundle savedInstanceState)
		{
		    RequestWindowFeature(WindowFeatures.NoTitle);
			base.OnCreate (savedInstanceState);
            SetContentView(Resource.Layout.SplashLayout);
            mFanword = (ImageView) FindViewById(Resource.Id.imageView1);
            this.PopulateViewProperties();
		    await Task.Delay(1000);
		    if (LocalStorage.IsLoggedIn())
		    {
                StartActivity((typeof(MainActivity)));
		    }
		    else
		    {
                StartActivity(typeof(SplashLoginActivity));
                Finish();
            }
        }
	}
}
