using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Test.Suitebuilder.Annotation;
using Android.Views;
using Android.Widget;
using Fanword.Android.Extensions;
using Fanword.Android.Shared;
using Fanword.Shared;
using Fanword.Shared.Models;
using Fanword.Poco.Models;
using Fanword.Shared.Helpers;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Views;
using FFImageLoading.Work;
using Mobile.Extensions.Android.Adapters;
using Mobile.Extensions.Android.Extensions;
using Android.Views.InputMethods;
using System.Threading.Tasks;
using Fanword.Android.Fragments;
using Square.Picasso;

namespace Fanword.Android.Activities.Search
{
    [Activity(Label = "SearchActivity", ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.StateHidden)]
    public class SearchActivity : BaseActivity
    {
        private ImageButton btnBack { get; set; }
        private CustomListAdapter<GlobalSearchItem> adapter;
        private ListView lvSearch { get; set; }
        private EditText txtSearch { get; set; }
        private ImageView imgSearch { get; set; }
        private FeedType type;
        private bool useType;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SearchLayout);
            this.PopulateViewProperties();
            SetupViewBindings();
        }

        void SetupViewBindings()
        {
            type = (FeedType)Intent.GetIntExtra("FeedType", 0);
            useType = Intent.GetBooleanExtra("UseType", false);
            btnBack.Click += (sender, args) => Finish();
            
            GetData();

            txtSearch.TextChanged += (sender, args) =>
            {
                GetData();
            };

            imgSearch.Click += (sender, e) => 
            {
				txtSearch.RequestFocus();
				InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
				imm.ShowSoftInput(txtSearch, ShowFlags.Implicit);
            };
        }

        void GetData()
        {
            Task<GlobalSearch> apiTask;
            if(useType)
            {
				apiTask = new ServiceApi().SearchByType(txtSearch.Text, type);
            }
            else
            {
				apiTask = new ServiceApi().Search(txtSearch.Text);
            }

            apiTask.HandleError(this);
            apiTask.OnSucess(this, response =>
            {
                if (txtSearch.Text == (response.Result.SearchText ?? ""))
                {
                    List<GlobalSearchItem> items = response.Result.Results;
                    adapter = new CustomListAdapter<GlobalSearchItem>(items, GetView);
                    adapter.SetViewTypes(GetType, 2);
                    lvSearch.Adapter = adapter;
                }
            });

        }

        int GetType(GlobalSearchItem item, int position)
        {
            if (string.IsNullOrEmpty(item.Id))
                return 0;
            else
                return 1;
        }

        View GetView(GlobalSearchItem item, int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                if (GetType(item, position) == 0)
                {
                    view = LayoutInflater.Inflate(Resource.Layout.SearchHeaderItem, null);
                }
                else
                {
                    view = LayoutInflater.Inflate(Resource.Layout.SearchItem, null);
                    view.FindViewById<Button>(Resource.Id.btnFollow).Click += (sender, args) =>
                    {
                        var model = adapter.Items[(int) view.Tag];
                        Fanword.Android.Shared.Follower.FollowToggle(this, sender as Button, model, model.Id, model.Type);
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
            }
            view.Tag = position;
            
            if (GetType(item, position) == 0)
            {
                if (!string.IsNullOrEmpty(item.Title))
                {
                    view.FindViewById<TextView>(Resource.Id.lblTitle).Text = item.Title;
                }
                else
                {
                    var str = string.Concat(item.Type.ToString().Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' '); // Add spaces
                    view.FindViewById<TextView>(Resource.Id.lblTitle).Text = str + "s";
                }
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.lblTitle).Text = item.Title;
                view.FindViewById<TextView>(Resource.Id.lblSubtitle).Text = item.Subtitle;
                view.FindViewById<TextView>(Resource.Id.lblSubtitle).Visibility = string.IsNullOrEmpty(item.Subtitle) ? ViewStates.Gone : ViewStates.Visible;
                view.FindViewById<TextView>(Resource.Id.lblFollowers).Text = LargeValueHelper.GetString(item.Followers) + " followers";
                var profileImageView = view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile);
                Picasso.With(this).Load(item.ProfileUrl).Resize(100, 100).CenterCrop().OnlyScaleDown().Placeholder(Resource.Drawable.DefProfPic).Transform(new PiccasoCircleTransformation()).Into(profileImageView);

                /*
                
                profileImageView.Tag?.CancelPendingTask(item.ProfileUrl);
                var task = ImageService.Instance.LoadUrl(item.ProfileUrl)
                    .Retry(3, 300)
                    .LoadingPlaceholder(Resource.Drawable.DefProfPic.ToString(), ImageSource.CompiledResource)
                    .DownSample(100)
                    .Transform(new CircleTransformation())
                    .Into(profileImageView);

                profileImageView.Tag = new ImageLoaderHelper(task);*/

                Views.SetFollowed(view.FindViewById<Button>(Resource.Id.btnFollow), item.IsFollowing);
            }

            return view;
        }

        void GoToProfile(GlobalSearchItem item)
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