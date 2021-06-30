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
    [Register ("BasicInfoViewContoller")]
    partial class BasicInfoViewContoller
    {
        [Outlet]
        UIKit.UIButton btnBack { get; set; }


        [Outlet]
        UIKit.UIButton btnUpload { get; set; }


        [Outlet]
        UIKit.UIImageView imgProfile { get; set; }


        [Outlet]
        UIKit.UILabel lblEmail { get; set; }


        [Outlet]
        UIKit.UILabel lblFirstName { get; set; }


        [Outlet]
        UIKit.UILabel lblLastName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnBack != null) {
                btnBack.Dispose ();
                btnBack = null;
            }

            if (btnUpload != null) {
                btnUpload.Dispose ();
                btnUpload = null;
            }

            if (imgProfile != null) {
                imgProfile.Dispose ();
                imgProfile = null;
            }

            if (lblEmail != null) {
                lblEmail.Dispose ();
                lblEmail = null;
            }

            if (lblFirstName != null) {
                lblFirstName.Dispose ();
                lblFirstName = null;
            }

            if (lblLastName != null) {
                lblLastName.Dispose ();
                lblLastName = null;
            }
        }
    }
}