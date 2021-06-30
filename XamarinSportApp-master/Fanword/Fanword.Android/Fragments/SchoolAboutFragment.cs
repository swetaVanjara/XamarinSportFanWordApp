using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fanword.Android.Extensions;
using Fanword.Android.Shared;
using Fanword.Poco.Models;
using Fanword.Shared;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Views;
using FFImageLoading.Work;
using Mobile.Extensions.Android.Adapters;
using Mobile.Extensions.Android.Extensions;

namespace Fanword.Android.Fragments
{
    public class SchoolAboutFragment : BaseFragment
    {
        public string SchoolId { get; set; }
        private Button btnTeams { get; set; }
        private Button btnAthletes { get; set; }
        private Button btnLinks { get; set; }
        private ListView lvTeams { get; set; }
        private ListView lvAthletes { get; set; }
        private LinearLayout llLinks { get; set; }
        private Button btnWebsite { get; set; }
        private Button btnFacebook { get; set; }
        private Button btnInstagram { get; set; }
        private Button btnTwitter { get; set; }
        private Button btnSchedule { get; set; }
        private CustomListAdapter<TeamProfile> teamsAdapter;
        private CustomListAdapter<AthleteItem> athletesAdapter;
        private SchoolProfile profile;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.SchoolAboutFragment, null);
            this.PopulateViewProperties(view);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            btnAthletes.Click += (sender, args) =>
            {
                SetButtons(btnAthletes, lvAthletes);
            };

            btnTeams.Click += (sender, args) =>
            {
                SetButtons(btnTeams, lvTeams);
            };

            btnLinks.Click += (sender, args) =>
            {
                SetButtons(btnLinks, llLinks);
            };

            GetData();

            (btnFacebook.Parent.Parent as ViewGroup).Visibility = ViewStates.Gone;
            (btnInstagram.Parent.Parent as ViewGroup).Visibility = ViewStates.Gone;
            (btnTwitter.Parent.Parent as ViewGroup).Visibility = ViewStates.Gone;
			btnSchedule.Click += (sender, e) => OpenUrl(profile?.ScheduleUrl);
			btnWebsite.Click += (sender, e) => OpenUrl(profile?.WebsiteUrl);
			btnFacebook.Click += (sender, e) => OpenUrl(profile?.FacebookUrl);
			btnTwitter.Click += (sender, e) => OpenUrl(profile?.TwitterUrl);
			btnInstagram.Click += (sender, e) => OpenUrl(profile?.InstagramUrl);
            SetView();
        }

        public void SetData(SchoolProfile profile)
        {
            this.profile = profile;
            SetView();
        }

        void OpenUrl(string url)
        {
            try
            {
                if (!url.ToLower().StartsWith("http"))
                {
                    url = "http://" + url;
                }
                Intent intent = new Intent(Intent.ActionView);
                intent.SetData(global::Android.Net.Uri.Parse(url));
                StartActivity(intent);
            }
            catch (Exception e)
            {
            }
        }

        void SetView()
        {
            if (btnSchedule == null || profile == null)
                return;
            
            if (!string.IsNullOrEmpty(profile.FacebookUrl))
            {
                (btnFacebook.Parent.Parent as ViewGroup).Visibility = ViewStates.Visible;
            }
            if (!string.IsNullOrEmpty(profile.TwitterUrl))
            {
                (btnTwitter.Parent.Parent as ViewGroup).Visibility = ViewStates.Visible;
            }
            if (!string.IsNullOrEmpty(profile.InstagramUrl))
            {
                (btnInstagram.Parent.Parent as ViewGroup).Visibility = ViewStates.Visible;
            }
        }

        void GetData()
        {
            var apiTask = new ServiceApi().GetTeamsForSchool(SchoolId);
            apiTask.HandleError(ActivityProgresDialog);
            apiTask.OnSucess(ActivityProgresDialog, response =>
            {
                teamsAdapter = new CustomListAdapter<TeamProfile>(response.Result, GetTeamsView);
                lvTeams.Adapter = teamsAdapter;
            });

            var apiTask2 = new ServiceApi().GetAthletesForSchool(SchoolId);
            apiTask2.HandleError(ActivityProgresDialog);
            apiTask2.OnSucess(ActivityProgresDialog, response =>
            {
                athletesAdapter = new CustomListAdapter<AthleteItem>(response.Result, GetAthletesView);
                lvAthletes.Adapter = athletesAdapter;
            });
        }

        void SetButtons(Button button, ViewGroup group)
        {
            btnAthletes.SetTextColor(new Color(144, 144, 144));
            btnTeams.SetTextColor(new Color(144, 144, 144));
            btnLinks.SetTextColor(new Color(144, 144, 144));
            lvTeams.Visibility = ViewStates.Gone;
            lvAthletes.Visibility = ViewStates.Gone;
            llLinks.Visibility = ViewStates.Gone;
            group.Visibility = ViewStates.Visible;
            button.SetTextColor(new Color(21, 21, 21));
        }

        View GetTeamsView(TeamProfile item, int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = Activity.LayoutInflater.Inflate(Resource.Layout.SportTeamsItem, null);
                view.FindViewById<Button>(Resource.Id.btnFollow).Click += (sender, e) =>
                {
                    var model = teamsAdapter.Items[(int)view.Tag];
                    Shared.Follower.FollowToggle(ActivityProgresDialog, sender as Button, model, model.Id, FeedType.Team);
                };

				view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile).Click += (sender, e) =>
				{
					var model = teamsAdapter.Items[(int)view.Tag];
					Navigator.GoToTeamProflie(model.Id, false);
				};

				view.FindViewById<TextView>(Resource.Id.lblName).Click += (sender, e) =>
				{
					var model = teamsAdapter.Items[(int)view.Tag];
					Navigator.GoToTeamProflie(model.Id, false);
				};
            }
            view.Tag = position;

            view.FindViewById<TextView>(Resource.Id.lblName).Text = item.SchoolName;
            view.FindViewById<TextView>(Resource.Id.lblSubtitle).Text = item.SportName;

            var profileImageView = view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile);
            profileImageView.Tag?.CancelPendingTask(item.ProfileUrl);
            var task = ImageService.Instance.LoadUrl(item.ProfileUrl)
                .Retry(3, 300)
                .LoadingPlaceholder(Resource.Drawable.DefProfPic.ToString(), ImageSource.CompiledResource)
                .Into(profileImageView);

            profileImageView.Tag = new ImageLoaderHelper(task);

            Views.SetFollowed(view.FindViewById<Button>(Resource.Id.btnFollow), item.IsFollowing);

            return view;
        }

        View GetAthletesView(AthleteItem item, int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = Activity.LayoutInflater.Inflate(Resource.Layout.SportTeamsItem, null);
                view.FindViewById<Button>(Resource.Id.btnFollow).Click += (sender, e) =>
                {
                    var model = athletesAdapter.Items[(int)view.Tag];
                    Fanword.Android.Shared.Follower.FollowToggle(ActivityProgresDialog, sender as Button, model, model.Id, FeedType.User);
                };

				view.FindViewById<TextView>(Resource.Id.lblName).Click += (sender, e) =>
				{
					var model = athletesAdapter.Items[(int)view.Tag];
					Navigator.GoToUserProflie(model.Id);
				};

				view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile).Click += (sender, e) =>
				{
					var model = athletesAdapter.Items[(int)view.Tag];
					Navigator.GoToUserProflie(model.Id);
				};
            }
            view.Tag = position;

            view.FindViewById<TextView>(Resource.Id.lblName).Text = item.Name;
            view.FindViewById<TextView>(Resource.Id.lblSubtitle).Text = item.SchoolName;

            var profileImageView = view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile);
            profileImageView.Tag?.CancelPendingTask(item.ProfileUrl);
            var task = ImageService.Instance.LoadUrl(item.ProfileUrl)
                .Retry(3, 300)
                .Transform(new CircleTransformation())
                .LoadingPlaceholder(Resource.Drawable.DefProfPic.ToString(), ImageSource.CompiledResource)
                .Into(profileImageView);

            profileImageView.Tag = new ImageLoaderHelper(task);

            Views.SetFollowed(view.FindViewById<Button>(Resource.Id.btnFollow), item.IsFollowing);

            return view;
        }
    }
}