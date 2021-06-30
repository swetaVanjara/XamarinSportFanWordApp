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
using Fanword.Android.Activities.SportProfile;
using Fanword.Poco.Models;
using Fanword.Shared;
using Mobile.Extensions.Android.Adapters;
using Mobile.Extensions.Android.Extensions;

namespace Fanword.Android.Fragments
{
    public class TeamRankingsFragment : BaseFragment
    {
        public string TeamId { get; set; }
        private ListView lvRanks { get; set; }
        private TextView lblRecord { get; set; }
        private Button btnSportRankings { get; set; }
        private CustomListAdapter<TeamRanking> adapter;
        private TeamProfile profile;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.TeamRankingsFragment, null);
            this.PopulateViewProperties(view);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
			btnSportRankings.Click += (sender, e) =>
			{
                if (profile == null)
                    return;
                
				Intent intent = new Intent(Activity, typeof(SportProfileActivity));
				intent.PutExtra("SportId", profile.SportId);
				intent.PutExtra("GoToRankings", true);
				StartActivity(intent);
			};
            SetView();
        }

        public void SetData(TeamProfile profile)
        {
            this.profile = profile;
            SetView();
        }
        
        void SetView()
        {
            if (lvRanks == null || profile == null)
                return;

            lblRecord.Text = profile.Wins + "W " + profile.Loss + "L " + profile.Ties + "T";
            btnSportRankings.Text = "Check the latest " + profile.SportName + " rankings";

            var apiTask = new ServiceApi().GetTeamRankings(TeamId);
            apiTask.HandleError(ActivityProgresDialog);
            apiTask.OnSucess(ActivityProgresDialog, (response) =>
            {
                adapter = new CustomListAdapter<TeamRanking>(response.Result, GetView);
                lvRanks.Adapter = adapter;
            });
        }

        View GetView(TeamRanking item, int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = Activity.LayoutInflater.Inflate(Resource.Layout.TeamRankingItem, null);
            }

            view.FindViewById<TextView>(Resource.Id.lblDate).Text = item.DateUpdatedUtc.ToString("dd MMMM");
            view.FindViewById<TextView>(Resource.Id.lblRank).Text = item.Rank.ToString();
            if (position == adapter.Items.Count -1)
            {
                view.FindViewById<TextView>(Resource.Id.lblChange).Visibility = ViewStates.Gone;
                view.FindViewById<ImageView>(Resource.Id.imgChange).Visibility = ViewStates.Gone;
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.lblChange).Visibility = ViewStates.Visible;
                view.FindViewById<ImageView>(Resource.Id.imgChange).Visibility = ViewStates.Visible;
                var nextItem = adapter.Items[position + 1];
                var change = nextItem.Rank - item.Rank;
                if (change < 0)
                {
                    view.FindViewById<TextView>(Resource.Id.lblChange).SetTextColor(Color.Red);
                }
                else
                {
                    view.FindViewById<TextView>(Resource.Id.lblChange).SetTextColor(Color.DarkGreen);
                }
                view.FindViewById<TextView>(Resource.Id.lblChange).Text = Math.Abs(change).ToString();
            }

            return view;
        }
    }
}