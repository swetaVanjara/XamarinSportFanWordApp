﻿using System;
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
 using Fanword.Android.TypeFaces;
 using Fanword.Poco.Models;
using Fanword.Shared;
using Mobile.Extensions.Android.Extensions;
using Fanword.Shared.Helpers;

namespace Fanword.Android.Activities.ContentSource
{
    [Activity(Label = "ContentSourceActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ContentSourceActivity : BaseActivity
    {
        private ImageButton btnBack { get; set; }
        private TextView lblTitle { get; set; }
        private ViewPager vpPager { get; set; }
        private Button btnProfile { get; set; }
        private Button btnAbout { get; set; }
        private string contentSourceId { get; set; }
        private List<Fragment> fragments;
        private RelativeLayout rlIndicator { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ContentSourceProfileLayout);
            this.PopulateViewProperties();
            SetupViewBindings();
        }

        void SetupViewBindings()
        {
            btnBack.Click += (sender, e) =>
            {
                Finish();
            };

            contentSourceId = Intent.GetStringExtra("ContentSourceId");
            ShowHelpIfNecessary(TutorialHelper.Profiles, () =>
            {
                ShowHelpIfNecessary(TutorialHelper.ContentSource);
            });



            lblTitle.Typeface = CustomTypefaces.RobotoBold;

            fragments = new List<Fragment>();
            fragments.Add(new ContentSourceProfileFragment() { ContentSourceId = contentSourceId });
            fragments.Add(new ContentSourceAboutFragment());
            vpPager.Adapter = new FragmentViewPagerAdapter(FragmentManager, fragments);

            btnProfile.Click += (sender, args) =>
            {
                vpPager.SetCurrentItem(0, true);
            };

            btnAbout.Click += (sender, args) =>
            {
                vpPager.SetCurrentItem(1, true);
            };

            Display display = WindowManager.DefaultDisplay;
            Point size = new Point();
            display.GetSize(size);
            var screenWidth = size.X;

            var parameters = new RelativeLayout.LayoutParams((int)(screenWidth / (float)fragments.Count), (int)(2 * Resources.DisplayMetrics.Density));
            parameters.AddRule(LayoutRules.AlignParentBottom);
            rlIndicator.LayoutParameters = parameters;
            vpPager.PageScrolled += (sender, args) =>
            {
                var margins = rlIndicator.LayoutParameters as RelativeLayout.MarginLayoutParams;
                margins.LeftMargin = (int)((args.Position) * (screenWidth / (float)fragments.Count) + (args.PositionOffsetPixels / (float)fragments.Count));
                rlIndicator.LayoutParameters = margins;

                rlIndicator.LayoutParameters = margins;

                int position = args.PositionOffsetPixels > screenWidth / 2 ? args.Position + 1 : args.Position;

            };
        }

        public void DataFectched(ContentSourceProfile profile)
        {
            if(!string.IsNullOrEmpty(profile.PrimaryColor))
            {
                try
                {
					(btnBack.Parent as ViewGroup).SetBackgroundColor(Color.ParseColor(profile.PrimaryColor));
					(btnProfile.Parent as ViewGroup).SetBackgroundColor(Color.ParseColor(profile.PrimaryColor));
				}
                catch
                {
                }
            }

            lblTitle.Text = profile.Name;
            (fragments[1] as ContentSourceAboutFragment).SetData(profile);
        }
    }
}