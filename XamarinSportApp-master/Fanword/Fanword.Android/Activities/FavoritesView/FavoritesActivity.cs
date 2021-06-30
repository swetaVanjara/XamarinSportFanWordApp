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
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Fanword.Android.Activities.Search;
using Fanword.Android.Adapters;
using Fanword.Android.Fragments;
using Fanword.Android.TypeFaces;
using Fanword.Shared;
using Mobile.Extensions.Android.Extensions;

namespace Fanword.Android.Activities.FavoritesView
{
    [Activity(Label = "FavoritesActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class FavoritesActivity : BaseActivity
    {
        private ImageButton btnBack { get; set; }
        private ImageButton btnAdd { get; set; }
        private ViewPager vpPager { get; set; }
        private List<Fragment> fragments;
        private Button btnTeams { get; set; }
        private Button btnSports { get; set; }
        private Button btnSchools { get; set; }
        private Button btnContentSources { get; set; }
        private Button btnUsers { get; set; }
        private TextView lblTitle { get; set; }

        private RelativeLayout rlIndicator { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FavoritesLayout);
            this.PopulateViewProperties();
            SetupViewBindings();
        }

        void SetupViewBindings()
        {
            btnBack.Click += (sender, e) =>
            {
                Finish();
            };

            lblTitle.Typeface = CustomTypefaces.RobotoBold;

            fragments = new List<Fragment>();
            fragments.Add(new FavoriteFragment());
            fragments.Add(new FavoriteFragment());
            fragments.Add(new FavoriteFragment());
            fragments.Add(new FavoriteFragment());
            fragments.Add(new FavoriteUsersFragment());

            vpPager.Adapter = new FragmentViewPagerAdapter(FragmentManager, fragments);
            btnAdd.Click += (sender, args) => StartActivity(typeof(SearchActivity));

            btnTeams.Click += (sender, args) =>
            {
                vpPager.CurrentItem = 0;
            };
            btnSchools.Click += (sender, args) =>
            {
                vpPager.CurrentItem = 1;
            };
            btnSports.Click += (sender, args) =>
            {
                vpPager.CurrentItem = 2;
            };
            btnContentSources.Click += (sender, args) =>
            {
                vpPager.CurrentItem = 3;
            };
            btnUsers.Click += (sender, args) =>
            {
                vpPager.CurrentItem = 4;
            };

            Display display = WindowManager.DefaultDisplay;
            Point size = new Point();
            display.GetSize(size);
            var screenWidth = size.X;

            var parameters = new RelativeLayout.LayoutParams((int)(90 * Resources.DisplayMetrics.Density), (int)(2 * Resources.DisplayMetrics.Density));
            parameters.AddRule(LayoutRules.AlignParentBottom);
            rlIndicator.LayoutParameters = parameters;
            vpPager.PageScrolled += (sender, args) =>
            {
                var margins = rlIndicator.LayoutParameters as RelativeLayout.MarginLayoutParams;
                margins.LeftMargin = (int)((args.Position) * (90 * Resources.DisplayMetrics.Density) + ((args.PositionOffsetPixels / (float)screenWidth) * (90 * Resources.DisplayMetrics.Density)));
                rlIndicator.LayoutParameters = margins;

                rlIndicator.LayoutParameters = margins;

                int position = args.PositionOffsetPixels > screenWidth / 2 ? args.Position + 1 : args.Position;
                
            };

            GetData();

            if(Intent.GetStringExtra("Fragment") == "Following")
            {
                vpPager.CurrentItem = 4;
            }
            else if(Intent.GetStringExtra("Fragment") == "Followers")
            {
                vpPager.CurrentItem = 4;
            }

        }

        void GetData()
        {
            ShowProgressDialog();
            var apiTask = new ServiceApi().Favorites();
            apiTask.HandleError(this);
            apiTask.OnSucess(this, (response) =>
            {
                HideProgressDialog();
                (fragments[0] as FavoriteFragment).SetData(response.Result.Teams);
                (fragments[1] as FavoriteFragment).SetData(response.Result.Schools);
                (fragments[2] as FavoriteFragment).SetData(response.Result.Sports);
                (fragments[3] as FavoriteFragment).SetData(response.Result.ContentSources);
                (fragments[4] as FavoriteUsersFragment).SetData(response.Result.Following, response.Result.Followers);
                if (Intent.GetStringExtra("Fragment") == "Followers")
				{
					(fragments[4] as FavoriteUsersFragment).SetFollowers();
				}
            });
        }
    }
}