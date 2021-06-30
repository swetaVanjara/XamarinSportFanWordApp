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
using Fanword.Android.Activities.ContentSource;
using Fanword.Android.Activities.SchoolProfile;
using Fanword.Android.Adapters;
using Fanword.Android.Fragments;
using Fanword.Android.Shared;
using Fanword.Android.TypeFaces;
using Fanword.Shared;
using Fanword.Shared.Helpers;
using FFImageLoading;
using FFImageLoading.Transformations;
using Mobile.Extensions.Android.Extensions;

namespace Fanword.Android.Activities.TeamProfile
{
    [Activity(Label = "TeamProfileActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class TeamProfileActivity : BaseActivity
    {
        private ImageButton btnBack { get; set; }
        private TextView lblTitle { get; set; }
        private TextView lblSport { get; set; }
        private ViewPager vpPager { get; set; }
        private Button btnProfile { get; set; }
        private Button btnScores { get; set; }
        private Button btnRankings { get; set; }
        private Button btnAbout { get; set; }
        private string teamId { get; set; }
        private List<Fragment> fragments;
        private RelativeLayout rlIndicator { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TeamProfileActivityLayout);
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

            teamId = Intent.GetStringExtra("TeamId");
            fragments = new List<Fragment>();
            fragments.Add(new TeamProfileFragment() { TeamId = teamId });
            fragments.Add(new ScoresFragment() { TeamId = teamId });
            fragments.Add(new TeamRankingsFragment() { TeamId = teamId});
            fragments.Add(new TeamAboutFragment());
            vpPager.Adapter = new FragmentViewPagerAdapter(FragmentManager, fragments);

            lblTitle.Typeface = CustomTypefaces.RobotoBold;

            btnProfile.Click += (sender, args) =>
            {
                vpPager.SetCurrentItem(0, true);
            };
            btnScores.Click += (sender, args) =>
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
                if(position == 1)
                {
                    ShowHelpIfNecessary(TutorialHelper.SchedulesScores);
                }

            };
            GetData();
        }

        public void GetData()
        {
            var apiTask = new ServiceApi().GetTeamProfile(teamId);
            apiTask.HandleError(this);
            apiTask.OnSucess(this, response =>
            {
                lblSport.Text = response.Result.SportName;
                lblTitle.Text = response.Result.SchoolName;
                (fragments[0] as TeamProfileFragment).SetData(response.Result);
                (fragments[2] as TeamRankingsFragment).SetData(response.Result);
                (fragments[3] as TeamAboutFragment).SetData(response.Result);
                //(Activity as ContentSourceActivity)?.DataFectched(response.Result);
                lblTitle.Click += (sender, args) =>
                {
                    Intent intent = new Intent(this, typeof(SchoolProfileActivity));
                    intent.PutExtra("SchoolId", response.Result.SchoolId);
                    StartActivity(intent);
                };

                if (!string.IsNullOrEmpty(response.Result.PrimaryColor))
                {
                    try
                    {
                        (btnBack.Parent as ViewGroup).SetBackgroundColor(Color.ParseColor(response.Result.PrimaryColor));
                        (btnProfile.Parent as ViewGroup).SetBackgroundColor(Color.ParseColor(response.Result.PrimaryColor));
                    }
                    catch
                    {
                    }
                }

            });
        }
    }
}