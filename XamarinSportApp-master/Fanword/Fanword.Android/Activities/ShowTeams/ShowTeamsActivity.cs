using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fanword.Android.Extensions;
using Fanword.Poco.Models;
using Fanword.Shared;
using FFImageLoading;
using FFImageLoading.Views;
using FFImageLoading.Work;
using Mobile.Extensions.Android.Adapters;
using Mobile.Extensions.Android.Extensions;

namespace Fanword.Android.Activities.ShowTeams
{
    [Activity(Label = "ShowTeamsActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ShowTeamsActivity : BaseActivity
    {
        public string EventId { get; set; }
        private ImageButton btnBack { get; set; }
        private TextView lblCount { get; set; }
        private ListView lvTeams { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ShowTeamsLayout);
            this.PopulateViewProperties();
            EventId = Intent.GetStringExtra("EventId");
            SetupViewBindings();
        }

        void SetupViewBindings()
        {
            btnBack.Click += (sender, args) => Finish();
            GetData();
        }

        void GetData()
        {
            ShowProgressDialog();
            var apiTask = new ServiceApi().GetEventTeams(EventId);
            apiTask.HandleError(this);
            apiTask.OnSucess(this, response =>
            {
                HideProgressDialog();
                var adapter = new CustomListAdapter<EventTeam>(response.Result, GetView);
                lvTeams.Adapter = adapter;
                lblCount.Text = response.Result.Count.ToString();
            });
        }

        View GetView(EventTeam item, int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = LayoutInflater.Inflate(Resource.Layout.EventTeamItem, null);
            }
            view.FindViewById<TextView>(Resource.Id.lblSchool).Text = item.SchoolName;
            view.FindViewById<TextView>(Resource.Id.lblSport).Text = item.SportName;
            view.FindViewById<TextView>(Resource.Id.lblScore).Text = item.Score;

            var profileImageView = view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile);
            profileImageView.Tag?.CancelPendingTask(item.ProfileUrl);
            var task = ImageService.Instance.LoadUrl(item.ProfileUrl)
                .Retry(3, 300)
                .LoadingPlaceholder(Resource.Drawable.DefProfPic.ToString(), ImageSource.CompiledResource)
                .Into(profileImageView);

            profileImageView.Tag = new ImageLoaderHelper(task);

            return view;
        }
    }
}