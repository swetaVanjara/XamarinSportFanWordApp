
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
using Fanword.Poco.Models;
using Fanword.Shared.Helpers;
using Fanword.Shared;
using Plugin.Dialog;
using System.Threading.Tasks;
using Fanword.Android.TypeFaces;

namespace Fanword.Android.Activities.AdminInfo
{
    [Activity(Label = "AdminInfoActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class AdminInfoActivity : BaseActivity
    {
        private ImageButton btnBack { get; set; }
        private Button btnNext { get; set; } 
        private Button btnContact { get; set; }
        private TextView lblContent { get; set; }
        string Id;
        FeedType Type;
        bool IsAdmin;
        protected override void OnCreate(Bundle savedInstanceState)
        {
			RequestWindowFeature(WindowFeatures.NoTitle);
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.AdminInfoLayout);
			this.PopulateViewProperties();
			SetupViewBindings();
        }

        void SetupViewBindings()
        {
            Id = Intent.GetStringExtra("Id");
            Type = (FeedType)Intent.GetIntExtra("Type", 0);
            IsAdmin = Intent.GetBooleanExtra("IsAdmin", false);

            ShowHelpIfNecessary(TutorialHelper.BecomeAdmin);

            if(IsAdmin)
            {
                (btnNext.Parent as ViewGroup).Visibility = ViewStates.Gone;
            }
			var parent = Window.DecorView.FindViewById(global::Android.Resource.Id.Content);
            SetViewBolds(parent as ViewGroup);

            btnContact.Click += (sender, e) => 
            {
                Intent intent = new Intent(Intent.ActionSendto, global::Android.Net.Uri.FromParts("mailto", FanwordConstants.Email, null));
                intent.PutExtra(Intent.ExtraEmail, new string[] { FanwordConstants.Email });
                intent.PutExtra(Intent.ExtraSubject, "Fanword");
                StartActivity(intent);
            };

            btnBack.Click += (sender, e) => Finish();
            btnNext.Click += (sender, e) => NextClicked();

			lblContent.Text = @"If you wish to become an Admin and upgrade this profile to be a premium profile, you will be able to do the following / unlock the following features: (1) Post as the “Official [Team Name]“, (2) Attach social media accounts to your posts, (3) Implement an RSS feed, (4) Ticket links for your events (if available) will automatically appear next to the events involving your team, (5) Receive basic monthly data reports, (6) Send notifications to all of your followers, (7) Receive $50 in Advertising credit, and (8) $50 in Content Source credit.

You do not have to pick a pricing option right now. Simply select your preferred pricing option once we contact you after your request has been submitted and approved. Billing will then occur via monthly invoices.

This page can be viewed again by selecting the header for becoming an admin of this profile.
";
        }

        void NextClicked()
        {
			if (!IsAdmin)
			{
				ShowProgressDialog();
                Task apiTask = null;

                if(Type == FeedType.School)
				    apiTask = new ServiceApi().RequestSchoolAdmin(Id);
                else
                    apiTask = new ServiceApi().RequestTeamAdmin(Id);  
                
				apiTask.HandleError(this);
				apiTask.OnSucess(this, response =>
				{
					HideProgressDialog();
					CrossDialog.Current.Show("Request Sent", "You must be approved by an administrator. Check back later to see if you've been approved.");
				});
			}
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
                    if(view is TextView && view.Tag?.ToString() == "bold")
                    {
                        (view as TextView).Typeface = CustomTypefaces.RobotoBold;
                    }
				}
			}
		}
    }
}
