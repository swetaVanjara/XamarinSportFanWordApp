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

namespace Fanword.Android.Fragments
{
    public class SportRankingsFragment : BaseFragment
    {
        public string SportId { get; set; }
        public TextView lblDate { get; set; }
        public ListView lvRankings { get; set; }
        private CustomListAdapter<Ranking> adapter;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.SportRankingsFragment, null);
            this.PopulateViewProperties(view);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            var apiTask = new ServiceApi().GetSportRankings(SportId);
            apiTask.HandleError(ActivityProgresDialog);
            apiTask.OnSucess(ActivityProgresDialog, response =>
            {
                adapter = new CustomListAdapter<Ranking>(response.Result, GetView);
                lblDate.Text = response.Result.FirstOrDefault()?.DateUpdatedUtc.ToLocalTime().ToString("D");
                lvRankings.Adapter = adapter;
              
            });
        }

      

        View GetView(Ranking item, int position, View convertView, ViewGroup parent)
        {
         // Toast.MakeText(this.Activity,  adapter.GetItemAtPosition(position).Rank.ToString(),ToastLength.Short).Show();
            View view = convertView;

            if (view == null)
            {
                view = Activity.LayoutInflater.Inflate(Resource.Layout.SportRankingItem, null);
                view.FindViewById<Button>(Resource.Id.btnFollow).Click += (sender, e) =>
                {
                    //  int Item_id = adapter.Items[(int)view.Tag].Rank;
                   
                    bool enable = adapter.Items[(int)view.Tag].IsActive;
                    if (enable == true)
                    {
                        var model = adapter.Items[(int)view.Tag];
                        
                        Shared.Follower.FollowToggle(ActivityProgresDialog, sender as Button, model, model.TeamId, FeedType.Team);
                    }
                };

				view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile).Click += (sender, e) =>
				{
                    bool enable = adapter.Items[(int)view.Tag].IsActive;

                    if (enable == true)
                    {
                        var model = adapter.Items[(int)view.Tag];
                        Navigator.GoToTeamProflie(model.TeamId, false);
                    }
				};

				view.FindViewById<TextView>(Resource.Id.lblName).Click += (sender, e) =>
				{
                    bool enable = adapter.Items[(int)view.Tag].IsActive;
                    if (enable == true)
                    {
                        var model = adapter.Items[(int)view.Tag];
                        Navigator.GoToTeamProflie(model.TeamId, false);
                    }
				};
            }
            view.Tag = position;
            view.FindViewById<TextView>(Resource.Id.lblRank).Text = item.Rank.ToString();
            view.FindViewById<TextView>(Resource.Id.lblName).Text = item.TeamName;
            if (item.IsActive == true)
            {

                view.FindViewById<TextView>(Resource.Id.lblRecord).Text = item.Wins + "W " + item.Loses + "L " + item.Ties + "T";
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.lblRecord).SetTextColor(Color.DarkGray);
                view.FindViewById<TextView>(Resource.Id.lblRecord).Text = "This Profile is not active yet";
            }
           
            if(item.IsActive==true)
            {
                (view.FindViewById<Button>(Resource.Id.btnFollow).Parent as RelativeLayout).Visibility = ViewStates.Visible;
                Views.SetFollowed(view.FindViewById<Button>(Resource.Id.btnFollow), item.IsFollowing);
               
            }
            else
            {
                Views.SetFollowed(view.FindViewById<Button>(Resource.Id.btnFollow), item.IsFollowing);
                (view.FindViewById<Button>(Resource.Id.btnFollow).Parent as RelativeLayout).Visibility = ViewStates.Gone;
                //view.FindViewById<Button>(Resource.Id.btnFollow).Visibility = ViewStates.Gone;
            }
           
            
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