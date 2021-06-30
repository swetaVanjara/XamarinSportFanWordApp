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
using Fanword.Android.Interfaces;
using Fanword.Android.TypeFaces;
using Fanword.Shared;
using Mobile.Extensions.Android.Extensions;

namespace Fanword.Android.Activities.PostDetails
{
    [Activity(Label = "PostDetailsActivity", ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.StateHidden)]
    public class PostDetailsActivity : BaseActivity
    {
        private ImageButton btnBack { get; set; }
        private TextView lblTitle { get; set; }
        private TextView lblCount { get; set; }
        private ViewPager vpPager { get; set; }
        private ImageButton btnLikes { get; set; }
        private ImageButton btnComments { get; set; }
        private ImageButton btnShares { get; set; }
        private ImageButton btnTags { get; set; }
        private RelativeLayout rlIndicator { get; set; }
        
        public string PostId { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PostDetailsLayout);
            this.PopulateViewProperties();
            PostId = Intent.GetStringExtra("PostId");
            SetupViewBindings();
        }

        void SetupViewBindings()
        {
            lblTitle.Typeface = CustomTypefaces.RobotoBold;
            btnBack.Click += (sender, e) =>
            {
                MainActivity.PostId = PostId;

                Finish();
            };

            var fragments = new List<Fragment>();
            fragments.Add(new PostLikesFragment());
            fragments.Add(new CommentsFragment());
            fragments.Add(new PostSharesFragment());
            fragments.Add(new PostTagsFragment());
            vpPager.Adapter = new FragmentViewPagerAdapter(FragmentManager, fragments);
            vpPager.OffscreenPageLimit = 4;
            
            btnLikes.Click += (sender, args) =>
            {
                vpPager.SetCurrentItem(0, true);
            };
            
            btnComments.Click += (sender, args) =>
            {
                vpPager.SetCurrentItem(1, true);
            };

            btnShares.Click += (sender, args) =>
            {
                vpPager.SetCurrentItem(2, true);
            };

            btnTags.Click += (sender, args) =>
            {
                vpPager.SetCurrentItem(3, true);
            };

            var fragment = Intent.GetStringExtra("Fragment");
            if (fragment == "Comments")
            {
                btnComments.PerformClick();
                var frag = (fragments[1] as IPostDetails);
                SetHeaderInfo(frag.Name, frag.Count);
            }
            if (fragment == "Shares")
            {
                btnShares.PerformClick();
                var frag = (fragments[2] as IPostDetails);
                SetHeaderInfo(frag.Name, frag.Count);
            }
            if (fragment == "Tags")
            {
                btnTags.PerformClick();
                var frag = (fragments[3] as IPostDetails);
                SetHeaderInfo(frag.Name, frag.Count);
            }

            Display display = WindowManager.DefaultDisplay;
            Point size = new Point();
            display.GetSize(size);
            var screenWidth = size.X;

            var parameters = new RelativeLayout.LayoutParams((int)(screenWidth/4f), (int)(2 * Resources.DisplayMetrics.Density));
            parameters.AddRule(LayoutRules.AlignParentBottom);
            rlIndicator.LayoutParameters = parameters;
            vpPager.PageScrolled += (sender, args) =>
            {
                var margins = rlIndicator.LayoutParameters as RelativeLayout.MarginLayoutParams;
                margins.LeftMargin = (int)((args.Position) * (screenWidth / 4f) + (args.PositionOffsetPixels /4f));
                rlIndicator.LayoutParameters = margins;

                rlIndicator.LayoutParameters = margins;

                int position = args.PositionOffsetPixels > screenWidth /2 ? args.Position + 1 : args.Position;
                var frag = (fragments[position] as IPostDetails);
                SetHeaderInfo(frag.Name, frag.Count);
            };

        }

        public void SetHeaderInfo(string title, int count)
        {

            if ((vpPager.CurrentItem == 0 && title == "Likes") || (vpPager.CurrentItem == 1 && title == "Comments") || (vpPager.CurrentItem == 2 && title == "Shares") || (vpPager.CurrentItem == 3 && title == "Tags"))
            {
                lblTitle.Text = title;
                lblCount.Text = count.ToString();
            }

        }


        public override void OnBackPressed()
        {
            base.OnBackPressed();
            MainActivity.PostId = PostId;

        }
    }
}