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
    [Register ("SchoolAthleteCell")]
    partial class SchoolAthleteCell
    {
        [Outlet]
        UIKit.UIButton btnFollow { get; set; }


        [Outlet]
        UIKit.UIImageView imgProfile { get; set; }


        [Outlet]
        UIKit.UILabel lblAthlete { get; set; }


        [Outlet]
        UIKit.UILabel lblName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnFollow != null) {
                btnFollow.Dispose ();
                btnFollow = null;
            }

            if (imgProfile != null) {
                imgProfile.Dispose ();
                imgProfile = null;
            }

            if (lblAthlete != null) {
                lblAthlete.Dispose ();
                lblAthlete = null;
            }

            if (lblName != null) {
                lblName.Dispose ();
                lblName = null;
            }
        }
    }
}