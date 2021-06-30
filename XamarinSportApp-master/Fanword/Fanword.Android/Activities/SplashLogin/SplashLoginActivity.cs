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

namespace Fanword.Android
{ 
    [Activity(Label = "SplashLoginActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashLoginActivity : BaseActivity
    {
        private Button btnLogin { get; set; }
        private Button btnSignUp { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SplashLogin);

            btnLogin = FindViewById<Button>(Resource.Id.btnLoginPage);
            btnSignUp = FindViewById<Button>(Resource.Id.btnSignUpPage);

            btnLogin.Click += (sender, e) =>
            {
                StartActivity(typeof(LoginActivity));
            };

            btnSignUp.Click += (sender, e) =>
            {
                StartActivity(typeof(SignUpActivity));
            };
        }
    }
}