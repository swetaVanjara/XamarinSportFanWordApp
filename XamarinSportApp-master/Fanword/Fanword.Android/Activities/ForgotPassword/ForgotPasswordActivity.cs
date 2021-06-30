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
using Fanword.Shared;
using Mobile.Extensions.Android.Extensions;
using Plugin.Dialog;

namespace Fanword.Android.Activities.ForgotPassword
{
    [Activity(Label = "ForgotPasswordActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ForgotPasswordActivity : BaseActivity
    {
        private ImageButton btnBack { get; set; }
        private Button btnResetPassword { get; set; }
        private EditText txtEmail { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ForgotPasswordLayout);
            this.PopulateViewProperties();
            SetupViewBindings();
        }

        void SetupViewBindings()
        {
            btnBack.Click += (sender, args) => Finish();

            btnResetPassword.Click += (sender, args) =>
            {
                if (string.IsNullOrEmpty(txtEmail.Text))
                    return;
                ShowProgressDialog();
                var apiTask = new ServiceApi().ForgotPassword(txtEmail.Text);
                apiTask.HandleError(this);
                apiTask.OnSucess(this, response =>
                {
                    HideProgressDialog();
                    new AlertDialog.Builder(this)
                        .SetTitle("Success")
                        .SetMessage("Check your email for password reset instructions")
                        .SetPositiveButton("Ok", (o, eventArgs) => Finish())
                        .Show();
                });
            };
        }
    }
}