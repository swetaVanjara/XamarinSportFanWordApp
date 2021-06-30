
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
using Fanword.Android.TypeFaces;
using Fanword.Shared.Helpers;
using Fanword.Shared.Service;
using Mobile.Extensions.Android.Extensions;

namespace Fanword.Android.Activities.ContentSourceInfo
{
    [Activity(Label = "ContentSourceInfoActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ContentSourceInfoActivity : BaseActivity
    {
		private ImageButton btnBack { get; set; }
		private Button btnNext { get; set; }
		private Button btnContact { get; set; }
		private TextView lblContent { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
			RequestWindowFeature(WindowFeatures.NoTitle);
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.ContentSourceInfoLayout);
			this.PopulateViewProperties();
			SetupViewBindings();
        }


		void SetupViewBindings()
		{
			ShowHelpIfNecessary(TutorialHelper.ContentSourceCreation);

			var parent = Window.DecorView.FindViewById(global::Android.Resource.Id.Content);
			SetViewBolds(parent as ViewGroup);

			btnContact.Click += (sender, e) =>
			{
				Intent intent = new Intent(Intent.ActionSendto, global::Android.Net.Uri.FromParts("mailto", FanwordConstants.Email, null));
				intent.PutExtra(Intent.ExtraEmail, new string[] {FanwordConstants.Email});
			    intent.PutExtra(Intent.ExtraSubject, "Fanword");
				StartActivity(intent);
			};

			btnBack.Click += (sender, e) => Finish();
			btnNext.Click += (sender, e) => NextClicked();

			lblContent.Text = @"If you wish to create a Content Source, you will be able to do the following / unlock the following features: (1) Adding content to any profile on FanWord (except other users), (2) Add a description on your profile, (3) Attach social media accounts to your posts, (4) Select a color theme, (5) Implement an RSS Feed, (6) Integrate an “Action Button” on your profile such as “Sign Up”, “Subscribe, “Visit Website”, etc., (7) Receive basic monthly data reports, (8) Send notifications to all of your followers, and (9) $50 in Advertising credit.

You do not have to pick a pricing option right now. Simply select your preferred pricing option once we contact you after your request has been submitted and approved. Billing will then occur via monthly invoices.

This page can be viewed again by selecting the tab for “Create a Content Source”.
";
		}

		void NextClicked()
		{
			Intent intent = new Intent(Intent.ActionView);
			intent.SetData(global::Android.Net.Uri.Parse(ServiceApiBase.MvcPortalURL + "/Registration/ContentSources"));
			StartActivity(intent);
		}

		public static void SetViewBolds(ViewGroup viewGroup)
		{

			for (int i = 0; i < viewGroup.ChildCount; i++)
			{
				var view = viewGroup.GetChildAt(i);
				if (view as ViewGroup != null)
				{
					SetViewBolds(view as ViewGroup);
				}
				else
				{
					if (view is TextView && view.Tag?.ToString() == "bold")
					{
						(view as TextView).Typeface = CustomTypefaces.RobotoBold;
					}
				}
			}
		}
    }
}
