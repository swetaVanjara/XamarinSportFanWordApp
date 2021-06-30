using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Fanword.Android.Activities.Followers;
using Fanword.Android.CustomViews;
using Fanword.Android.Shared;
using Fanword.Poco.Models;
using Fanword.Shared;
using Fanword.Shared.Helpers;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Views;
using Mobile.Extensions.Android.Extensions;
using Plugin.Dialog;
using Fanword.Android.Activities.SchoolProfile;
using Fanword.Android.TypeFaces;
using Plugin.Settings;
using Fanword.Android.Activities.AdminInfo;

namespace Fanword.Android.Fragments
{
    public class SchoolProfileFragment : BaseFragment
    {
        private SwipeRefreshLayout slRefresh { get; set; }
        private FeedRecyclerView rvFeed { get; set; }
        public string SchoolId { get; set; }
        private TextView lblPosts;
        private TextView lblFollowers;
        private TextView lblName;
        private ImageViewAsync imgProfile;
        private Button btnFollow;
        private bool isFollowing;
        private Button btnAdmin;
        SchoolProfile profile;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.TeamProfileFragment, null);
            this.PopulateViewProperties(view);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            var headerView = Activity.LayoutInflater.Inflate(Resource.Layout.SchoolProfileHeader, null);
            lblName = headerView.FindViewById<TextView>(Resource.Id.lblName);
            imgProfile = headerView.FindViewById<ImageViewAsync>(Resource.Id.imgProfile);
            btnFollow = headerView.FindViewById<Button>(Resource.Id.btnFollow);
            lblPosts = headerView.FindViewById<TextView>(Resource.Id.lblPosts);
            lblFollowers = headerView.FindViewById<TextView>(Resource.Id.lblFollowers);
            //btnAdmin = headerView.FindViewById<Button>(Resource.Id.btnAdmin);

            rvFeed.Initialize(Activity as BaseActivity, headerView, SchoolId, FeedType.School);
            rvFeed.SwipeContainer = slRefresh;
            rvFeed.GetNewsFeedItems(true);

            lblPosts.Typeface = CustomTypefaces.RobotoBold;
            lblFollowers.Typeface = CustomTypefaces.RobotoBold;

            slRefresh.Refresh += (sender, args) =>
            {
                rvFeed.GetNewsFeedItems(true);
                (Activity as SchoolProfileActivity).GetData();
            };

			btnFollow.Click += (sender, e) =>
			{
				if (profile == null)
					return;

			    Shared.Follower.FollowToggle(ActivityProgresDialog, btnFollow, profile, profile.Id, FeedType.School, (following) =>
				{
					(Activity as SchoolProfileActivity).GetData();
				});
			};

            (lblFollowers.Parent as ViewGroup).Click += (sender, args) =>
            {
                if (profile == null)
                    return;
                Intent intent = new Intent(Activity, typeof(FollowersActivity));
                intent.PutExtra("Id", profile.Id);
                intent.PutExtra("Type", (int)FeedType.School);
                StartActivity(intent);
            };
        }

        public void SetData(SchoolProfile profile)
        {
            this.profile = profile;
            lblName.Text = profile.Name;
            lblPosts.Text = LargeValueHelper.GetString(profile.Posts);
            lblPosts.Text = LargeValueHelper.GetString(profile.Posts);
            lblFollowers.Text = LargeValueHelper.GetString(profile.Followers);

            isFollowing = profile.IsFollowing;
            if (profile.IsProfileAdmin)
            {
                btnAdmin.Text = "You are an admin of this profile";
            }

            Views.SetFollowed(btnFollow, isFollowing);

            if (!string.IsNullOrEmpty(profile.ProfileUrl))
            {
                ImageService.Instance.LoadUrl(profile.ProfileUrl).Transform(new CircleTransformation()).Retry(3, 300)
                    .Into(imgProfile);
            }

            //btnAdmin.Click += (sender, args) =>
            //{
            //    if (profile == null)
            //        return;

            //    Intent intent = new Intent(Activity, typeof(AdminInfoActivity));
            //    intent.PutExtra("Id", SchoolId);
            //    intent.PutExtra("Type", (int)FeedType.School);
            //    intent.PutExtra("IsAdmin", profile.IsProfileAdmin);
            //    StartActivity(intent);
            //};
        }

		public override void OnResume()
		{
			base.OnResume();
			if (!string.IsNullOrEmpty(MainActivity.PostId))
			{
				rvFeed.UpdateFeedItem(MainActivity.PostId);
				MainActivity.PostId = null;
			}
		}
        
    }
}