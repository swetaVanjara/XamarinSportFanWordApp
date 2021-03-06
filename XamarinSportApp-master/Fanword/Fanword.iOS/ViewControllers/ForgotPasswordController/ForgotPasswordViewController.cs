// This file has been autogenerated from a class added in the UI designer.

using System;
using Fanword.Shared;
using Foundation;
using Mobile.Extensions.iOS.Extensions;
using UIKit;

namespace Fanword.iOS
{
	public partial class ForgotPasswordViewController : BaseViewController
	{
		public ForgotPasswordViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			btnBack.TouchUpInside += (sender, e) =>
			{
				NavigationController.PopViewController (true);
			};

			btnResetPassword.TouchUpInside += (sender, e) =>
			{
				if (string.IsNullOrEmpty (txtEmail.Text))
					return;

				LoadingScreen.Show ();
				var apiTask = new ServiceApi ().ForgotPassword (txtEmail.Text);
				apiTask.HandleError (LoadingScreen);
				apiTask.OnSucess (response =>
				{
					LoadingScreen.Hide ();
					var alertview = new UIAlertView ();
					alertview.Title = "Success";
					alertview.Message = "Check your email for password reset instructions";
					alertview.AddButton ("Ok");
					alertview.Clicked += (sender2, e2) => NavigationController.PopViewController (true);
					alertview.Show ();
				});
			};

		}
	}
}
