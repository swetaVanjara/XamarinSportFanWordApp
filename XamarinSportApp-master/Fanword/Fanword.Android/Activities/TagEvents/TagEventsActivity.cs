using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Fanword.Android.Adapters;
using Fanword.Android.Extensions;
using Fanword.Android.ViewHolders;
using Fanword.Poco.Models;
using Fanword.Shared;
using FFImageLoading;
using FFImageLoading.Views;
using FFImageLoading.Work;
using Mobile.Extensions.Android.Adapters;
using Mobile.Extensions.Android.Extensions;
using Newtonsoft.Json;
using Xamarin.Facebook.Share.Model;
using Xamarin.Facebook.Share.Widget;
using System.ComponentModel;
using Fanword.Android.Shared;
using Fanword.Android.TypeFaces;
using Java.Lang;
using Fanword.Shared.Helpers;
using System.Globalization;
using TimeZoneNames;

namespace Fanword.Android.Activities.TagEvents
{
    [Activity(Label = "TagEventsActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class TagEventsActivity : BaseActivity
    {
        private RecyclerView rvDates { get; set; }
        private ImageButton btnBack { get; set; }
        private Button btnPost { get; set; }
        private ImageButton btnShare { get; set; }
        private ListView lvEvents { get; set; }
        private TextView lblTitle { get; set; }

        DateTime currentTime;
        Post Post;
        private CustomListAdapter<EventProfile> adapter;
        EventSearch result;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TagEventLayout);
            this.PopulateViewProperties();
            Post = JsonConvert.DeserializeObject<Post>(Intent.GetStringExtra("Post"));
            SetupViewBindings();
        }

        void SetupViewBindings()
        {
            btnBack.Click += (sender, args) => Finish();

			ShowHelpIfNecessary(TutorialHelper.TagEvents);

            lblTitle.Typeface = CustomTypefaces.RobotoBold;

            var layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false);
            rvDates.SetLayoutManager(layoutManager);

            var times = new List<DateTime>();
           currentTime = DateTime.Now.Date;

        //   cureentTime = DateTime.Now.Date;

         //   currentTime = DateTime.Now.Date.ToUniversalTime();
            var startTime = DateTime.Now.Date.ToUniversalTime().AddDays(-2);
            for (int i = 0; i < 5; i++)
            {
                times.Add(startTime.AddDays(i));
            }

            rvDates.SetAdapter(new CustomRecyclerViewAdapter<DateTime>(times, BindViewHolder, CreateViewHolder, (time, i) => 0));

            adapter = new CustomListAdapter<EventProfile>(new List<EventProfile>(), GetView);
            adapter.NoContentText = "No Events";
            lvEvents.Adapter = adapter;
            lvEvents.ItemClick += (sender, e2) =>
            {
                var e = adapter[e2.Position];
                if (Post.Events.Contains((e.Id)))
                {
                    Post.Events.Remove(e.Id);
                }
                else
                {
                    Post.Events.Add(e.Id);
                }

                adapter.NotifyDataSetChanged();
            };
            GetData();

            btnPost.Click += (sender, e) =>
            {
                if (Post.Videos.Any(m => string.IsNullOrEmpty(m.Id)))
                {
                    new AlertDialog.Builder(this)
                        .SetTitle("Video Upload")
                        .SetMessage("You video will be uploaded in the background and the post will be made visible once the upload is complete")
                        .SetPositiveButton("Ok", (o, args) => SavePost())
                        .Show();
                }
                else
                {
                    SavePost();
                }

            };

            if(!string.IsNullOrEmpty(Post.Id))
            {
                btnShare.Visibility = ViewStates.Gone;
            }

            btnShare.Click += (sender, e) => 
            {
                var builder = new AlertDialog.Builder(this);
                builder.SetTitle("Share On");
                ApplicationInfo info = null;
				try
				{
                    info = PackageManager.GetApplicationInfo("com.facebook.katana", 0);
				}
				catch (PackageManager.NameNotFoundException ex)
				{
				}

                if((Post.Images.Any() || Post.Links.Any() || Post.Videos.Any()) && info != null)
                {
                    builder.SetPositiveButton("Facebook", (s2,e2) =>
                    {
                        Sharer.ShareFacebook(Post);
                        Post.IsShared = true;
                    });
                }

                builder.SetNeutralButton("Other", (s2, e2) =>
                {
                    Sharer.ShareOther(Post);
                    Post.IsShared = true;
                });				
                builder.SetNegativeButton("Cancel", (s2,e2) =>{});
                builder.Show();

            };
        }

        void SavePost()
        {
            ShowProgressDialog();
            var json = JsonConvert.SerializeObject(Post);
            var apiTask = new ServiceApi().SavePost(Post);
            apiTask.HandleError(this);
            apiTask.OnSucess(this, respons =>
            {
                MainActivity.PostId = "Refresh";
                Intent intent = new Intent(this, typeof(MainActivity));
                intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
                StartActivity(intent);
            });
        }

        View GetView(EventProfile item, int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = LayoutInflater.Inflate(Resource.Layout.EventProfileItem, null);
            }

			string lang = CultureInfo.CurrentCulture.Name;
			var abbreviations = TZNames.GetAbbreviationsForTimeZone(item.TimezoneId, lang);
			string lblTimeZone = abbreviations.Standard;

            view.FindViewById<TextView>(Resource.Id.lblTeam1).Text = item.Team1Name;
            view.FindViewById<TextView>(Resource.Id.lblTeam2).Text = item.Team2Name;
            view.FindViewById<TextView>(Resource.Id.lblEventName).Text = item.Name;
            view.FindViewById<TextView>(Resource.Id.lblSport).Text = item.SportName;
            //view.FindViewById<TextView>(Resource.Id.lblTime).Text = item.DateOfEventUtc.ToLocalTime().ToString("hh:mm tt") + " " + lblTimeZone;
            if(item.IsTbd)
            {
                view.FindViewById<TextView>(Resource.Id.lblTime).Text= "TBD"  + " " + lblTimeZone;
            }
            else
            {
                DateTime eventDate = ConvertToUTC(item.DateOfEventUtc, item.TimezoneId);
                //view.FindViewById<TextView>(Resource.Id.lblTime).Text = item.DateOfEventUtc.ToString("hh:mm tt") + " " + lblTimeZone;
                view.FindViewById<TextView>(Resource.Id.lblTime).Text = eventDate.ToString("hh:mm tt") + " " + lblTimeZone;

            }

            var team2ImageView = view.FindViewById<ImageViewAsync>(Resource.Id.imgTeam2);
            team2ImageView.Tag?.CancelPendingTask(item.Team2Url);
            var task2 = ImageService.Instance.LoadUrl(item.Team2Url)
                .Retry(3, 300)
                .LoadingPlaceholder(Resource.Drawable.DefProfPic.ToString(), ImageSource.CompiledResource)
                .Into(team2ImageView);
            team2ImageView.Tag = new ImageLoaderHelper(task2);

            var team1ImageView = view.FindViewById<ImageViewAsync>(Resource.Id.imgTeam1);
            team1ImageView.Tag?.CancelPendingTask(item.Team1Url);
            var task = ImageService.Instance.LoadUrl(item.Team1Url)
                .Retry(3, 300)
                .LoadingPlaceholder(Resource.Drawable.DefProfPic.ToString(), ImageSource.CompiledResource)
                .Into(team1ImageView);
            team1ImageView.Tag = new ImageLoaderHelper(task);

            if (item.TeamCount == 2)
            {
                view.FindViewById<LinearLayout>(Resource.Id.llTeam1).Visibility = ViewStates.Visible;
                view.FindViewById<LinearLayout>(Resource.Id.llTeam2).Visibility = ViewStates.Visible;
                view.FindViewById<TextView>(Resource.Id.lblEventName).Visibility = ViewStates.Gone;
            }
            else
            {
                view.FindViewById<LinearLayout>(Resource.Id.llTeam1).Visibility = ViewStates.Gone;
                view.FindViewById<LinearLayout>(Resource.Id.llTeam2).Visibility = ViewStates.Gone;
                view.FindViewById<TextView>(Resource.Id.lblEventName).Visibility = ViewStates.Visible;
            }

            if (Post.Events.Contains(item.Id))
            {
                view.FindViewById<ImageView>(Resource.Id.imgTagged).SetImageResource(Resource.Drawable.CheckYES);
            }
            else
            {
                view.FindViewById<ImageView>(Resource.Id.imgTagged).SetImageResource(Resource.Drawable.CheckNO);
            }

            return view;
        }


        void GetData()
        {
            var apiTask = new ServiceApi().SearchEvents(currentTime);
            apiTask.HandleError(this);
            apiTask.OnSucess(this, response =>
            {
                if (response.Result.SearchTime == currentTime)
                {
                    result = response.Result;
                    adapter.Items = response.Result.Events;
                    adapter.NotifyDataSetChanged();
                }
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
        void BindViewHolder(RecyclerView.ViewHolder holder, DateTime item, int position, int viewType)
        {
            if (item == DateTime.Now.Date.ToUniversalTime())
            {
                (holder as DateViewHolder).lblDate.Text = "Today";
            }
            else
            {
                (holder as DateViewHolder).lblDate.Text = item.ToLocalTime().ToString("M");
            }

            if (item == currentTime)
            {
                (holder as DateViewHolder).lblDate.SetBackgroundColor(new Color(244,244,244));
                (holder as DateViewHolder).lblDate.SetTextColor(Color.Black);
            }
            else
            {
                (holder as DateViewHolder).lblDate.SetBackgroundColor(Color.White);
                (holder as DateViewHolder).lblDate.SetTextColor(new Color(144, 144, 144));
            }
        }

        RecyclerView.ViewHolder CreateViewHolder(ViewGroup parent, int viewType, Action<int> onClick)
        {
            var view = LayoutInflater.Inflate(Resource.Layout.DateItem, null);
            var holder = new DateViewHolder(view, (position) =>
            {
                var adater = rvDates.GetAdapter() as CustomRecyclerViewAdapter<DateTime>;
                var item = adater.Items[position];
                currentTime = item;
                adater.NotifyDataSetChanged();
                GetData();
            });

            return holder;
        }
    }
}