using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fanword.Android.Activities.ContentSource;
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

namespace Fanword.Android.Fragments
{
    public class ContentSourceProfileFragment : BaseFragment
    {
        private SwipeRefreshLayout slRefresh { get; set; }
        private FeedRecyclerView rvFeed { get; set; }
        private TextView lblPosts;
        private TextView lblFollowers;
        private TextView lblName;
        private TextView lblDescription;
        private TextView lblUrl;
        private Button btnActionButton;
        private ImageViewAsync imgProfile;
        private Button btnFollow;
        public string ContentSourceId;
        private bool isFollowing;
        ContentSourceProfile profile;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ContentSourceProfileFragment, null);
            this.PopulateViewProperties(view);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            var headerView = Activity.LayoutInflater.Inflate(Resource.Layout.ContentSourceHeaderLayout, null);

            lblName = headerView.FindViewById<TextView>(Resource.Id.lblName);
            lblDescription = headerView.FindViewById<TextView>(Resource.Id.lblDescription);
            lblUrl = headerView.FindViewById<TextView>(Resource.Id.lblUrl);
            imgProfile = headerView.FindViewById<ImageViewAsync>(Resource.Id.imgProfile);
            btnFollow = headerView.FindViewById<Button>(Resource.Id.btnFollow);
            lblPosts = headerView.FindViewById<TextView>(Resource.Id.lblPosts);
            lblFollowers = headerView.FindViewById<TextView>(Resource.Id.lblFollowers);
            btnActionButton = headerView.FindViewById<Button>(Resource.Id.btnActionButton);

            lblPosts.Typeface = CustomTypefaces.RobotoBold;
            lblFollowers.Typeface = CustomTypefaces.RobotoBold;

            rvFeed.Initialize(Activity as BaseActivity, headerView, ContentSourceId, FeedType.ContentSource);
            rvFeed.SwipeContainer = slRefresh;
            rvFeed.GetNewsFeedItems(true);

            slRefresh.Refresh += (sender, args) =>
            {
                rvFeed.GetNewsFeedItems(true);
                GetData();
            };

            btnFollow.Click += (sender, args) =>
            {
                Fanword.Android.Shared.Follower.FollowToggle(ActivityProgresDialog, btnFollow, profile, profile.ContentSourceId, FeedType.ContentSource,(following) => 
                {
                    GetData();
                });
            };

            (lblFollowers.Parent as ViewGroup).Click += (sender, args) =>
            {
                if (profile == null)
                    return;
                Intent intent = new Intent(Activity, typeof(FollowersActivity));
                intent.PutExtra("Id", profile.ContentSourceId);
                intent.PutExtra("Type", (int)FeedType.ContentSource);
                StartActivity(intent);
            };

            GetData();

            btnActionButton.Click += (sender, args) =>
            {
                if (profile != null)
                {
                    Links.OpenUrl(profile.ActionButtonLink);
                }
            };

            lblUrl.Click += (sender, args) =>
            {
                Links.OpenUrl(profile?.WebsiteLink);
            };
        }

        public override void OnResume()
        {
            base.OnResume();
            if(!string.IsNullOrEmpty(MainActivity.PostId))
            {
                rvFeed.UpdateFeedItem(MainActivity.PostId);
                MainActivity.PostId = null;
            }
        }

        void GetData()
        {
            var apiTask = new ServiceApi().GetContentSourceProfile(ContentSourceId);
            apiTask.HandleError(ActivityProgresDialog);
            apiTask.OnSucess(ActivityProgresDialog, response =>
            {
                profile = response.Result;
                btnActionButton.Text = profile.ActionButtonText;
                if (!string.IsNullOrEmpty(response.Result.PrimaryColor))
                {
                    try
                    {
                        btnActionButton.SetTextColor(Color.ParseColor(response.Result.PrimaryColor));
                    }
                    catch
                    {
                    }
                }

                (Activity as ContentSourceActivity)?.DataFectched(response.Result);
                lblName.Text = response.Result.Name;
                lblPosts.Text = LargeValueHelper.GetString(response.Result.Posts);
                lblFollowers.Text = LargeValueHelper.GetString(response.Result.Followers);
                try
                {
                    if (!response.Result.WebsiteLink.StartsWith("http://") && !response.Result.WebsiteLink.StartsWith("https://"))
                    {
                        response.Result.WebsiteLink = "http://" + response.Result.WebsiteLink;
                    }
                    lblUrl.Text = new Uri(response.Result.WebsiteLink).Host;
                }
                catch (Exception e)
                {
                }
                lblDescription.Text = response.Result.Description;
                isFollowing = response.Result.IsFollowing;
                Views.SetFollowed(btnFollow, isFollowing);
                
                if (!string.IsNullOrEmpty(response.Result.ProfileUrl))
                {
                    ImageService.Instance.LoadUrl(response.Result.ProfileUrl).Transform(new CircleTransformation()).Retry(3, 300).Into(imgProfile);
                }
            });
        }
    }
}