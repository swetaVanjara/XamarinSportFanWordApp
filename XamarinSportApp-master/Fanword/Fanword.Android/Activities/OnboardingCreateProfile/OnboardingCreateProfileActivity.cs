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
using Fanword.Android.Activities.OnboardingAthleteTeam;
using Fanword.Android.Extensions;
using Fanword.Poco.Models;
using Fanword.Shared;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Views;
using Mobile.Extensions.Android.Extensions;
using Mobile.Extensions.Extensions;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Settings;

namespace Fanword.Android
{
    [Activity(Label = "OnboardingCreateProfileActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class OnboardingCreateProfileActivity : BaseActivity
    {
        private ImageButton btnYes { get; set; }
        private ImageButton btnNo { get; set; }
        private Button btnUpload { get; set; }
        private Button btnDone { get; set; }
        private ImageViewAsync imgProfile { get; set; }
        private TextView lblName { get; set; }
        private bool? IsAthlete;
        private string ImageFilePath;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.OnboardingCreateProfileLayout);
            this.PopulateViewProperties();
            SetupViewBindings();
        }

        void SetupViewBindings()
        {
            var user = CrossSettings.Current.GetValueOrDefaultJson<User>("User");
            lblName.Text = user.FirstName + " " + user.LastName;

            btnUpload.Click += (sender, args) =>
            {
                new AlertDialog.Builder(this)
                    .SetTitle("Add Photo")
                    .SetMessage("Choose a way to add a photo")
                    .SetNegativeButton("Cancel", (o, eventArgs) => { })
                    .SetPositiveButton("Take Photo", (o, eventArgs) => TakePhoto())
                    .SetNeutralButton("Pick Photo", (o, eventArgs) => PickPhoto())
                    .Show();
            };

            btnNo.Click += (sender, args) =>
            {
                IsAthlete = false;
                SetCheckboxes();
            };

            btnYes.Click += (sender, args) =>
            {
                IsAthlete = true;
                SetCheckboxes();
            };

            btnDone.Click += async (sender, args) =>
            {
                if (IsAthlete == true)
                {
                    // go somewhere
                    Intent intent = new Intent(this, typeof(OnboardingAthleteTeamActivity));
                    intent.PutExtra("ImageFilePath", ImageFilePath);
                    StartActivity(intent);
                }
                else if (IsAthlete == false)
                {
                    ShowProgressDialog();

                    var error = await new ServiceApi().SaveUser(ImageFilePath);
                    HideProgressDialog();

                    if (error.DisplayErrorMessage(this))
                        return;

                    StartActivity(typeof(MainActivity));
                    Finish();
                }
            };
        }

        async void TakePhoto()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                new AlertDialog.Builder(this).SetTitle("Camera").SetMessage("Camera Not Available").SetPositiveButton("Ok", (sender, args) => { }).Show();
                return;
            }
            var file = await AppMedia.TakePhotoAsync(50, 70);
            AddFile(file);

        }

        async void PickPhoto()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                new AlertDialog.Builder(this).SetTitle("Photo Library").SetMessage("Photo Library Not Available").SetPositiveButton("Ok", (sender, args) => { }).Show();
                return;
            }
            var file = await AppMedia.PickPhotoAsync(50, 70);
            AddFile(file);

        }

        void AddFile(MediaFile file)
        {
            if (file != null)
            {
                ImageFilePath = file.Path;
                ImageService.Instance.LoadFile(ImageFilePath).Transform(new CircleTransformation()).Into(imgProfile);
            }
        }

        void SetCheckboxes()
        {
            btnDone.SetBackgroundColor(new Color(249, 95, 6));
            btnDone.SetTextColor(Color.White);
            if (IsAthlete == true)
            {
                btnNo.SetImageResource(Resource.Drawable.CheckNO);
                btnYes.SetImageResource(Resource.Drawable.CheckYES);
                btnDone.Text = "Next";
            }   
            else if (IsAthlete == false)
            {
                btnNo.SetImageResource(Resource.Drawable.CheckYES);
                btnYes.SetImageResource(Resource.Drawable.CheckNO);
                btnDone.Text = "Done";
            }
        }
    }
}