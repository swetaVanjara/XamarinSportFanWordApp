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
    [Register ("OnboardingCreateProfileViewController")]
    partial class OnboardingCreateProfileViewController
    {
        [Outlet]
        UIKit.UIButton btnDone { get; set; }


        [Outlet]
        UIKit.UIButton btnNo { get; set; }


        [Outlet]
        UIKit.UIButton btnUpload { get; set; }


        [Outlet]
        UIKit.UIButton btnYes { get; set; }


        [Outlet]
        UIKit.UIImageView imgProfile { get; set; }


        [Outlet]
        UIKit.UILabel lblName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnDone != null) {
                btnDone.Dispose ();
                btnDone = null;
            }

            if (btnNo != null) {
                btnNo.Dispose ();
                btnNo = null;
            }

            if (btnUpload != null) {
                btnUpload.Dispose ();
                btnUpload = null;
            }

            if (btnYes != null) {
                btnYes.Dispose ();
                btnYes = null;
            }

            if (imgProfile != null) {
                imgProfile.Dispose ();
                imgProfile = null;
            }

            if (lblName != null) {
                lblName.Dispose ();
                lblName = null;
            }
        }
    }
}