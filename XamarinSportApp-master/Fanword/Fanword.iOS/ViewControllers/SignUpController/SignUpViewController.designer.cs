// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Fanword.iOS
{
    [Register ("SignUpViewController")]
    partial class SignUpViewController
    {
        [Outlet]
        UIKit.UIButton btnBack { get; set; }


        [Outlet]
        UIKit.UIButton btnSignUp { get; set; }


        [Outlet]
        UIKit.UIButton btnTermsOfUse { get; set; }


        [Outlet]
        UIKit.UITextField txtEmail { get; set; }


        [Outlet]
        UIKit.UITextField txtFirstName { get; set; }


        [Outlet]
        UIKit.UITextField txtLastName { get; set; }


        [Outlet]
        UIKit.UITextField txtPassword { get; set; }


        [Outlet]
        UIKit.UIView vwFacebookContainer { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnBack != null) {
                btnBack.Dispose ();
                btnBack = null;
            }

            if (btnSignUp != null) {
                btnSignUp.Dispose ();
                btnSignUp = null;
            }

            if (btnTermsOfUse != null) {
                btnTermsOfUse.Dispose ();
                btnTermsOfUse = null;
            }

            if (txtEmail != null) {
                txtEmail.Dispose ();
                txtEmail = null;
            }

            if (txtFirstName != null) {
                txtFirstName.Dispose ();
                txtFirstName = null;
            }

            if (txtLastName != null) {
                txtLastName.Dispose ();
                txtLastName = null;
            }

            if (txtPassword != null) {
                txtPassword.Dispose ();
                txtPassword = null;
            }

            if (vwFacebookContainer != null) {
                vwFacebookContainer.Dispose ();
                vwFacebookContainer = null;
            }
        }
    }
}