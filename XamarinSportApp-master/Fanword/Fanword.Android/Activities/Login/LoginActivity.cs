using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Fanword.Shared;
using Mobile.Extensions.Android.Extensions;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Login.Widget;
using Android.Util;
using Fanword.Android.Activities.ForgotPassword;
using Java.Security;
using Mixpanel.Android.MpMetrics;
using System;
using System.Linq;
using System.Text;
using Android.Runtime;
using System.Resources;
using Plugin.Dialog;
using Plugin.PushNotifications.Shared;
//using Plugin.Dialog;

namespace Fanword.Android
{
    [Activity(Label = "LoginActivity", ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.StateHidden)]
    public class LoginActivity : BaseActivity, IFacebookCallback
    {
        private EditText txtEmail { get; set; }
        private EditText txtPassword { get; set; }
        private Button btnForgotPassword { get; set; }
        private Button btnSignIn { get; set; }
        private Button btnRegister { get; set; }
        LoginButton btnFacebook { get; set; }
        ICallbackManager manager;
        public const string MIXPANEL_TOKEN = "298c35129fd8b5dd8d197a4cd395fab5";


        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginLayout);
            FacebookSdk.SdkInitialize(this);
            this.PopulateViewProperties();

            SetupViewBindings();
        }

        void SetupViewBindings()
        {
           /* PackageInfo info = PackageManager.GetPackageInfo(PackageName, global::Android.Content.PM.PackageInfoFlags.Signatures);
            foreach (global::Android.Content.PM.Signature signature in info.Signatures)
            {
                MessageDigest md = MessageDigest.GetInstance("SHA");
                md.Update(signature.ToByteArray());
                var hash = Base64.EncodeToString(md.Digest(), Base64Flags.Default);
                int j = 1;
            }*/
            var drawable = Resources.GetDrawable(Resource.Drawable.FacebookRoundedBackground);
            drawable.SetBounds(0, 0, drawable.IntrinsicWidth, drawable.IntrinsicHeight);
            btnFacebook.SetCompoundDrawables(drawable, drawable, drawable, drawable);
            btnFacebook.CompoundDrawablePadding = 24;
            btnFacebook.SetReadPermissions(new List<string>() { "email", "public_profile", "user_friends" });
            manager = CallbackManagerFactory.Create();
            btnFacebook.RegisterCallback(manager, this);

#if DEBUG
            txtEmail.Text = "support@agilx.com"; //"dkprajapati18@gmail.com";
            txtPassword.Text = "Password$1"; //"Darshan@1234";
#endif

            btnSignIn.Click += (sender, args) =>
            {
                ShowProgressDialog();

                MixpanelAPI mixpanel = MixpanelAPI.GetInstance(this, MIXPANEL_TOKEN);

                mixpanel.TimeEvent("SignIn");

                mixpanel.Track("SignIn");

                var apiTask = new ServiceApi().Login(txtEmail.Text, txtPassword.Text);
                apiTask.HandleError(this);
                apiTask.OnSucess(this, (response) =>
                {
                    HideProgressDialog();
                    StartActivity(typeof(MainActivity));
                    Finish();
                });
            };
            
            btnRegister.Click += (sender, args) =>
            {
                StartActivity(typeof(SignUpActivity));
            };

            btnForgotPassword.Click += (sender, args) => StartActivity(typeof(ForgotPasswordActivity));
        }

        public void OnCancel()
        {
        }

        public void OnError(FacebookException p0)
        {
            new AlertDialog.Builder(this).SetTitle("Error").SetMessage(p0.Message).SetNeutralButton("Ok", (sender, e) => { }).Show();
        }

        public void OnSuccess(Java.Lang.Object p0)
        {
            ShowProgressDialog();
            var loginResult = p0 as LoginResult;
            var apiTask = new ServiceApi().RegisterFacebook(loginResult.AccessToken.Token);
            apiTask.HandleError(this);
            apiTask.OnSucess(this, (response) =>
           {
               HideProgressDialog();
               var user = new ServiceApi().GetMyUser();
               if (user == response)
               {
                   StartActivity(typeof(MainActivity));
                   Finish();
               }
               else
               {
                   StartActivity(typeof(MainActivity));
               }
               
           });
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            manager.OnActivityResult(requestCode, (int)resultCode, data);
        }
    }
}
