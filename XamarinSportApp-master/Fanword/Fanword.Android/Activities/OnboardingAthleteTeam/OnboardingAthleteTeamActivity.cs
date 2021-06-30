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
using Fanword.Android.Activities.OnboardingAthleteSeason;
using Fanword.Android.Extensions;
using Fanword.Poco.Models;
using Fanword.Shared;
using FFImageLoading;
using FFImageLoading.Views;
using FFImageLoading.Work;
using Mobile.Extensions.Android.Adapters;
using Mobile.Extensions.Android.Extensions;
using Newtonsoft.Json;

namespace Fanword.Android.Activities.OnboardingAthleteTeam
{
    [Activity(Label = "OnboardingAthleteTeamActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class OnboardingAthleteTeamActivity : BaseActivity
    {
        private ImageButton btnBack { get; set; }
        private ListView lvTeams { get; set; }
        private EditText txtSearch { get; set; }
        private string ImageFilePath { get; set; }
        private bool isEdit { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.OnboardingAthleteTeamLayout);
            this.PopulateViewProperties();
            ImageFilePath = Intent.GetStringExtra("ImageFilePath");
            isEdit = Intent.GetBooleanExtra("IsEdit", false);
            SetupViewBindings();
        }

        void SetupViewBindings()
        {
            btnBack.Click += (sender, args) => Finish();
            lvTeams.ItemClick += (sender, e) =>
            {
                var item = lvTeams.GetItemAtPosition<AthleteTeam>(e.Position);
                Intent intent = new Intent(this, typeof(OnboardingAthleteSeasonActivity));
                intent.PutExtra("ImageFilePath", ImageFilePath);
                intent.PutExtra("AthleteTeam", JsonConvert.SerializeObject(item));
                intent.PutExtra("IsEdit", isEdit);
                StartActivity(intent);
            };

            txtSearch.TextChanged += (sender, args) => GetData();
        }

        void GetData()
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                var adapter = lvTeams.Adapter as CustomListAdapter<AthleteTeam>;
                if (adapter == null)
                    return;
                adapter.Items = new List<AthleteTeam>();
                adapter.NoContentEnabled = false;
                adapter.NotifyDataSetChanged();
            }
            else
            {
                var apiTask = new ServiceApi().GetAthleteTeams(txtSearch.Text);
                apiTask.HandleError(this);
                apiTask.OnSucess(this, response =>
                {
                    if (response.Result.SearchText == txtSearch.Text)
                    {
                        if (lvTeams.Adapter == null)
                        {
                            var apdater = new CustomListAdapter<AthleteTeam>(response.Result.AthleteTeams, GetView);
                            apdater.NoContentText = "No Teams Found";
                            lvTeams.Adapter = apdater;
                        }
                        else
                        {
                            var adapter = lvTeams.Adapter as CustomListAdapter<AthleteTeam>;
                            adapter.Items = response.Result.AthleteTeams;
                            adapter.NoContentEnabled = true;
                            adapter.NotifyDataSetChanged();
                        }
                    }
                });
            }
        }

        View GetView(AthleteTeam item, int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = LayoutInflater.Inflate(Resource.Layout.AthleteTeamItem, null);
            }

            view.FindViewById<TextView>(Resource.Id.lblSchoolName).Text = item.SchoolName;
            view.FindViewById<TextView>(Resource.Id.lblSportName).Text = item.SportName;

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