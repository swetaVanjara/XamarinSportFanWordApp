using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fanword.Shared;
using Java.Security;
using Mobile.Extensions.Android.Extensions;
using System.Collections.Generic;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Login.Widget;

namespace Fanword.Android
{
    [Activity (Label = "SignUpActivity", ScreenOrientation = ScreenOrientation.Portrait)]
	public class SignUpActivity : BaseActivity, IFacebookCallback
    {
		private ImageButton btnBack { get; set; }
		private EditText txtFirstName { get; set; }
		private EditText txtLastName { get; set; }
		private EditText txtEmail { get; set; }
		private EditText txtPassword { get; set; }
		private Button btnRegister { get; set; }
		private Button btnTermsOfUse { get; set; }

		LoginButton btnFacebook { get; set; }
		ICallbackManager manager;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			RequestWindowFeature (WindowFeatures.NoTitle);
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.SignUpActivity);
			this.PopulateViewProperties ();
            FacebookSdk.SdkInitialize(this);
			SetupViewBindings ();
		}

		void SetupViewBindings ()
		{
            PackageInfo info = PackageManager.GetPackageInfo(PackageName, global::Android.Content.PM.PackageInfoFlags.Signatures);
            foreach (global::Android.Content.PM.Signature signature in info.Signatures)
            {
                MessageDigest md = MessageDigest.GetInstance("SHA");
                md.Update(signature.ToByteArray());
                var hash = Base64.EncodeToString(md.Digest(), Base64Flags.Default);
                int j = 1;
            }
            var drawable = Resources.GetDrawable(Resource.Drawable.FacebookRoundedBackground);
            drawable.SetBounds(0, 0, drawable.IntrinsicWidth, drawable.IntrinsicHeight);
            btnFacebook.SetCompoundDrawables(drawable, drawable, drawable, drawable);
            btnFacebook.CompoundDrawablePadding = 24;
            btnFacebook.SetReadPermissions(new List<string>() { "email", "public_profile", "user_friends" });
            manager = CallbackManagerFactory.Create();
            btnFacebook.RegisterCallback(manager, this);
            btnBack.Click += (sender, args) => Finish ();
			btnRegister.Click += (sender, args) =>
			{
				ShowProgressDialog ();
				var apiTask = new ServiceApi ().Register (txtFirstName.Text, txtLastName.Text, txtEmail.Text, txtPassword.Text, txtPassword.Text);
				apiTask.HandleError (this);
				apiTask.OnSucess (this, (response) =>
				 {
					 HideProgressDialog ();
					 Intent intent = new Intent (this, typeof (OnboardingCreateProfileActivity));
					 intent.SetFlags (ActivityFlags.ClearTask | ActivityFlags.NewTask);
					 StartActivity (intent);
                     Finish();
				 });
			};

			btnTermsOfUse.Click += (sender, e) =>
			{
				Intent intent = new Intent (Intent.ActionView);
				intent.SetData (global::Android.Net.Uri.Parse ("https://docs.wixstatic.com/ugd/6c5b94_41c4e7c1ad7c49e5bb8be83a4a02fd60.pdf"));
				StartActivity (intent);
			};
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
                System.Diagnostics.Debug.WriteLine(response.Result.user);


                HideProgressDialog();
                StartActivity(typeof(OnboardingCreateProfileActivity));
                Finish();
            });
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            manager.OnActivityResult(requestCode, (int)resultCode, data);
        }
    }
}