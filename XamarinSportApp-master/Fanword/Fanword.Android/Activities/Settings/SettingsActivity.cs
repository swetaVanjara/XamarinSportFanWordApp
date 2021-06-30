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
using Fanword.Android.Activities.BasicProfileInfo;
using Fanword.Android.Activities.StudentAthleteInfo;
using Fanword.Android.TypeFaces;
using Fanword.Poco.Models;
using Mobile.Extensions.Android.Extensions;
using Newtonsoft.Json;

namespace Fanword.Android.Activities.Settings
{
    [Activity(Label = "SettingsActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SettingsActivity : BaseActivity
    {
        private ImageButton btnBack { get; set; }
        private LinearLayout llProfile { get; set; }
        private LinearLayout llAthlete { get; set; }
        private TextView lblTitle { get; set; }
        private TextView lblBasic { get; set; }
        private TextView lblAthlete { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SettingsLayout);
            this.PopulateViewProperties();
            SetupViewBindings();
        }

        void SetupViewBindings()
        {
            lblTitle.Typeface = CustomTypefaces.RobotoBold;
            lblBasic.Typeface = CustomTypefaces.RobotoBold;
            lblAthlete.Typeface = CustomTypefaces.RobotoBold;

            btnBack.Click += (sender, args) => Finish();
            llProfile.Click += (sender, args) => StartActivity(typeof(BasicProfileInfoActivity));
            llAthlete.Click += (sender, args) => StartActivity(typeof(StudentAthleteInfoActivity));
        }

    }
}