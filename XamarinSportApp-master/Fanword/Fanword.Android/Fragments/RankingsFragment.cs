using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Style;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fanword.Android.Extensions;
using Fanword.Android.TypeFaces;
using Fanword.Poco.Models;
using Fanword.Shared;
using FFImageLoading;
using FFImageLoading.Views;
using FFImageLoading.Work;
using Mobile.Extensions.Android.Adapters;
using Mobile.Extensions.Android.Extensions;
using Android.Support.V4.Widget;
using Fanword.Android.Activities.SportProfile;
using Fanword.Android.Shared;

namespace Fanword.Android.Fragments
{
    public class RankingsFragment : BaseFragment
    {

        private SwipeRefreshLayout slRefresh { get; set; }
        private ImageButton btnFilter { get; set; }
        private ListView lvRankings { get; set; }
        private CustomListAdapter<Ranking> adapter;
        private bool myTeams;
        private bool mySports;
        private bool mySchools;
     
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.RankingsLayout, null);
            this.PopulateViewProperties(view);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
			mySchools = true;
			mySports = true;
			myTeams = true;	

            GetData();

            btnFilter.Click += (sender, args) =>
            {
                View v = Activity.LayoutInflater.Inflate(Resource.Layout.FollowFilterLayout, null);
                var dialog = new AlertDialog.Builder(Activity).SetView(v).Create();

                v.FindViewById<ImageView>(Resource.Id.imgTeams).SetImageResource(myTeams ? Resource.Drawable.CheckYES : Resource.Drawable.CheckNO);
                v.FindViewById<ImageView>(Resource.Id.imgSchools).SetImageResource(mySchools ? Resource.Drawable.CheckYES : Resource.Drawable.CheckNO);
                v.FindViewById<ImageView>(Resource.Id.imgSports).SetImageResource(mySports ? Resource.Drawable.CheckYES : Resource.Drawable.CheckNO);

                v.FindViewById<ImageView>(Resource.Id.imgTeams).Click += (o, eventArgs) =>
                {
                    myTeams = !myTeams;
                    v.FindViewById<ImageView>(Resource.Id.imgTeams).SetImageResource(myTeams ? Resource.Drawable.CheckYES : Resource.Drawable.CheckNO);
                };

                v.FindViewById<ImageView>(Resource.Id.imgSchools).Click += (o, eventArgs) =>
                {
                    mySchools = !mySchools;
                    v.FindViewById<ImageView>(Resource.Id.imgSchools).SetImageResource(mySchools ? Resource.Drawable.CheckYES : Resource.Drawable.CheckNO);

                };

                v.FindViewById<ImageView>(Resource.Id.imgSports).Click += (o, eventArgs) =>
                {
                    mySports = !mySports;
                    v.FindViewById<ImageView>(Resource.Id.imgSports).SetImageResource(mySports ? Resource.Drawable.CheckYES : Resource.Drawable.CheckNO);
                };
                v.FindViewById<Button>(Resource.Id.btnFilter).Click += (o, eventArgs) => 
                {
                    dialog.Dismiss();
                };
                dialog.DismissEvent += (o, eventArgs) =>
                {
                    GetData();
                };
                dialog.Show();
            };

            slRefresh.Refresh += (sender, e) => GetData();
        }
        

        void GetData()
        {
            var filter = new FollowingFilterModel();
            filter.MySchools = mySchools;
            filter.MySports = mySports;
            filter.MyTeams = myTeams;
            var apiTask = new ServiceApi().GetRankings(filter);
            apiTask.HandleError(ActivityProgresDialog);
            apiTask.OnSucess(ActivityProgresDialog, response =>
            {
                if (lvRankings.Adapter == null)
                {
                    adapter = new CustomListAdapter<Ranking>(response.Result, GetView);
                    adapter.NoContentText = "No Rankings";
                    lvRankings.Adapter = adapter;
                }
                else
                {
                    adapter.Items = response.Result;
                    adapter.NotifyDataSetChanged();
                }
                slRefresh.Refreshing = false;
            });
        }

        View GetView(Ranking item, int position, View convertView, ViewGroup parent)
        {
           
            View view = convertView;
            if (view == null)
            {
                view = Activity.LayoutInflater.Inflate(Resource.Layout.RankingItem, null);
                view.FindViewById<Button>(Resource.Id.btnFollow).Click += (sender, args) =>
                {
					var model = adapter.Items[(int)view.Tag];
                    Shared.Follower.FollowToggle(ActivityProgresDialog, sender as Button, model, model.TeamId, FeedType.Team);
                };

                view.FindViewById<Button>(Resource.Id.btnShowRankings).Click += (sender, e) => 
                {
                    var model = adapter.Items[(int)view.Tag];
                    if (model.IsActive == true)
                    {
                        
                        Navigator.GoToSportProflie(model.SportId, true);
                    }
                };

				view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile).Click += (sender, e) =>
				{
                    var model = adapter.Items[(int)view.Tag];
                    if (model.IsActive == true)
                    {
                        
                        Navigator.GoToTeamProflie(model.TeamId, false);
                    }
				};

                view.FindViewById<TextView>(Resource.Id.lblTeamName).Click += (sender, e) => 
                {
                    var model = adapter.Items[(int)view.Tag];
                    if (model.IsActive == true)
                    {
                      
                        Navigator.GoToTeamProflie(model.TeamId, false);
                    }
                };
                view.FindViewById<TextView>(Resource.Id.lblRank).Typeface = CustomTypefaces.RobotoBold;
                view.FindViewById<TextView>(Resource.Id.lblTeamName).Typeface = CustomTypefaces.RobotoBold;
            }

            view.Tag = position;
            view.FindViewById<TextView>(Resource.Id.lblRank).Text = item.Rank.ToString();
            view.FindViewById<TextView>(Resource.Id.lblTeamName).Text = item.TeamName;
            view.FindViewById<TextView>(Resource.Id.lblSportName).Text = item.SportName;
            if (item.IsActive == true)
            {
                view.FindViewById<TextView>(Resource.Id.lblWinsLosses).Text = item.Wins + "W " + item.Loses + "L " + item.Ties + "T";
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.lblWinsLosses).SetTextColor(Color.DarkGray);
                view.FindViewById<TextView>(Resource.Id.lblWinsLosses).Text = "This Profile is not active yet";
            }
            view.FindViewById<TextView>(Resource.Id.lblDate).Text = item.DateUpdatedUtc.ToString("dd MMMM");

            var profileImageView = view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile);
            profileImageView.Tag?.CancelPendingTask(item.ProfileUrl);
            var task = ImageService.Instance.LoadUrl(item.ProfileUrl)
                .Retry(3, 300)
                .DownSample(200)
                .LoadingPlaceholder(Resource.Drawable.DefProfPic.ToString(), ImageSource.CompiledResource)
                .Into(profileImageView);

            profileImageView.Tag = new ImageLoaderHelper(task);

            string text = "Show " + item.SportName + " Rankings";
            var span = new SpannableString(text);
            span.SetSpan(new FanwordTypefaceSpan(CustomTypefaces.RobotoBold), 5, item.SportName.Length + 5, SpanTypes.ExclusiveExclusive);
            view.FindViewById<TextView>(Resource.Id.btnShowRankings).TextFormatted = span;
            if (item.IsFollowing)
            {
                if(item.IsActive==true)
                {
                    (view.FindViewById<Button>(Resource.Id.btnFollow).Parent as RelativeLayout).Visibility = ViewStates.Visible;
                    view.FindViewById<Button>(Resource.Id.btnFollow).SetTextColor(Color.White);
                    view.FindViewById<Button>(Resource.Id.btnFollow).Text = "Following";
                    (view.FindViewById<Button>(Resource.Id.btnFollow).Parent as RelativeLayout).Background = Resources.GetDrawable(Resource.Drawable.FollowingRoundedBackground);
                }
                else
                {
                    (view.FindViewById<Button>(Resource.Id.btnFollow).Parent as RelativeLayout).Visibility = ViewStates.Gone;
                }
             
            }
            else
            {

                if (item.IsActive == true)
                {
                    (view.FindViewById<Button>(Resource.Id.btnFollow).Parent as RelativeLayout).Visibility = ViewStates.Visible;
                    view.FindViewById<Button>(Resource.Id.btnFollow).SetTextColor(new Color(144, 144, 144));
                    view.FindViewById<Button>(Resource.Id.btnFollow).Text = "Follow";
                    (view.FindViewById<Button>(Resource.Id.btnFollow).Parent as RelativeLayout).Background = Resources.GetDrawable(Resource.Drawable.FollowRoundedBackground);
                }
                else
                {
                    (view.FindViewById<Button>(Resource.Id.btnFollow).Parent as RelativeLayout).Visibility = ViewStates.Gone;

                }
            }
            return view;
        }
    }

    public class FanwordTypefaceSpan : TypefaceSpan
    {
        Typeface _typeface;
        public FanwordTypefaceSpan(Typeface typeface) : base("Normal")
        {
            _typeface = typeface;
        }

        public override void UpdateDrawState(TextPaint ds)
        {
            base.UpdateDrawState(ds);
            ds.SetTypeface(_typeface);
        }

        public override void UpdateMeasureState(TextPaint paint)
        {
            base.UpdateMeasureState(paint);
            paint.SetTypeface(_typeface);
        }

    }
}