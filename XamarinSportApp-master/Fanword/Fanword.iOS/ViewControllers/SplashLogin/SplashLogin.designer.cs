// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Fanword.iOS
{
    [Register ("SplashLogin")]
    partial class SplashLogin
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSignInPage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSignUpPage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView LogoWhite { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnSignInPage != null) {
                btnSignInPage.Dispose ();
                btnSignInPage = null;
            }

            if (btnSignUpPage != null) {
                btnSignUpPage.Dispose ();
                btnSignUpPage = null;
            }

            if (LogoWhite != null) {
                LogoWhite.Dispose ();
                LogoWhite = null;
            }
        }
    }
}