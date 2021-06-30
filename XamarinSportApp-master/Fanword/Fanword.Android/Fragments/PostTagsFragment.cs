using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fanword.Android.Activities.PostDetails;
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
    public class PostTagsFragment : BaseFragment, IPostDetails
    {
        public string Name => "Tags";
        public int Count { get; set; }
        private ListView lvTags { get; set; }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.PostTagsFragmentLayout, null);
            this.PopulateViewProperties(view);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            GetData();
            lvTags.ItemClick += (sender, args) =>
            {
                var item = (lvTags.Adapter as CustomListAdapter<PostTag>).Items[args.Position];
                if (item.Type == FeedType.User)
                {
                    Navigator.GoToUserProflie(item.Id);
                }
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
            };
        }

        void GetData()
        {
            var apiTask = new ServiceApi().GetTags((Activity as PostDetailsActivity).PostId);
            apiTask.HandleError(ActivityProgresDialog);
            apiTask.OnSucess(ActivityProgresDialog, response =>
            {
                var adapter = new CustomListAdapter<PostTag>(response.Result, GetView);
                adapter.NoContentText = "No Tags";
                lvTags.Adapter = adapter;
                Count = response.Result.Count;
                (Activity as PostDetailsActivity).SetHeaderInfo(Name, Count);
            });
        }

        View GetView(PostTag item, int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = Activity.LayoutInflater.Inflate(Resource.Layout.PostTagItem, null);
            }

            view.FindViewById<TextView>(Resource.Id.lblTitle).Text = item.Title;
            view.FindViewById<TextView>(Resource.Id.lblSubtitle).Text = item.Subtitle;
            if (string.IsNullOrEmpty(item.Subtitle))
            {
                view.FindViewById<TextView>(Resource.Id.lblSubtitle).Visibility = ViewStates.Gone;
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.lblSubtitle).Visibility = ViewStates.Visible;

            }

            var profileImageView = view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile);
            profileImageView.Tag?.CancelPendingTask(item.ProfileUrl);
            var task = ImageService.Instance.LoadUrl(item.ProfileUrl)
                .Retry(3, 300)
                .LoadingPlaceholder(Resource.Drawable.DefProfPic.ToString(), ImageSource.CompiledResource)
                .Transform(new CircleTransformation())
                .Into(profileImageView);

            profileImageView.Tag = new ImageLoaderHelper(task);


            return view;
        }
    }
}