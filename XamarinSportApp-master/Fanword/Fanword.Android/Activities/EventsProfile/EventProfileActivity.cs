using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using Fanword.Android.Activities.ShowTeams;
using Fanword.Android.CustomViews;
using Fanword.Android.Fragments;
using Fanword.Android.Shared;
using Fanword.Android.TypeFaces;
using Fanword.Poco.Models;
using Fanword.Shared;
using Fanword.Shared.Helpers;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Views;
using Mobile.Extensions.Android.Extensions;
using TimeZoneNames;

namespace Fanword.Android.Activities.EventsProfile
{
    [Activity(Label = "EventProfileActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class EventProfileActivity : BaseActivity
    {
        private SwipeRefreshLayout slRefresh { get; set; }
        private FeedRecyclerView rvFeed { get; set; }
        private ImageButton btnBack { get; set; }
        private TextView lblSport { get; set; }
        private TextView lblTitle { get; set; }
        public string EventId { get; set; }
        private TextView lblPosts;
        private TextView lblName;
        private TextView lblDate;
        private TextView lblTime;
        private TextView lblLocation;
        private LinearLayout llTwoTeams;
        private LinearLayout llManyTeams;
        private ImageViewAsync imgTeam1;
        private ImageViewAsync imgTeam2;
        private TextView lblTeam1Name;
        private TextView lblTeam2Name;
        private TextView lblEventName;
        private Button btnShowTeams;
        private ImageViewAsync imgRight;
        private TextView lblTeam1Score;
        private TextView lblTeam2Score;
        Poco.Models.EventProfile profile;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EventProfileLayout);
            this.PopulateViewProperties();
            EventId = Intent.GetStringExtra("EventId");
            SetupViewBindings();

        }

        void SetupViewBindings()
        {
            btnBack.Click += (sender, args) => Finish();

            var headerView = LayoutInflater.Inflate(Resource.Layout.EventProfileHeaderLayout, null);
            imgTeam1 = headerView.FindViewById<ImageViewAsync>(Resource.Id.imgTeam1);
            imgTeam2 = headerView.FindViewById<ImageViewAsync>(Resource.Id.imgTeam2);
            lblTeam1Name = headerView.FindViewById<TextView>(Resource.Id.lblTeam1Name);
            lblTeam2Name = headerView.FindViewById<TextView>(Resource.Id.lblTeam2Name);
            imgRight = headerView.FindViewById<ImageViewAsync>(Resource.Id.imgRight);
            lblLocation = headerView.FindViewById<TextView>(Resource.Id.lblLocation);
            lblEventName = headerView.FindViewById<TextView>(Resource.Id.lblEventName);
            btnShowTeams = headerView.FindViewById<Button>(Resource.Id.btnShowTeams);
            lblDate = headerView.FindViewById<TextView>(Resource.Id.lblDate);
            lblTime = headerView.FindViewById<TextView>(Resource.Id.lblTime);
            lblName = headerView.FindViewById<TextView>(Resource.Id.lblName);
            llTwoTeams = headerView.FindViewById<LinearLayout>(Resource.Id.llTwoTeams);
            llManyTeams = headerView.FindViewById<LinearLayout>(Resource.Id.llManyTeams);
            lblPosts = headerView.FindViewById<TextView>(Resource.Id.lblPosts);
            lblTeam1Score = headerView.FindViewById<TextView>(Resource.Id.lblTeam1Score);
            lblTeam2Score = headerView.FindViewById<TextView>(Resource.Id.lblTeam2Score);

            rvFeed.Initialize(this, headerView, EventId, FeedType.Event);
            rvFeed.SwipeContainer = slRefresh;
            rvFeed.GetNewsFeedItems(true);
            rvFeed.RefreshRequested += (sender, args) =>
            {
                GetData();
            };
            lblTitle.Typeface = CustomTypefaces.RobotoBold;

            lblPosts.Typeface = CustomTypefaces.RobotoBold;

            imgRight.Click += (sender, args) =>
            {
                if (profile != null && !string.IsNullOrEmpty(profile.TicketUrl))
                {
                    Links.OpenUrl(profile.TicketUrl);
                }
            };

            btnShowTeams.Click += (sender, args) =>
            {
                if (profile != null)
                {
                    Intent intent = new Intent(this, typeof(ShowTeamsActivity));
                    intent.PutExtra("EventId", EventId);
                    StartActivity(intent);
                }
            };

            lblTime.Typeface = CustomTypefaces.RobotoBold;
            lblLocation.Typeface = CustomTypefaces.RobotoBold;
            lblEventName.Typeface = CustomTypefaces.RobotoBold;
            GetData();
        }

        void GetData()
        {
            DateTime eventDate = DateTime.Now;
            var apiTask = new ServiceApi().GetEventProfile(EventId);
            apiTask.HandleError(this);
            apiTask.OnSucess(this, response =>
            {
                this.profile = response.Result;
                lblTitle.Text = response.Result.Name;
                lblSport.Text = response.Result.SportName;
                lblLocation.Text = response.Result.Location;

                eventDate = ConvertToUTC(response.Result.DateOfEventUtc, response.Result.TimezoneId);

                if (response.Result.IsTbd)
                {
                    lblTime.Text = "TBD";
                }
                else
                {
                    string lang = CultureInfo.CurrentCulture.Name;
                    var abbreviations = TZNames.GetAbbreviationsForTimeZone(response.Result.TimezoneId, lang);

                    lblTime.Text = eventDate.ToString("h:mm tt") + " " + abbreviations.Standard;
                }
                lblDate.Text = eventDate.ToString("D");
                lblName.Text = response.Result.Name;
                lblEventName.Text = profile.Name;

                if (!string.IsNullOrEmpty(profile.Team1Url))
                    ImageService.Instance.LoadUrl(profile.Team1Url).Retry(3,300).Into(imgTeam1);

                if (!string.IsNullOrEmpty(profile.Team2Url))
                    ImageService.Instance.LoadUrl(profile.Team2Url).Retry(3, 300).Into(imgTeam2);

                lblTeam1Name.Text = profile.Team1Name;
                lblTeam2Name.Text = profile.Team2Name;

                imgRight.Enabled = true;

                if (string.IsNullOrEmpty(profile.TicketUrl))
                {
                    imgRight.Visibility = ViewStates.Invisible;
                }

                if (profile.DateOfEventUtc <= DateTime.UtcNow)
                {
                    if (profile.TeamCount == 2)
                    {
                        imgRight.Visibility = ViewStates.Gone;
                    }
                    else
                    {
                        imgRight.Visibility = ViewStates.Visible;
                    }
                    imgRight.SetImageBitmap(null);
                    if (!string.IsNullOrEmpty(profile.WinningTeamUrl))
                        ImageService.Instance.LoadUrl(profile.WinningTeamUrl).Retry(3, 300).Into(imgRight);

                    imgRight.Enabled = false;

                }

                var span = new SpannableString("Show all " + profile.TeamCount + " teams");
                span.SetSpan(new FanwordTypefaceSpan(CustomTypefaces.RobotoBold), 9, profile.TeamCount.ToString().Length + 15, SpanTypes.ExclusiveExclusive);
                btnShowTeams.TextFormatted = span;



                if (profile.TeamCount == 2)
                {
                    llTwoTeams.Visibility = ViewStates.Visible;
                    llManyTeams.Visibility = ViewStates.Gone;
                }
                else
                {
                    llTwoTeams.Visibility = ViewStates.Gone;
                    llManyTeams.Visibility = ViewStates.Visible;
                }

                lblPosts.Text = LargeValueHelper.GetString(response.Result.Posts);

                lblTeam1Score.Text = response.Result.Team1Score;
                lblTeam2Score.Text = response.Result.Team2Score;
            });
        }

        public DateTime ConvertToUTC(DateTime dd, string timezoneId)
        {
            DateTime eventDate = dd;
            if (!string.IsNullOrEmpty(timezoneId))
            {
                TimeZoneInfo zoneInfo;
                try
                {
                    zoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
                }
                catch (TimeZoneNotFoundException)
                {
                    zoneInfo = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
                }

                if (zoneInfo.StandardName == TimeZone.CurrentTimeZone.StandardName)
                {
                    eventDate = eventDate.ToLocalTime();
                }
                else if (timezoneId.Contains("Central"))
                {
                    eventDate = dd.AddHours(-6);
                }
                else
                {
                    eventDate = TimeZoneInfo.ConvertTimeFromUtc(dd, zoneInfo);
                }
            }
            return eventDate;
        }
    }
}