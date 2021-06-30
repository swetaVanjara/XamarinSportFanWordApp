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
    [Register ("StudentAthleteInfoViewController")]
    partial class StudentAthleteInfoViewController
    {
        [Outlet]
        UIKit.UIButton btnBack { get; set; }


        [Outlet]
        UIKit.UIButton btnDelete { get; set; }


        [Outlet]
        UIKit.UIButton btnEdit { get; set; }


        [Outlet]
        UIKit.UIImageView imgProfile { get; set; }


        [Outlet]
        UIKit.UILabel lblSchoolName { get; set; }


        [Outlet]
        UIKit.UILabel lblSportName { get; set; }


        [Outlet]
        UIKit.UILabel lblYears { get; set; }


        [Outlet]
        UIKit.UIStackView svAthlete { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnBack != null) {
                btnBack.Dispose ();
                btnBack = null;
            }

            if (btnDelete != null) {
                btnDelete.Dispose ();
                btnDelete = null;
            }

            if (btnEdit != null) {
                btnEdit.Dispose ();
                btnEdit = null;
            }

            if (imgProfile != null) {
                imgProfile.Dispose ();
                imgProfile = null;
            }

            if (lblSchoolName != null) {
                lblSchoolName.Dispose ();
                lblSchoolName = null;
            }

            if (lblSportName != null) {
                lblSportName.Dispose ();
                lblSportName = null;
            }

            if (lblYears != null) {
                lblYears.Dispose ();
                lblYears = null;
            }

            if (svAthlete != null) {
                svAthlete.Dispose ();
                svAthlete = null;
            }
        }
    }
}