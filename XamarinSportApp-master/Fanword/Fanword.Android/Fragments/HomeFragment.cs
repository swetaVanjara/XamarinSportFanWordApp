using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fanword.Android.Activities.FavoritesView;
using Fanword.Android.Activities.Notifications;
using Fanword.Android.Activities.Search;
using Fanword.Android.Adapters;
using Fanword.Android.TypeFaces;
using Fanword.Shared;
using Fanword.Shared.Helpers;
using HockeyApp.Android;
using Mobile.Extensions.Android.Extensions;

namespace Fanword.Android.Fragments
{
    public class HomeFragment : BaseFragment
    {
        public ViewPager vpPager { get; set; }
        private ImageButton btnMenu { get; set; }
        private ImageButton btnSearch { get; set; }
        private ImageButton btnExplore { get; set; }
        private ImageButton btnNotifications { get; set; }
        private Button btnFeed { get; set; }
        private Button btnScores { get; set; }
        private Button btnRankings { get; set; }
        private TextView lblTitle { get; set; }
        private RelativeLayout rlIndicator { get; set; }
        private TextView lblNotificationCount { get; set; }
        private List<Fragment> fragments;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.HomeFragmentLayout, null);
            this.PopulateViewProperties(view);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            lblTitle.Typeface = CustomTypefaces.RobotoBold;
            fragments = new List<Fragment>();
            fragments.Add(new FeedFragment());
            fragments.Add(new ScoresFragment());
            fragments.Add(new RankingsFragment());
            vpPager.Adapter = new FragmentViewPagerAdapter(FragmentManager, fragments);
            vpPager.OffscreenPageLimit = 3;

			btnMenu.Click += (sender, e) =>
			{
				(Activity as MainActivity).dlDrawer.OpenDrawer((Activity as MainActivity).flMenuContainer);
			};

            btnFeed.Tag = 0;
            btnScores.Tag = 1;
            btnRankings.Tag = 2;
            btnFeed.Click += ButtonClicked;
            btnScores.Click += ButtonClicked;
            btnRankings.Click += ButtonClicked;
            
            Display display = Activity.WindowManager.DefaultDisplay;
            Point size = new Point();
            display.GetSize(size);
            var screenWidth = size.X;

            var parameters = new RelativeLayout.LayoutParams((int)(screenWidth / 3f), (int)(2 * Resources.DisplayMetrics.Density));
            parameters.AddRule(LayoutRules.AlignParentBottom);
            rlIndicator.LayoutParameters = parameters;
            vpPager.PageScrolled += (sender, args) =>
            {
                var margins = rlIndicator.LayoutParameters as RelativeLayout.MarginLayoutParams;
                margins.LeftMargin = (int)((args.Position) * (screenWidth / 3f) + (args.PositionOffsetPixels / 3f));
                rlIndicator.LayoutParameters = margins;

                int position = args.PositionOffsetPixels > screenWidth / 2 ? args.Position + 1 : args.Position;
                SetButtonStates(position);
				if (position == 1)
				{
					(Activity as BaseActivity).ShowHelpIfNecessary(TutorialHelper.SchedulesScores);
				}
            };

            btnNotifications.Click += (sender, args) => Activity.StartActivity(typeof(NotificationActivity));
            btnSearch.Click += (sender, args) => Activity.StartActivity(typeof(SearchActivity));
            btnExplore.Click += (sender, args) => Activity.StartActivity(typeof(FavoritesActivity));
            lblNotificationCount.Visibility = ViewStates.Gone;
        }

		public void GetNotifications()
		{
			var apiTask = new ServiceApi().GetNotifcations();
			apiTask.HandleError(ActivityProgresDialog);
			apiTask.OnSucess(ActivityProgresDialog,(response) =>
			{
				var count = response.Result.Count(m => !m.IsRead);
				lblNotificationCount.Text = count.ToString();
				if (count > 0)
				{
					lblNotificationCount.Visibility = ViewStates.Visible;
				}
				else
				{
					lblNotificationCount.Visibility = ViewStates.Gone;
				}
			});

            (fragments[1] as ScoresFragment).GetData();
        }

        void SetButtonStates(int position)
        {
            btnFeed.SetTextColor(new Color(144,144,144));
            btnScores.SetTextColor(new Color(144, 144, 144));
            btnRankings.SetTextColor(new Color(144, 144, 144));

            if (position == 0)
            {
                btnFeed.SetTextColor(Color.White);
            }
            if (position == 1)
            {
                btnScores.SetTextColor(Color.White);
            }
            if (position == 2)
            {
                btnRankings.SetTextColor(Color.White);
            }
        }

        void ButtonClicked(object sender, EventArgs e)
        {
            vpPager.SetCurrentItem((int)(sender as Button).Tag, true);
        }
    }
}