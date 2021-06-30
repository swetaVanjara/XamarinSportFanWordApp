using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Fanword.Android.Activities.ViewPost;
using Fanword.Android.Extensions;
using Fanword.Android.Shared;
using Fanword.Android.TypeFaces;
using Fanword.Poco.Models;
using Fanword.Shared;
using Fanword.Shared.Models;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Views;
using FFImageLoading.Work;
using Mobile.Extensions.Android.Adapters;
using Mobile.Extensions.Android.Extensions;

namespace Fanword.Android.Activities.Notifications
{
   
    [Activity(Label = "NotificationActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class NotificationActivity : BaseActivity
    {
        private ImageButton btnBack { get; set; }
        private CustomListAdapter<UserNotification> adapter;
        private ListView lvNotifications { get; set; }
        private TextView lblTitle { get; set; }

        private SwipeRefreshLayout slRefresh { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.NotificationLayout);
            this.PopulateViewProperties();
            SetupViewBindings();
        }

        void SetupViewBindings()
        {
            lblTitle.Typeface = CustomTypefaces.RobotoBold;

            btnBack.Click += (sender, args) => Finish();

            GetData();

            slRefresh.Refresh += (sender, args) => GetData();

            lvNotifications.ItemClick += (sender, args) =>
            {
                var model = adapter.Items[args.Position];
                Navigator.HandleNotificationTap(model.MetaData, model.Title, model.Message);
            };
        }

        

        void GetData()
        {
            var apiTask = new ServiceApi().GetNotifcations();
            apiTask.HandleError(this);
            apiTask.OnSucess(this, (response) =>
            {
                adapter = new CustomListAdapter<UserNotification>(response.Result, GetView);
                lvNotifications.Adapter = adapter;
                slRefresh.Refreshing = false;
                var apiTask2 = new ServiceApi().MarkNotificationsAsRead();
                apiTask2.HandleError(this);
            });
        }

        View GetView(UserNotification item, int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = LayoutInflater.Inflate(Resource.Layout.NotificationItem, null);
            }

            if (item.IsRead)
            {
                view.FindViewById<ImageView>(Resource.Id.imgNew).Visibility = ViewStates.Gone;
            }
            else
            {
                view.FindViewById<ImageView>(Resource.Id.imgNew).Visibility = ViewStates.Visible;
            }

            view.FindViewById<TextView>(Resource.Id.lblTimeAgo).Text = TimeAgoHelper.GetTimeAgo(item.DateCreatedUtc);
            view.FindViewById<TextView>(Resource.Id.lblMessage).Text = item.Title;
            
            var profileImageView = view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile);
            profileImageView.Tag?.CancelPendingTask(item.ProfileUrl);
            var task = ImageService.Instance.LoadUrl(item.ProfileUrl)
                .Retry(3, 300)
                .Transform(new CircleTransformation())
                .DownSample(100)
                .LoadingPlaceholder(Resource.Drawable.DefProfPic.ToString(), ImageSource.CompiledResource)
                .Into(profileImageView);

            profileImageView.Tag = new ImageLoaderHelper(task);

            return view;
        }
    }
}