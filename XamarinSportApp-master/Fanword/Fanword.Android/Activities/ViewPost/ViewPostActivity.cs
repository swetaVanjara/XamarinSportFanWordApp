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
using Fanword.Android.Shared;
using Fanword.Poco.Models;
using Mobile.Extensions.Android.Extensions;
using Mobile.Extensions.Extensions;
using Plugin.Settings;

namespace Fanword.Android.Activities.ViewPost
{
    [Activity(Label = "ViewPostActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ViewPostActivity : BaseActivity
    {
        private SwipeRefreshLayout slRefresh { get; set; }
        private FeedRecyclerView rvFeed { get; set; }
        private ImageButton btnBack { get; set; }

        private string PostId { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ViewPostLayout);
            this.PopulateViewProperties();
            PostId = Intent.GetStringExtra("PostId");
            SetupViewBindings();
        }

        void SetupViewBindings()
        {
            btnBack.Click += (sender, e) => Finish();
            rvFeed.Initialize(this, null, PostId, FeedType.SinglePost);
            rvFeed.SwipeContainer = slRefresh;
            rvFeed.GetNewsFeedItems(true);
            slRefresh.Refresh += (sender, args) => rvFeed.GetNewsFeedItems(true);
        }
    }
}