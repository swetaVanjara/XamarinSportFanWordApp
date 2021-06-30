using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fanword.Android.TypeFaces;
using Fanword.Poco.Models;
using Fanword.Shared;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Views;
using Mobile.Extensions.Android.Extensions;
using Mobile.Extensions.Extensions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Settings;

namespace Fanword.Android.Activities.BasicProfileInfo
{
    [Activity(Label = "BasicProfileInfoActivity")]
    public class BasicProfileInfoActivity : BaseActivity
    {
        private ImageButton btnBack { get; set; }
        private ImageViewAsync imgProfile { get; set; }
        private Button btnUpload { get; set; }
        private TextView lblFirstName { get; set; }
        private TextView lblLastName { get; set; }
        private TextView lblEmail { get; set; }
        private TextView lblTitle { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.BasicProfileInfoLayout);
            this.PopulateViewProperties();
            SetupViewBindings();
        }

        void SetupViewBindings()
        {
            lblTitle.Typeface = CustomTypefaces.RobotoBold;
            lblEmail.Typeface = CustomTypefaces.RobotoBold;
            lblFirstName.Typeface = CustomTypefaces.RobotoBold;
            lblLastName.Typeface = CustomTypefaces.RobotoBold;

            btnBack.Click += (sender, args) => Finish();
            var user = CrossSettings.Current.GetValueOrDefaultJson<User>("User");
            if (!string.IsNullOrEmpty(user.ProfileUrl))
                ImageService.Instance.LoadUrl(user.ProfileUrl).Retry(3, 300).Transform(new CircleTransformation()).Into(imgProfile);

            lblEmail.Text = user.Email;
            lblFirstName.Text = user.FirstName;
            lblLastName.Text = user.LastName;

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

        async void AddFile(MediaFile file)
        {
            if (file != null)
            {
                ShowProgressDialog();
                ImageService.Instance.LoadFile(file.Path).Transform(new CircleTransformation()).Into(imgProfile);
                await new ServiceApi().SaveUser(file.Path);
                HideProgressDialog();
            }
        }
    }
}