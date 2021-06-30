
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using HockeyApp.Android;
using Mobile.Extensions.Android.Activities;
using Mobile.Extensions.Android.Extensions;
using Plugin.Settings;
using Fanword.Shared.Helpers;
using Fanword.Android.TypeFaces;
using Android.Text;
using Fanword.Android.Fragments;
using Android.Text.Style;
using Fanword.Android.Activities.Search;

namespace Fanword.Android
{
	[Activity (Label = "BaseActivity")]
	public class BaseActivity : ExtensionsBaseActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
            ActivityExtensions.CustomTypeface = ActivityExtensions.CustomTypeface ?? Typeface.CreateFromAsset(Application.Context.Assets, "Roboto-Regular.ttf");
            base.OnCreate (savedInstanceState);
		    ImageService.Instance.Config.SchedulerMaxParallelTasks = 10;
            CrashManager.Register(this, "46de58f4a58f4f41a124f33d98c6f30d");
            // Create your application here

        }

        public void ShowHelpIfNecessary(TutorialHelper tutorial, Action dismissed = null)
        {
            if (CrossSettings.Current.GetValueOrDefault(tutorial.Id, false))
            {
                dismissed?.Invoke();
                return;
            }
            CrossSettings.Current.AddOrUpdateValue(tutorial.Id, true);
                
            
            View view = LayoutInflater.Inflate(Resource.Layout.TutorialLayout, null);
            ActivityExtensions.SetViewFont(view as ViewGroup);

            var lblTitle = view.FindViewById<TextView>(Resource.Id.lblTitle);
            var lblSubtitle = view.FindViewById<TextView>(Resource.Id.lblSubtitle);
            var imgImage = view.FindViewById<ImageView>(Resource.Id.imgImage);
            var btnOk = view.FindViewById<Button>(Resource.Id.btnOk);
            var id = Resources.GetIdentifier(tutorial.AndroidIcon.ToLower(), "drawable", PackageName);
            imgImage.SetImageResource(id);

            lblTitle.Text = tutorial.Title;

            btnOk.Typeface = CustomTypefaces.RobotoBold;
			lblTitle.Typeface = CustomTypefaces.RobotoBold;

            btnOk.Text = tutorial.ButtonText;

            if(tutorial.Id == "Welcome")
            {
				var span = new SpannableString(tutorial.Title);
				span.SetSpan(new FanwordTypefaceSpan(CustomTypefaces.RobotoBold), 8, tutorial.Title.Length, SpanTypes.ExclusiveExclusive);
                span.SetSpan(new ForegroundColorSpan(new Color(249,95,6)) , 8 ,tutorial.Title.Length, SpanTypes.ExclusiveExclusive);
				lblTitle.TextFormatted = span;
			}

            lblSubtitle.Text = tutorial.Subtitle;

            var dialog = new AlertDialog.Builder(this).SetView(view).Create();

            if(btnOk.Text == "Follow Profiles")
            {
                btnOk.Click += (sender, e) =>
                {
                    dialog.Dismiss();
                    StartActivity(typeof(SearchActivity));
                };
            }
            else
            {
                btnOk.Click += (sender, e) =>
                {
                    dialog.Dismiss();
                };
            }
            

            dialog.DismissEvent += (sender, e) => 
            {
                dismissed?.Invoke();
            };

            dialog.Show();

        }

	}
}
