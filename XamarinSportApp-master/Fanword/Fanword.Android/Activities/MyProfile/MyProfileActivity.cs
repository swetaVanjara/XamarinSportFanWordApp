using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Fanword.Android.CustomViews;
using Fanword.Poco.Models;
using Fanword.Shared;
using Fanword.Shared.Helpers;
using Fanword.Shared.Models;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Views;
using Mobile.Extensions.Android.Extensions;
using Mobile.Extensions.Extensions;
using Plugin.Settings;
using Fanword.Android.Activities.FavoritesView;
using Fanword.Android.TypeFaces;

namespace Fanword.Android.Activities.MyProfile
{
    [Activity(Label = "MyProfileActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MyProfileActivity : BaseActivity
    {
        private SwipeRefreshLayout slRefresh { get; set; }
        private FeedRecyclerView rvFeed { get; set; }
        private ImageButton btnBack { get; set; }
        private TextView lblTitle { get; set; }
        private TextView lblPosts;
        private TextView lblFollowers;
        private TextView lblFollowing;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MyProfileActivityLayout);
            this.PopulateViewProperties();

            SetupViewBindings();
        }

        void SetupViewBindings()
        {
            btnBack.Click += (sender, args) => Finish();
			ShowHelpIfNecessary(TutorialHelper.Profiles);

            var headerView = LayoutInflater.Inflate(Resource.Layout.MyProfileLayout, null);
            var user = CrossSettings.Current.GetValueOrDefaultJson<User>("User");
            headerView.FindViewById<TextView>(Resource.Id.lblName).Text = user.FirstName + " " + user.LastName;
            lblTitle.Text = user.FirstName + " " + user.LastName;
            var lblAthlete = headerView.FindViewById<TextView>(Resource.Id.lblAthlete);
            lblAthlete.Text = "";
            if (string.IsNullOrEmpty(lblAthlete.Text))
            {
                lblAthlete.Visibility = ViewStates.Gone;
            }

            if (!string.IsNullOrEmpty(user.ProfileUrl))
            {
                ImageService.Instance.LoadUrl(user.ProfileUrl).DownSample(300).Transform(new CircleTransformation()).Retry(3,300).Into(headerView.FindViewById<ImageViewAsync>(Resource.Id.imgProfile));
            }

            if (user.AthleteVerified) {
                lblAthlete.Visibility = string.IsNullOrEmpty(user.AthleteTeamId) ? ViewStates.Gone : ViewStates.Visible;
                lblAthlete.Text = user.AthleteSchool + " - " + user.AthleteSport;
            } else {
                lblAthlete.Text = "";
                lblAthlete.Visibility = ViewStates.Gone;
            }


			rvFeed.Initialize(this, headerView, CrossSettings.Current.GetValueOrDefaultJson<User>("User").Id, FeedType.MyProfile);
            rvFeed.SwipeContainer = slRefresh;
            rvFeed.GetNewsFeedItems(true);
            rvFeed.RefreshRequested += (sender, e) => GetProfileData();
            slRefresh.Refresh += (sender, args) => rvFeed.GetNewsFeedItems(true);

            lblPosts = headerView.FindViewById<TextView>(Resource.Id.lblPosts);
            lblFollowers = headerView.FindViewById<TextView>(Resource.Id.lblFollowers);
            lblFollowing = headerView.FindViewById<TextView>(Resource.Id.lblFollowing);

            (lblFollowing.Parent as ViewGroup).Click += (sender, e) => 
            {
                Intent intent = new Intent(this, typeof(FavoritesActivity));
                //intent.PutExtra("Fragment", "Following");
                StartActivity(intent);
            };
			(lblFollowers.Parent as ViewGroup).Click += (sender, e) =>
            {
				Intent intent = new Intent(this, typeof(FavoritesActivity));
				intent.PutExtra("Fragment", "Followers");
				StartActivity(intent);
			};

            lblPosts.Typeface = CustomTypefaces.RobotoBold;
            lblFollowers.Typeface = CustomTypefaces.RobotoBold;
            lblFollowing.Typeface = CustomTypefaces.RobotoBold;
            lblTitle.Typeface = CustomTypefaces.RobotoBold;
        }

        protected override void OnResume()
        {
            base.OnResume();
            GetProfileData();
			if (!string.IsNullOrEmpty(MainActivity.PostId))
			{
				rvFeed.UpdateFeedItem(MainActivity.PostId);
				MainActivity.PostId = null;
			}
        }

        void GetProfileData()
        {
			var apiTask = new ServiceApi().GetMyProfileDetails();
			apiTask.HandleError(this);
			apiTask.OnSucess(this, (response) =>
			{
				lblPosts.Text = LargeValueHelper.GetString(response.Result.Posts);
				lblFollowers.Text = LargeValueHelper.GetString(response.Result.Followers);
				lblFollowing.Text = LargeValueHelper.GetString(response.Result.Following);
			});
        }
    }
}