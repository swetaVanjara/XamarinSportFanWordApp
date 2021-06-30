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
using Fanword.Android.Activities.OnboardingAthleteTeam;
using Fanword.Android.TypeFaces;
using Fanword.Poco.Models;
using Fanword.Shared;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Views;
using Mobile.Extensions.Android.Extensions;
using Mobile.Extensions.Extensions;
using Plugin.Settings;

namespace Fanword.Android.Activities.StudentAthleteInfo
{
    [Activity(Label = "StudentAthleteInfoActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class StudentAthleteInfoActivity : BaseActivity
    {
        private ImageButton btnBack { get; set; }
        private ImageViewAsync imgProfile { get; set; }
        private Button btnEdit { get; set; }
        private TextView lblSportName { get; set; }
        private TextView lblSchoolName { get; set; }
        private TextView lblYears { get; set; }
        private LinearLayout llAthlete { get; set; }
        private ImageButton btnDelete { get; set; }
        private TextView lblTitle { get; set; }
        private TextView lblCurrent { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.StudentAthleteInfoLayout);
            this.PopulateViewProperties();
            SetupViewBindings();
        }

        void SetupViewBindings()
        {
            lblSchoolName.Typeface = CustomTypefaces.RobotoBold;
            lblYears.Typeface = CustomTypefaces.RobotoBold;
            lblCurrent.Typeface = CustomTypefaces.RobotoBold;
            lblTitle.Typeface = CustomTypefaces.RobotoBold;

            btnBack.Click += (sender, args) => Finish();
            var user = CrossSettings.Current.GetValueOrDefaultJson<User>("User");

            if (string.IsNullOrEmpty(user.AthleteTeamId))
            {
                llAthlete.Visibility = ViewStates.Gone;
                btnEdit.Text = "Add";
                btnDelete.Visibility = ViewStates.Gone;
            }
            else
            {
                if (!string.IsNullOrEmpty(user.AthleteProfileUrl))
                    ImageService.Instance.LoadUrl(user.AthleteProfileUrl).Retry(3, 300).Transform(new CircleTransformation()).Into(imgProfile);

                lblSchoolName.Text = user.AthleteSchool;
                lblSportName.Text = user.AthleteSport;
                lblYears.Text = user.AthleteStartDateUtc.Year + " - " + (user.AthleteEndDateUtc != null ? user.AthleteEndDateUtc?.Year.ToString() : "Present");
            }

            btnEdit.Click += (sender, args) =>
            {
                Intent intent = new Intent(this, typeof(OnboardingAthleteTeamActivity));
                intent.PutExtra("IsEdit", true);
                StartActivity(intent);
            };

            btnDelete.Click += (sender, args) =>
            {
                ShowProgressDialog();
                var apiTask = new ServiceApi().DeleteAthlete();
                apiTask.HandleError(this);
                apiTask.OnSucess(this, (response) =>
                {
                    HideProgressDialog();
                    llAthlete.Visibility = ViewStates.Gone;
                    btnEdit.Text = "Add";
                    btnDelete.Visibility = ViewStates.Gone;
                });
            };
        }
    }
}