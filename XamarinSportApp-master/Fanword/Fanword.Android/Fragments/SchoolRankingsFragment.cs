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
using Fanword.Android.Extensions;
using Fanword.Android.Shared;
using Fanword.Android.TypeFaces;
using Fanword.Poco.Models;
using Fanword.Shared;
using FFImageLoading;
using FFImageLoading.Views;
using FFImageLoading.Work;
using Mobile.Extensions.Android.Adapters;
using Mobile.Extensions.Android.Extensions;

namespace Fanword.Android.Fragments
{
    public class SchoolRankingsFragment : BaseFragment
    {
        public string SchoolId { get; set; }
        public ListView lvRankings { get; set; }
        private CustomListAdapter<Ranking> adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.SchoolRankingFragment, null);
            this.PopulateViewProperties(view);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            var apiTask = new ServiceApi().GetSchoolRankings(SchoolId);
            apiTask.HandleError(ActivityProgresDialog);
            apiTask.OnSucess(ActivityProgresDialog, response =>
            {
                adapter = new CustomListAdapter<Ranking>(response.Result, GetView);
                adapter.NoContentText = "No Rankings";
                lvRankings.Adapter = adapter;
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
                    var model = adapter.Items[(int) view.Tag];
                    Shared.Follower.FollowToggle(ActivityProgresDialog, view.FindViewById<Button>(Resource.Id.btnFollow), model, model.TeamId, FeedType.Team);
                };

				view.FindViewById<Button>(Resource.Id.btnShowRankings).Click += (sender, e) =>
				{
					var model = adapter.Items[(int)view.Tag];
					Navigator.GoToSportProflie(model.SportId, true);
				};

				view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile).Click += (sender, e) =>
				{
					var model = adapter.Items[(int)view.Tag];
					Navigator.GoToTeamProflie(model.TeamId, false);
				};

				view.FindViewById<TextView>(Resource.Id.lblTeamName).Click += (sender, e) =>
				{
					var model = adapter.Items[(int)view.Tag];
					Navigator.GoToTeamProflie(model.TeamId, false);
				};
            }
            view.Tag = position;
            view.FindViewById<TextView>(Resource.Id.lblRank).Text = item.Rank.ToString();
            view.FindViewById<TextView>(Resource.Id.lblTeamName).Text = item.TeamName;
            view.FindViewById<TextView>(Resource.Id.lblSportName).Text = item.SportName;
            view.FindViewById<TextView>(Resource.Id.lblWinsLosses).Text = item.Wins + "W " + item.Loses + "L ";
            view.FindViewById<TextView>(Resource.Id.lblDate).Text = item.DateUpdatedUtc.ToString("dd MMMM");

            var profileImageView = view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile);
            profileImageView.Tag?.CancelPendingTask(item.ProfileUrl);
            var task = ImageService.Instance.LoadUrl(item.ProfileUrl)
                .Retry(3, 300)
                .LoadingPlaceholder(Resource.Drawable.DefProfPic.ToString(), ImageSource.CompiledResource)
                .Into(profileImageView);

            profileImageView.Tag = new ImageLoaderHelper(task);

            string text = "Show " + item.SportName + " Rankings";
            var span = new SpannableString(text);
            span.SetSpan(new FanwordTypefaceSpan(CustomTypefaces.RobotoBold), 5, item.SportName.Length + 5, SpanTypes.ExclusiveExclusive);
            view.FindViewById<TextView>(Resource.Id.btnShowRankings).TextFormatted = span;
            Views.SetFollowed(view.FindViewById<Button>(Resource.Id.btnFollow), item.IsFollowing);
            return view;
        }

    }
}