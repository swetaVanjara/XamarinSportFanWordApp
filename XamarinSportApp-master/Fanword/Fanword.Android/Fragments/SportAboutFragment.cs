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
using FFImageLoading.Views;
using FFImageLoading.Work;
using Mobile.Extensions.Android.Adapters;
using Mobile.Extensions.Android.Extensions;
using FFImageLoading.Transformations;

namespace Fanword.Android.Fragments
{
    public class SportAboutFragment : BaseFragment
    {
        public string SportId { get; set; }
        private Button btnTeams { get; set; }
        private Button btnAthletes { get; set; }
        private ListView lvTeams { get; set; }
        private ListView lvAthletes { get; set; }
        private CustomListAdapter<TeamProfile> teamsAdapter;
        private CustomListAdapter<AthleteItem> athletesAdapter;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.SportAboutFragment, null);
            this.PopulateViewProperties(view);
            return view;
        }


        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            btnAthletes.Click += (sender, args) =>
            {
                SetButtons(btnAthletes);
                lvAthletes.Visibility = ViewStates.Visible;
                lvTeams.Visibility = ViewStates.Gone;
            };

            btnTeams.Click += (sender, args) =>
            {
                SetButtons(btnTeams);
                lvAthletes.Visibility = ViewStates.Gone;
                lvTeams.Visibility = ViewStates.Visible;
            };

            GetData();
        }

        void GetData()
        {
            var apiTask = new ServiceApi().GetTeamsForSport(SportId);
            apiTask.HandleError(ActivityProgresDialog);
            apiTask.OnSucess(ActivityProgresDialog, response =>
            {
                teamsAdapter = new CustomListAdapter<TeamProfile>(response.Result, GetTeamsView);
                lvTeams.Adapter = teamsAdapter;
            });

            var apiTask2 = new ServiceApi().GetAthletesForSport(SportId);
            apiTask2.HandleError(ActivityProgresDialog);
            apiTask2.OnSucess(ActivityProgresDialog, response =>
            {
                athletesAdapter = new CustomListAdapter<AthleteItem>(response.Result, GetAthletesView);
                lvAthletes.Adapter = athletesAdapter;
            });
        }

        void SetButtons(Button button)
        {
            btnAthletes.SetTextColor(new Color(144, 144, 144));
            btnTeams.SetTextColor(new Color(144, 144, 144));
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
                    var model = teamsAdapter.Items[(int)view.Tag];
                    Shared.Follower.FollowToggle(ActivityProgresDialog, sender as Button, model, model.Id, FeedType.Team);
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