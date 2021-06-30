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
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Views;
using FFImageLoading.Work;
using Mobile.Extensions.Android.Adapters;
using Mobile.Extensions.Android.Extensions;

namespace Fanword.Android.Fragments
{
    public class FavoriteUsersFragment : BaseFragment
    {
        private List<FavoriteItem> Followers;
        private List<FavoriteItem> Following;
        private ListView lvFavorites { get; set; }
        private Button btnFollowing { get; set; }
        private Button btnFollowers { get; set; }
        private CustomListAdapter<FavoriteItem> adapter;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.FavoriteUsersFragment, null);
            this.PopulateViewProperties(view);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            btnFollowing.Click += (sender, args) =>
            {
                SetButtons(btnFollowing);
                adapter.Items = Following;
                adapter.NotifyDataSetChanged();
            };

            btnFollowers.Click += (sender, args) =>
            {
                SetFollowers();
            };

			if (Followers == null)
				return;

            SetView();
        }

        void SetButtons(Button button)
        {
            btnFollowing.SetTextColor(new Color(144, 144, 144));
            btnFollowers.SetTextColor(new Color(144, 144, 144));
            button.SetTextColor(new Color(21, 21, 21));
        }

        public void SetFollowers()
        {
			SetButtons(btnFollowers);
			adapter.Items = Followers;
			adapter.NotifyDataSetChanged();
        }

        public void SetData(List<FavoriteItem> following, List<FavoriteItem> followers)
        {
            Followers = followers;
            Following = following;

            if (lvFavorites == null)
                return;


            SetView();
        }

        void SetView()
        {
            adapter = new CustomListAdapter<FavoriteItem>(Following, GetView);
            lvFavorites.Adapter = adapter;
        }

        View GetView(FavoriteItem item, int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = Activity.LayoutInflater.Inflate(Resource.Layout.FavoriteItem, null);
                view.FindViewById<Button>(Resource.Id.btnFollow).Click += (sender, args) =>
                {
                    var model = adapter.Items[(int)view.Tag];
                    Shared.Follower.FollowToggle(ActivityProgresDialog, sender as Button, model, model.Id, model.Type);
                };

                view.FindViewById<TextView>(Resource.Id.lblTitle).Click += (sender, args) =>
                {
                    var model = adapter.Items[(int)view.Tag];
                    GoToProfile(model);
                };

                view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile).Click += (sender, args) =>
                {
                    var model = adapter.Items[(int)view.Tag];
                    GoToProfile(model);
                };
            }
            view.Tag = position;

            view.FindViewById<TextView>(Resource.Id.lblTitle).Text = item.Title;
            view.FindViewById<TextView>(Resource.Id.lblSubtitle).Text = item.Subtitle;
            view.FindViewById<TextView>(Resource.Id.lblSubtitle).Visibility = string.IsNullOrEmpty(item.Subtitle) ? ViewStates.Gone : ViewStates.Visible;

            var profileImageView = view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile);
            profileImageView.Tag?.CancelPendingTask(item.ProfileUrl);
            var task = ImageService.Instance.LoadUrl(item.ProfileUrl)
                .Retry(3, 300)
                .LoadingPlaceholder(Resource.Drawable.DefProfPic.ToString(), ImageSource.CompiledResource)
                .Transform(new CircleTransformation())
                .Into(profileImageView);

            profileImageView.Tag = new ImageLoaderHelper(task);

            Views.SetFollowed(view.FindViewById<Button>(Resource.Id.btnFollow), item.IsFollowing);

            return view;
        }

        void GoToProfile(FavoriteItem item)
        {
            if (item.Type == FeedType.Team)
            {
                Navigator.GoToTeamProflie(item.Id);
            }
            if (item.Type == FeedType.School)
            {
                Navigator.GoToSchoolProflie(item.Id);
            }
            if (item.Type == FeedType.Sport)
            {
                Navigator.GoToSportProflie(item.Id);
            }
            if (item.Type == FeedType.ContentSource)
            {
                Navigator.GoToContentSource(item.Id);
            }
            if (item.Type == FeedType.User)
            {
                Navigator.GoToUserProflie(item.Id);
            }
        }
    }
}