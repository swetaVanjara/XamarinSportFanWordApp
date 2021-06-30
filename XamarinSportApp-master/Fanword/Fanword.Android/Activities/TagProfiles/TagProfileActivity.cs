using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fanword.Android.Activities.TagEvents;
using Fanword.Android.Extensions;
using Fanword.Android.TypeFaces;
using Fanword.Poco.Models;
using Fanword.Shared;
using FFImageLoading;
using FFImageLoading.Views;
using FFImageLoading.Work;
using Mobile.Extensions.Android.Adapters;
using Mobile.Extensions.Android.Extensions;
using Newtonsoft.Json;

namespace Fanword.Android.Activities.TagProfiles
{
    [Activity(Label = "TagProfileActivity", ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.StateHidden)]
    public class TagProfileActivity : BaseActivity
    {
        private ImageButton btnBack { get; set; }
        private Button btnNext { get; set; }
        private EditText txtSearch { get; set; }
        private ListView lvProfiles { get; set; }
        private TextView lblProfiles { get; set; }
        private TextView lblTitle { get; set; }

        private CustomListAdapter<Profile> adapter;
        Post Post;
        ProfileSearch result;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TagProfileLayout);
            this.PopulateViewProperties();
            Post = JsonConvert.DeserializeObject<Post>(Intent.GetStringExtra("Post"));
            SetupViewBindings();
        }

        void SetupViewBindings()
        {
            btnBack.Click += (sender, e) => Finish();

            lblTitle.Typeface = CustomTypefaces.RobotoBold;

            adapter = new CustomListAdapter<Profile>(new List<Profile>(), GetView);
            lvProfiles.ItemClick += (sender, e2) =>
            {
                var e = adapter.Items[e2.Position];
                if (result.TeamProfiles.Any(m => m.Id == e.Id))
                {
                    if (Post.Teams.Contains((e.Id)))
                    {
                        Post.Teams.Remove(e.Id);
                    }
                    else
                    {
                        Post.Teams.Add(e.Id);
                    }

                }
                else if (result.SchoolProfiles.Any(m => m.Id == e.Id))
                {
                    if (Post.Schools.Contains((e.Id)))
                    {
                        Post.Schools.Remove(e.Id);
                    }
                    else
                    {
                        Post.Schools.Add(e.Id);
                    }
                }
                else if (result.SportProfile.Any(m => m.Id == e.Id))
                {
                    if (Post.Sports.Contains((e.Id)))
                    {
                        Post.Sports.Remove(e.Id);
                    }
                    else
                    {
                        Post.Sports.Add(e.Id);
                    }
                }
                adapter.NotifyDataSetChanged();
            };

            lvProfiles.Adapter = adapter;

            txtSearch.TextChanged += (sender, e) =>
            {
                GetData();
            };

            btnNext.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(TagEventsActivity));
                intent.PutExtra("Post", JsonConvert.SerializeObject(Post));
                StartActivity(intent);
            };
            ShowProgressDialog();
            GetData();

        }

        void GetData()
        {
            var feedType = Post.ContentSourceId != null
                ? FeedType.ContentSource
                : Post.TeamId != null
                    ? FeedType.Team
                    : Post.SchoolId != null
                        ? FeedType.School
                        : FeedType.User;

            var apiTask = new ServiceApi().SearchProfiles(txtSearch.Text, Post.TeamId ?? Post.SchoolId, feedType);
            apiTask.HandleError(this);
            apiTask.OnSucess(this, response =>
            {
                if (response.Result.SearchText == txtSearch.Text)
                {
                    result = response.Result;
                    adapter.Items = response.Result.TeamProfiles.Union(response.Result.SchoolProfiles).Union(response.Result.SportProfile).ToList();
                    adapter.NotifyDataSetChanged();
                    HideProgressDialog();
                }
            });
        }

        View GetView(Profile item, int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = LayoutInflater.Inflate(Resource.Layout.ProfileItem, null);
            }

            if (Post.Schools.Contains(item.Id) || Post.Teams.Contains(item.Id) || Post.Sports.Contains(item.Id))
            {
                view.FindViewById<ImageView>(Resource.Id.imgTagged).SetImageResource(Resource.Drawable.CheckYES);
            }
            else
            {
                view.FindViewById<ImageView>(Resource.Id.imgTagged).SetImageResource(Resource.Drawable.CheckNO);
            }

            view.FindViewById<TextView>(Resource.Id.lblTitle).Text = item.Name;
            view.FindViewById<TextView>(Resource.Id.lblSubtitle).Text = item.SubTitle;
            if (string.IsNullOrEmpty(item.SubTitle))
            {
                view.FindViewById<TextView>(Resource.Id.lblSubtitle).Visibility = ViewStates.Gone;
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.lblSubtitle).Visibility = ViewStates.Visible;
            }

            var profileImageView = view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile);
            profileImageView.Tag?.CancelPendingTask(item.ProfilePublicUrl);
            var task = ImageService.Instance.LoadUrl(item.ProfilePublicUrl)
                .Retry(3, 300)
                .LoadingPlaceholder(Resource.Drawable.DefProfPic.ToString(), ImageSource.CompiledResource)
                .Into(profileImageView);
            profileImageView.Tag = new ImageLoaderHelper(task);


            return view;
        }
    }
}