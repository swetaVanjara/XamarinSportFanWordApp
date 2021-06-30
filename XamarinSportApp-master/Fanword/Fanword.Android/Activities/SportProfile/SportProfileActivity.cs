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
using Fanword.Android.Adapters;
using Fanword.Android.Fragments;
using Fanword.Shared;
using Fanword.Shared.Helpers;
using Mobile.Extensions.Android.Extensions;

namespace Fanword.Android.Activities.SportProfile
{
    [Activity(Label = "SportProfileActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SportProfileActivity : BaseActivity
    {
        private ImageButton btnBack { get; set; }
        private TextView lblTitle { get; set; }
        private TextView lblSport { get; set; }
        private ViewPager vpPager { get; set; }
        private Button btnProfile { get; set; }
        private Button btnEvents { get; set; }
        private Button btnRankings { get; set; }
        private Button btnAbout { get; set; }
        private string sportId { get; set; }
        private List<Fragment> fragments;
        private RelativeLayout rlIndicator { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SportProfileActivity);
            this.PopulateViewProperties();
            SetupViewBindings();
        }

        void SetupViewBindings()
        {
            btnBack.Click += (sender, e) =>
            {
                Finish();
            };

            ShowHelpIfNecessary(TutorialHelper.Profiles);

            sportId = Intent.GetStringExtra("SportId");

            fragments = new List<Fragment>();
            fragments.Add(new SportProfileFragment() { SportId = sportId });
            fragments.Add(new ScoresFragment() { SportId = sportId });
            fragments.Add(new SportRankingsFragment() { SportId = sportId });
            fragments.Add(new SportAboutFragment() { SportId = sportId});

            vpPager.Adapter = new FragmentViewPagerAdapter(FragmentManager, fragments);

            btnProfile.Click += (sender, args) =>
            {
                vpPager.SetCurrentItem(0, true);
            };
            btnEvents.Click += (sender, args) =>
            {
                vpPager.SetCurrentItem(1, true);
            };
            btnRankings.Click += (sender, args) =>
            {
                vpPager.SetCurrentItem(2, true);
            };

            btnAbout.Click += (sender, args) =>
            {
                vpPager.SetCurrentItem(3, true);
            };

            Display display = WindowManager.DefaultDisplay;
            Point size = new Point();
            display.GetSize(size);
            var screenWidth = size.X;

            var parameters = new RelativeLayout.LayoutParams((int)(screenWidth / (float)fragments.Count), (int)(2 * Resources.DisplayMetrics.Density));
            parameters.AddRule(LayoutRules.AlignParentBottom);
            rlIndicator.LayoutParameters = parameters;
            vpPager.OffscreenPageLimit = 4;
            vpPager.PageScrolled += (sender, args) =>
            {
                var margins = rlIndicator.LayoutParameters as RelativeLayout.MarginLayoutParams;
                margins.LeftMargin = (int)((args.Position) * (screenWidth / (float)fragments.Count) + (args.PositionOffsetPixels / (float)fragments.Count));
                rlIndicator.LayoutParameters = margins;

                rlIndicator.LayoutParameters = margins;

                int position = args.PositionOffsetPixels > screenWidth / 2 ? args.Position + 1 : args.Position;
				if (position == 1)
				{
					ShowHelpIfNecessary(TutorialHelper.SchedulesScores);
				}

            };
            if (Intent.GetBooleanExtra("GoToRankings", false))
            {
                vpPager.SetCurrentItem(2, true);
            }
            GetData();
        }

        public void GetData()
        {
            var apiTask = new ServiceApi().GetSportProfile(sportId);
            apiTask.HandleError(this);
            apiTask.OnSucess(this, response =>
            {
                lblTitle.Text = response.Result.Name;
                (fragments[0] as SportProfileFragment).SetData(response.Result);
                //(Activity as ContentSourceActivity)?.DataFectched(response.Result);

            });
        }
    }
}