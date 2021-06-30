using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fanword.Android.Activities.StudentAthleteInfo;
using Fanword.Android.Extensions;
using Fanword.Poco.Models;
using Fanword.Shared;
using FFImageLoading;
using FFImageLoading.Views;
using Mobile.Extensions.Android.Adapters;
using Mobile.Extensions.Android.Extensions;
using Mobile.Extensions.Extensions;
using Plugin.Settings;

namespace Fanword.Android.Activities.OnboardingAthleteSeason
{
    [Activity(Label = "OnboardingAthleteSeasonActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class OnboardingAthleteSeasonActivity : BaseActivity
    {
        private AthleteTeam AthleteTeam { get; set; }
        private ImageButton btnBack { get; set; }
        private ListView lvFrom { get; set; }
        private ListView lvUntil { get; set; }
        private RelativeLayout rlDivider { get; set; }
        private ImageButton btnTrash { get; set; }
        private ImageViewAsync imgProfile { get; set; }
        private TextView lblSchoolName { get; set; }
        private TextView lblSportName { get; set; }
        private Button btnFrom { get; set; }
        private Button btnUntil { get; set; }
        private Button btnDone { get; set; }

        private DateTime? fromDate;
        private DateTime? untilDate;
        private bool untilDatePicked;

        private string ImageFilePath { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.OnboardingAthleteSeasonLayout);
            this.PopulateViewProperties();
            ImageFilePath = Intent.GetStringExtra("ImageFilePath");
            AthleteTeam = Intent.GetObject<AthleteTeam>("AthleteTeam");
            SetupViewBindings();
        }

        void SetupViewBindings()
        {
            btnBack.Click += (sender, args) => Finish();

            ImageService.Instance.LoadUrl(AthleteTeam.ProfilePublicUrl).Into(imgProfile);
            lblSportName.Text = AthleteTeam.SportName;
            lblSchoolName.Text = AthleteTeam.SchoolName;

            lvFrom.Visibility = ViewStates.Invisible;
            lvUntil.Visibility = ViewStates.Invisible;
            rlDivider.Visibility = ViewStates.Invisible;

            btnTrash.Click += (sender, args) => Finish();

            btnFrom.Click += (sender, args) =>
            {
                lvFrom.Visibility = ViewStates.Visible;
                rlDivider.Visibility = ViewStates.Visible;
            };

            btnUntil.Click += (sender, args) =>
            {
                lvUntil.Visibility = ViewStates.Visible;
                rlDivider.Visibility = ViewStates.Visible;
            };

            List<int> years = Enumerable.Range(DateTime.Now.Year - 65, 66).OrderByDescending(m => m).ToList();
            lvFrom.Adapter = new CustomListAdapter<int>(years, GetView);

            List<int> years2 = years.ToList();
            years2.Insert(0, -1);
            lvUntil.Adapter = new CustomListAdapter<int>(years2, GetView);

            lvUntil.ItemClick += (sender, args) =>
            {
                var item = lvUntil.GetItemAtPosition<int>(args.Position);
                untilDatePicked = true;
                if (item == -1)
                {
                    btnUntil.Text = "Still competing";
                    untilDate = null;
                }
                else
                {
                    btnUntil.Text = item.ToString();
                    untilDate = new DateTime(item, 1, 1);

                }
                lvUntil.Visibility = ViewStates.Invisible;

                if (lvFrom.Visibility == ViewStates.Invisible)
                {
                    rlDivider.Visibility = ViewStates.Invisible;
                }

                CheckIfDone();
            };

            lvFrom.ItemClick += (sender, args) =>
            {
                var item = lvFrom.GetItemAtPosition<int>(args.Position);

                btnFrom.Text = item.ToString();

                fromDate = new DateTime(item, 1, 1);

                lvFrom.Visibility = ViewStates.Invisible;

                if (lvUntil.Visibility == ViewStates.Invisible)
                {
                    rlDivider.Visibility = ViewStates.Invisible;
                }
                CheckIfDone();
            };

            btnDone.Enabled = false;

            btnDone.Click += async (sender, args) =>
            {
                ShowProgressDialog();
                var error = await new ServiceApi().SaveAthleteUser(ImageFilePath, fromDate.Value, untilDate, AthleteTeam.Id);
                HideProgressDialog();

                if (error.DisplayErrorMessage(this))
                    return;

                if (Intent.GetBooleanExtra("IsEdit", false))
                {
                    Intent intent = new Intent(this, typeof(StudentAthleteInfoActivity));
                    intent.SetFlags(ActivityFlags.ClearTop);
                    StartActivity(intent);
                }
                else
                {
                    StartActivity(typeof(MainActivity));
                    Finish();
                }
            };
        }

        void CheckIfDone()
        {
            if (fromDate != null && untilDatePicked)
            {
                btnDone.Enabled = true;
                btnDone.SetBackgroundColor(new Color(249, 95, 6));
                btnDone.SetTextColor(Color.White);
                btnFrom.SetTextColor(new Color(249, 95, 6));
                btnUntil.SetTextColor(new Color(249, 95, 6));
            }
        }

        View GetView(int item, int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = LayoutInflater.Inflate(Resource.Layout.AthleteSeasonItem, null);
            }
            if (item == -1)
            {
                view.FindViewById<TextView>(Resource.Id.lblYear).Text = "Still competing";
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.lblYear).Text = item.ToString();
            }

            return view;
        }
    }
}