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
    [Register ("AthleteTeamCell")]
    partial class AthleteTeamCell
    {
        [Outlet]
        UIKit.UIImageView imgProfile { get; set; }


        [Outlet]
        UIKit.UILabel lblSchool { get; set; }


        [Outlet]
        UIKit.UILabel lblSport { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgProfile != null) {
                imgProfile.Dispose ();
                imgProfile = null;
            }

            if (lblSchool != null) {
                lblSchool.Dispose ();
                lblSchool = null;
            }

            if (lblSport != null) {
                lblSport.Dispose ();
                lblSport = null;
            }
        }
    }
}