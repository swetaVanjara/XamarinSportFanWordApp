using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
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
using Follower = Fanword.Poco.Models.Follower;

namespace Fanword.Android.Activities.Followers
{
    [Activity(Label = "FollowersActivity")]
    public class FollowersActivity : BaseActivity
    {
        private string Id { get; set; }
        private CustomListAdapter<Follower> adapter;
        private ListView lvFollowers { get; set; }
        private ImageButton btnBack { get; set; }
        private TextView lblCount { get; set; }
        private FeedType Type { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FollowersLayout);
            this.PopulateViewProperties();
            Id = Intent.GetStringExtra("Id");
            Type = (FeedType) Intent.GetIntExtra("Type", 0);
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
            var apiTask = new ServiceApi().Followers(Id, Type);
            apiTask.HandleError(this);
            apiTask.OnSucess(this, (response) =>
            {
                HideProgressDialog();
                lblCount.Text = response.Result.Count.ToString();
                adapter = new CustomListAdapter<Follower>(response.Result, GetView);
                lvFollowers.Adapter = adapter;
            });
        }

        View GetView(Follower item, int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = LayoutInflater.Inflate(Resource.Layout.FavoriteItem, null);
                view.FindViewById<Button>(Resource.Id.btnFollow).Click += (sender, args) =>
                {
                    var model = adapter.Items[(int)view.Tag];
                    Shared.Follower.FollowToggle(this, sender as Button, model, model.Id, FeedType.User);
                };

                view.FindViewById<TextView>(Resource.Id.lblTitle).Click += (sender, args) =>
                {
                    var model = adapter.Items[(int)view.Tag];
                    Navigator.GoToUserProflie(model.Id);
                };

                view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile).Click += (sender, args) =>
                {
                    var model = adapter.Items[(int)view.Tag];
                    Navigator.GoToUserProflie(model.Id);
                };
            }
            view.Tag = position;

            view.FindViewById<TextView>(Resource.Id.lblTitle).Text = item.FirstName + " " + item.LastName;
            view.FindViewById<TextView>(Resource.Id.lblSubtitle).Visibility = ViewStates.Gone;

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
        
    }
}