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
using Android.Util;
using Android.Views;
using Android.Widget;
using Fanword.Android.Activities.ShowTeams;
using Fanword.Android.Extensions;
using Fanword.Android.TypeFaces;
using Fanword.Poco.Models;
using Fanword.Shared;
using FFImageLoading;
using FFImageLoading.Views;
using FFImageLoading.Work;
using Mobile.Extensions.Android.Adapters;
using Mobile.Extensions.Android.Extensions;
using Fanword.Android.Shared;
using TimeZoneNames;
using System.Globalization;
using Felipecsl.GifImageViewLibrary;
using System.Net.Http;
using Java.IO;
using Android.Graphics.Drawables;
using Android.Webkit;
using System.Timers;

namespace Fanword.Android.Fragments
{
    public class ScoresFragment : BaseFragment
    {

        private ImageButton btnFilter { get; set; }
        private ListView lvScores { get; set; }
        private CustomListAdapter<ScoreModel> adapter;
        private bool myTeams;
        private bool mySports;
        private bool mySchools;
        private Button btnPast { get; set; }
        private Button btnToday { get; set; }
        private Button btnUpcoming { get; set; }

        private DateFilter dateFilter = DateFilter.Today;
        public string TeamId;
        public string SportId;
        public string SchoolId;
        public static event Action<List<ScoreModel>> ScoreChanged;
        WebView webLoadingIcon;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ScoresLayout, null);

            webLoadingIcon = view.FindViewById<WebView>(Resource.Id.webLoadingIcon);
            webLoadingIcon.SetLayerType(LayerType.Software, null);

            this.PopulateViewProperties(view);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            
            if (string.IsNullOrEmpty(TeamId + SportId + SchoolId))
            {
                mySchools = true;
                mySports = true;
                myTeams = true;
            }
            btnPast.Click += (sender, args) =>
            {
                SetButtons(btnPast);
                dateFilter = DateFilter.Past;
                GetData();
            };

            btnToday.Click += (sender, args) =>
            {
                SetButtons(btnToday);
                dateFilter = DateFilter.Today;
                GetData();
            };

            btnUpcoming.Click += (sender, args) =>
            {
                
                SetButtons(btnUpcoming);
                dateFilter = DateFilter.Upcoming;
                GetData();
            };

            SetButtons(btnToday);
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



            if (!string.IsNullOrEmpty(TeamId) || !string.IsNullOrEmpty(SportId) || !string.IsNullOrEmpty(SchoolId))
            {
                btnFilter.Visibility = ViewStates.Gone;
            }
            lvScores.ItemClick += (sender, e) =>
            {
                var model = adapter.Items[e.Position];

                if (!string.IsNullOrEmpty(model.EventId))
                {
                    Navigator.GoToEventProfile(model.EventId);
                }
            };

            btnPast.Typeface = CustomTypefaces.RobotoBold;
            btnToday.Typeface = CustomTypefaces.RobotoBold;
            btnUpcoming.Typeface = CustomTypefaces.RobotoBold;
        }

        void SetButtons(Button button)
        {
            btnPast.SetTextColor(new Color(144, 144, 144));
            btnToday.SetTextColor(new Color(144, 144, 144));
            btnUpcoming.SetTextColor(new Color(144, 144, 144));
            button.SetTextColor(new Color(21, 21, 21));
        }

        public void GetData()
        {
            if (lvScores == null)
                return;

            var filter = new ScoresFilterModel();
          
            filter.FollowFilter = new FollowingFilterModel();
            filter.TeamId = TeamId;
            filter.SportId = SportId;
            filter.SchoolId = SchoolId;
            filter.FollowFilter.MySchools = mySchools;
            filter.FollowFilter.MySports = mySports;
            filter.FollowFilter.MyTeams = myTeams;
            filter.DateFilter = dateFilter;
          //  var dt = DateTime.ParseExact("1/14/2018 12:00:00 AM", "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            filter.Today = DateTime.Now.Date;
            //filter.Today = DateTime.UtcNow.Date;

            webLoadingIcon.Visibility = ViewStates.Visible;
            webLoadingIcon.SetBackgroundColor(new Color(227, 228, 230));
            webLoadingIcon.LoadUrl(string.Format("file:///android_asset/loader.gif"));

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Start();
            timer.Elapsed += (object sender, ElapsedEventArgs e) => {
                webLoadingIcon.LoadUrl(string.Format("file:///android_asset/loader.gif"));
            };

            var apiTask = new ServiceApi().GetScores(filter);
            apiTask.HandleError(ActivityProgresDialog);

            apiTask.OnSucess(ActivityProgresDialog, response =>
            { 
                ActivityProgresDialog.HideProgressDialog();
                if (lvScores.Adapter == null)
                {
                    adapter = new CustomListAdapter<ScoreModel>(response.Result, GetView);
                    adapter.SetViewTypes(GetViewType, 2);
                    adapter.NoContentText = "No Scores";
                    lvScores.Adapter = adapter;
                    adapter.NotifyDataSetChanged();
                }
                else
                {
                    if (ScoreChanged != null)
                    {
                        ScoreChanged.Invoke(response.Result);
                    }
                    //adapter.Items = response.Result;
                    //adapter.NotifyDataSetChanged();
                }
                webLoadingIcon.Visibility = ViewStates.Invisible;
                timer.Stop();
            });

            ScoresFragment.ScoreChanged += (List<ScoreModel> obj) =>
            {
                List<ScoreModel> TempList = new List<ScoreModel>();
                TempList.Clear();
                foreach (var item in obj)
                {
                    DateTime eventDate = ConvertToUTC(item.EventDate, item.TimezoneId);
                    if (dateFilter == DateFilter.Today)
                    {
                        if (eventDate.Date < DateTime.Now.Date)
                        {
                            System.Console.Write("No");
                        }
                        else
                        {

                            TempList.Add(new ScoreModel()
                            {
                                EventDate = eventDate,
                                EventId = item.EventId,
                                EventName = item.EventName,
                                IsTbd = item.IsTbd,
                                PostCount = item.PostCount,
                                ShowTicketUrl = item.ShowTicketUrl,
                                SportName = item.SportName,
                                SportProfileUrl = item.SportProfileUrl,
                                Team1Name = item.Team1Name,
                                Team1Score = item.Team1Score,
                                Team1Url = item.Team1Url,
                                Team2Name = item.Team2Name,
                                Team2Score = item.Team2Score,
                                Team2Url = item.Team2Url,
                                TeamCount = item.TeamCount,
                                TicketUrl = item.TicketUrl,
                                TimezoneId = item.TimezoneId
                            });
                        }

                    }
                    else if(dateFilter == DateFilter.Upcoming)
                    {
                        if (eventDate.Date > DateTime.Now.Date)
                        {
                            TempList.Add(new ScoreModel()
                            {
                                EventDate = eventDate,
                                EventId = item.EventId,
                                EventName = item.EventName,
                                IsTbd = item.IsTbd,
                                PostCount = item.PostCount,
                                ShowTicketUrl = item.ShowTicketUrl,
                                SportName = item.SportName,
                                SportProfileUrl = item.SportProfileUrl,
                                Team1Name = item.Team1Name,
                                Team1Score = item.Team1Score,
                                Team1Url = item.Team1Url,
                                Team2Name = item.Team2Name,
                                Team2Score = item.Team2Score,
                                Team2Url = item.Team2Url,
                                TeamCount = item.TeamCount,
                                TicketUrl = item.TicketUrl,
                                TimezoneId = item.TimezoneId
                            });
                        }
                    }
                    else
                    {
                        TempList.Add(new ScoreModel()
                        {
                            EventDate = eventDate,
                            EventId = item.EventId,
                            EventName = item.EventName,
                            IsTbd = item.IsTbd,
                            PostCount = item.PostCount,
                            ShowTicketUrl = item.ShowTicketUrl,
                            SportName = item.SportName,
                            SportProfileUrl = item.SportProfileUrl,
                            Team1Name = item.Team1Name,
                            Team1Score = item.Team1Score,
                            Team1Url = item.Team1Url,
                            Team2Name = item.Team2Name,
                            Team2Score = item.Team2Score,
                            Team2Url = item.Team2Url,
                            TeamCount = item.TeamCount,
                            TicketUrl = item.TicketUrl,
                            TimezoneId = item.TimezoneId
                        });

                    }



                }

                var grouped = TempList.GroupBy(m => m.EventDate.ToString("D"));
                var items = new List<ScoreModel>();
                foreach (var group in grouped)
                {
                    items.Add(new ScoreModel()
                    {
                        EventDate = Convert.ToDateTime(group.Key),
                        TeamCount = group.Count()
                    });
                    items.AddRange(group);
                }
                adapter.Items = items;
                adapter.NotifyDataSetChanged();
            };
        }

        int GetViewType(ScoreModel item, int position)
        {
            if (string.IsNullOrEmpty(item.EventId))
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        View GetView(ScoreModel item, int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                if (GetViewType(item, position) == 0)
                {
                    view = Activity.LayoutInflater.Inflate(Resource.Layout.ScoreItemHeader, null);
                    view.FindViewById<TextView>(Resource.Id.lblDate).Typeface = CustomTypefaces.RobotoBold;
                }
                else
                {
                    view = Activity.LayoutInflater.Inflate(Resource.Layout.ScoreItem, null);
                    view.FindViewById<TextView>(Resource.Id.lblShowMore).Click += (sender, args) =>
                    {
                        var model = adapter.Items[(int)view.Tag];

                        Intent intent = new Intent(Activity, typeof(ShowTeamsActivity));
                        intent.PutExtra("EventId", model.EventId);
                        StartActivity(intent);
                    };
                    view.FindViewById<TextView>(Resource.Id.lblTime).Typeface = CustomTypefaces.RobotoBold;
                    view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile).Click += (sender, args) =>
                    {
                        Links.OpenUrl(adapter[(int)view.Tag].TicketUrl);
                    };
                }
            }
            view.Tag = position;
            if (GetViewType(item, position) == 0)
            {
                var layout = view.FindViewById<LinearLayout>(Resource.Id.llContainer).LayoutParameters as LinearLayout.MarginLayoutParams;
                layout.TopMargin = position == 0 ? 0 : (int)(35 * Resources.DisplayMetrics.Density);
                view.FindViewById<LinearLayout>(Resource.Id.llContainer).LayoutParameters = layout;
                DateTime time = item.EventDate;  
                string format = "dddd, MMMM d, yyyy";   
                view.FindViewById<TextView>(Resource.Id.lblDate).Text = item.EventDate.ToString(format).ToUpper();
                view.FindViewById<TextView>(Resource.Id.lblCount).Text = item.TeamCount.ToString();
            }
            else
            {
                view.Tag = position;

                view.FindViewById<TextView>(Resource.Id.lblTeam1Name).Text = item.Team1Name;
                view.FindViewById<TextView>(Resource.Id.lblTeam1Score).Text = item.Team1Score;
                view.FindViewById<TextView>(Resource.Id.lblTeam2Name).Text = item.Team2Name;
                view.FindViewById<TextView>(Resource.Id.lblTeam2Score).Text = item.Team2Score;
                view.FindViewById<TextView>(Resource.Id.lblPostCount).Text = item.PostCount.ToString();
                view.FindViewById<TextView>(Resource.Id.lblSportName).Text = item.SportName;

                if (item.IsTbd)
                {
                    view.FindViewById<TextView>(Resource.Id.lblTime).Text = "TBD";
                }
                else
                {
                    view.FindViewById<TextView>(Resource.Id.lblTime).Text = item.EventDate.ToString("h:mm tt");
                }

                string lang = CultureInfo.CurrentCulture.Name;
                var abbreviations = TZNames.GetAbbreviationsForTimeZone(item.TimezoneId, lang);
                view.FindViewById<TextView>(Resource.Id.lblTimeZone).Text = abbreviations.Standard;


                if (item.EventDate <= DateTime.UtcNow)
                {
                    view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile).Visibility = ViewStates.Gone;
                    view.FindViewById<TextView>(Resource.Id.lblTeam1Score).Visibility = ViewStates.Visible;
                    view.FindViewById<TextView>(Resource.Id.lblTeam2Score).Visibility = ViewStates.Visible;
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.TicketUrl))
                    {
                        view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile).Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile).Visibility = ViewStates.Gone;
                    }
                    view.FindViewById<TextView>(Resource.Id.lblTeam1Score).Visibility = ViewStates.Gone;
                    view.FindViewById<TextView>(Resource.Id.lblTeam2Score).Visibility = ViewStates.Gone;
                }

                view.FindViewById<TextView>(Resource.Id.lblEventName).Text = item.EventName;
                var span = new SpannableString("Show all " + item.TeamCount + " teams");
                span.SetSpan(new FanwordTypefaceSpan(CustomTypefaces.RobotoBold), 9, item.TeamCount.ToString().Length + 15, SpanTypes.ExclusiveExclusive);
                view.FindViewById<TextView>(Resource.Id.lblShowMore).TextFormatted = span;

                if (item.TeamCount == 2)
                {
                    view.FindViewById<LinearLayout>(Resource.Id.llTwoTeams).Visibility = ViewStates.Visible;
                    view.FindViewById<LinearLayout>(Resource.Id.llMoreTeams).Visibility = ViewStates.Gone;
                }
                else
                {
                    view.FindViewById<LinearLayout>(Resource.Id.llTwoTeams).Visibility = ViewStates.Gone;
                    view.FindViewById<LinearLayout>(Resource.Id.llMoreTeams).Visibility = ViewStates.Visible;
                }

                var profileImageView = view.FindViewById<ImageViewAsync>(Resource.Id.imgTeam1);
                profileImageView.Tag?.CancelPendingTask(item.Team1Url);
                var task = ImageService.Instance.LoadUrl(item.Team1Url)
                    .Retry(3, 300)
                    .LoadingPlaceholder(Resource.Drawable.DefProfPic.ToString(), ImageSource.CompiledResource)
                    .Into(profileImageView);

                profileImageView.Tag = new ImageLoaderHelper(task);

                var profileImageView2 = view.FindViewById<ImageViewAsync>(Resource.Id.imgTeam2);
                profileImageView2.Tag?.CancelPendingTask(item.Team2Url);
                var task2 = ImageService.Instance.LoadUrl(item.Team2Url)
                    .Retry(3, 300)
                    .LoadingPlaceholder(Resource.Drawable.DefProfPic.ToString(), ImageSource.CompiledResource)
                    .Into(profileImageView2);

                profileImageView2.Tag = new ImageLoaderHelper(task2);

                var profileImageView3 = view.FindViewById<ImageViewAsync>(Resource.Id.imgSport);
                profileImageView3.Tag?.CancelPendingTask(item.SportProfileUrl);
                var task3 = ImageService.Instance.LoadUrl(item.SportProfileUrl)
                    .Retry(3, 300)
                    .LoadingPlaceholder(Resource.Drawable.DefProfPic.ToString(), ImageSource.CompiledResource)
                    .Into(profileImageView3);

                profileImageView3.Tag = new ImageLoaderHelper(task3);
            }
            return view;
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