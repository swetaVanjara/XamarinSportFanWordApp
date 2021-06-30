using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Fanword.Android.Activities.Followers;
using Fanword.Android.CustomViews;
using Fanword.Android.Shared;
using Fanword.Android.TypeFaces;
using Fanword.Poco.Models;
using Fanword.Shared;
using Fanword.Shared.Helpers;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Views;
using Mobile.Extensions.Android.Extensions;
using Mobile.Extensions.Extensions;
using Plugin.Settings;

namespace Fanword.Android.Activities.UserProfile
{
    [Activity(Label = "UserProfileActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class UserProfileActivity : BaseActivity
    {
        private SwipeRefreshLayout slRefresh { get; set; }
        private FeedRecyclerView rvFeed { get; set; }
        private ImageButton btnBack { get; set; }
        private TextView lblTitle { get; set; }
        private TextView lblPosts;
        private TextView lblFollowers;
        private TextView lblAthlete;
        private TextView lblName;
        private ImageViewAsync imgProfile;
        private Button btnFollow;
        private string userId;
        private bool isFollowing;
        Fanword.Poco.Models.UserProfile profile;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.UserProfileActivityLayout);
            this.PopulateViewProperties();

            SetupViewBindings();
        }

        void SetupViewBindings()
        {
            userId = Intent.GetStringExtra("UserId");
            btnBack.Click += (sender, args) => Finish();

            ShowHelpIfNecessary(TutorialHelper.Profiles);

            var headerView = LayoutInflater.Inflate(Resource.Layout.UserProfileLayout, null);

            lblName = headerView.FindViewById<TextView>(Resource.Id.lblName);
            lblAthlete = headerView.FindViewById<TextView>(Resource.Id.lblAthlete);
            imgProfile = headerView.FindViewById<ImageViewAsync>(Resource.Id.imgProfile);
            btnFollow = headerView.FindViewById<Button>(Resource.Id.btnFollow);
            rvFeed.Initialize(this, headerView, userId, FeedType.User);
            rvFeed.SwipeContainer = slRefresh;
            rvFeed.GetNewsFeedItems(true);


            slRefresh.Refresh += (sender, args) =>
            {
                rvFeed.GetNewsFeedItems(true);
                GetData();
            };

            lblPosts = headerView.FindViewById<TextView>(Resource.Id.lblPosts);
            lblFollowers = headerView.FindViewById<TextView>(Resource.Id.lblFollowers);

			lblPosts.Typeface = CustomTypefaces.RobotoBold;
			lblFollowers.Typeface = CustomTypefaces.RobotoBold;
			lblTitle.Typeface = CustomTypefaces.RobotoBold;

            btnFollow.Click += (sender, args) =>
			{
                if (profile == null)
                    return;
                
				Fanword.Android.Shared.Follower.FollowToggle(this, btnFollow, profile, profile.UserId, FeedType.User, (following) =>
				{
					GetData();
				});
			};

            (lblFollowers.Parent as ViewGroup).Click += (sender, args) =>
            {
                if (profile == null)
                    return;
                Intent intent = new Intent(this, typeof(FollowersActivity));
                intent.PutExtra("Id", profile.UserId);
                intent.PutExtra("Type", (int) FeedType.User);
                StartActivity(intent);
            };
        }

        protected override void OnResume()
        {
            base.OnResume();
            GetData();
			if (!string.IsNullOrEmpty(MainActivity.PostId))
			{
				rvFeed.UpdateFeedItem(MainActivity.PostId);
				MainActivity.PostId = null;
			}
        }

        void GetData()
        {
            var apiTask = new ServiceApi().GetUserProfile(userId);
            apiTask.HandleError(this);
            apiTask.OnSucess(this, response =>
            {
                this.profile = response.Result;
                lblTitle.Text = response.Result.Name;
                lblName.Text = response.Result.Name;
                lblPosts.Text = LargeValueHelper.GetString(response.Result.Posts);
                lblFollowers.Text = LargeValueHelper.GetString(response.Result.Followers);
                lblAthlete.Text = response.Result.Athlete;
                isFollowing = response.Result.IsFollowing;
                Views.SetFollowed(btnFollow, isFollowing);
                if (string.IsNullOrEmpty(lblAthlete.Text))
                {
                    lblAthlete.Visibility = ViewStates.Gone;
                }
                
                if (!string.IsNullOrEmpty(response.Result.ProfileUrl))
                {
                    ImageService.Instance.LoadUrl(response.Result.ProfileUrl).Transform(new CircleTransformation()).Retry(3, 300).Into(imgProfile);
                }

                var apiTask2 = new ServiceApi().GetUser(userId);
                apiTask2.HandleError(this);
                apiTask2.OnSucess(this, response2 => {
                    if (!response2.Result.AthleteVerified) {
                        lblAthlete.Text = "";
                        lblAthlete.Visibility = ViewStates.Gone;
                    }
                });
            });

        }
    }
}