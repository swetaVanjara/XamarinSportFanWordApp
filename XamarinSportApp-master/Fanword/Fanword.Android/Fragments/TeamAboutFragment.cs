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
    public class TeamAboutFragment : BaseFragment
    {
        private TeamProfile profile;
        private Button btnWebsite { get; set; }
        private Button btnFacebook { get; set; }
        private Button btnInstagram { get; set; }
        private Button btnTwitter { get; set; }
        private Button btnSchedule { get; set; }
        private Button btnRoster { get; set; }

		private ListView lvAthletes { get; set; }
		private LinearLayout llLinks { get; set; }
		private Button btnAthletes { get; set; }
		private Button btnLinks { get; set; }

		private CustomListAdapter<AthleteItem> athletesAdapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.TeamAboutFragment, null);
            this.PopulateViewProperties(view);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            (btnFacebook.Parent.Parent as ViewGroup).Visibility = ViewStates.Gone;
            (btnInstagram.Parent.Parent as ViewGroup).Visibility = ViewStates.Gone;
            (btnTwitter.Parent.Parent as ViewGroup).Visibility = ViewStates.Gone;
			btnRoster.Click += (sender, args) => Links.OpenUrl(profile?.RosterUrl);
			btnSchedule.Click += (sender, e) => Links.OpenUrl(profile?.ScheduleUrl);
			btnWebsite.Click += (sender, e) => Links.OpenUrl(profile?.WebsiteUrl);
			btnFacebook.Click += (sender, e) => Links.OpenUrl(profile?.FacebookUrl);
			btnTwitter.Click += (sender, e) => Links.OpenUrl(profile?.TwitterUrl);
			btnInstagram.Click += (sender, e) => Links.OpenUrl(profile?.InstagramUrl);

			btnAthletes.Click += (sender, args) =>
			{
				SetButtons(btnAthletes, lvAthletes);
			};

			btnLinks.Click += (sender, args) =>
			{
				SetButtons(btnLinks, llLinks);
			};

            SetView();

            lvAthletes.Visibility = ViewStates.Visible;
			llLinks.Visibility = ViewStates.Gone;
        }

		void GetData()
		{
			var apiTask2 = new ServiceApi().GetAthletesForTeam(profile.Id);
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
			btnLinks.SetTextColor(new Color(144, 144, 144));
			lvAthletes.Visibility = ViewStates.Gone;
			llLinks.Visibility = ViewStates.Gone;
			group.Visibility = ViewStates.Visible;
			button.SetTextColor(new Color(21, 21, 21));
		}

        void SetView()
        {
            if (btnRoster == null || profile == null)
                return;

            GetData();

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

        public void SetData(TeamProfile profile)
        {
            this.profile = profile;
            SetView();
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