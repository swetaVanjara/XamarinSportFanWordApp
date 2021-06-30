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
    public class PostSharesFragment : BaseFragment, IPostDetails
    {
        public string Name => "Shares";
        public int Count { get; set; }
        private ListView lvShares { get; set; }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.PostSharesFragmentLayout, null);
            this.PopulateViewProperties(view);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            GetData();
        }

        void GetData()
        {
            var apiTask = new ServiceApi().GetShares((Activity as PostDetailsActivity).PostId);
            apiTask.HandleError(ActivityProgresDialog);
            apiTask.OnSucess(ActivityProgresDialog, response =>
            {
                var adapter = new CustomListAdapter<PostShare>(response.Result, GetView);
                adapter.NoContentText = "No Share";
                lvShares.Adapter = adapter;
                Count = response.Result.Count;
                (Activity as PostDetailsActivity).SetHeaderInfo(Name, Count);
            });
        }

        View GetView(PostShare item, int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = Activity.LayoutInflater.Inflate(Resource.Layout.PostLikeItem, null);
            }

            view.FindViewById<TextView>(Resource.Id.lblName).Text = item.FirstName + " " + item.LastName;

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