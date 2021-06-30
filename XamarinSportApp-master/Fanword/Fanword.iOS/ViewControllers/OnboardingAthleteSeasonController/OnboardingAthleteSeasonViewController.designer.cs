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
    [Register ("OnboardingAthleteSeasonViewController")]
    partial class OnboardingAthleteSeasonViewController
    {
        [Outlet]
        UIKit.UIButton btnBack { get; set; }


        [Outlet]
        UIKit.UIButton btnDone { get; set; }


        [Outlet]
        UIKit.UIButton btnFrom { get; set; }


        [Outlet]
        UIKit.UIButton btnTrash { get; set; }


        [Outlet]
        UIKit.UIButton btnUntil { get; set; }


        [Outlet]
        UIKit.UIImageView imgProfile { get; set; }


        [Outlet]
        UIKit.UILabel lblSchoolName { get; set; }


        [Outlet]
        UIKit.UILabel lblSportName { get; set; }


        [Outlet]
        UIKit.UITableView tvFrom { get; set; }


        [Outlet]
        UIKit.UITableView tvUntil { get; set; }


        [Outlet]
        UIKit.UIView vwDivider { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnBack != null) {
                btnBack.Dispose ();
                btnBack = null;
            }

            if (btnDone != null) {
                btnDone.Dispose ();
                btnDone = null;
            }

            if (btnFrom != null) {
                btnFrom.Dispose ();
                btnFrom = null;
            }

            if (btnTrash != null) {
                btnTrash.Dispose ();
                btnTrash = null;
            }

            if (btnUntil != null) {
                btnUntil.Dispose ();
                btnUntil = null;
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

            if (tvFrom != null) {
                tvFrom.Dispose ();
                tvFrom = null;
            }

            if (tvUntil != null) {
                tvUntil.Dispose ();
                tvUntil = null;
            }

            if (vwDivider != null) {
                vwDivider.Dispose ();
                vwDivider = null;
            }
        }
    }
}