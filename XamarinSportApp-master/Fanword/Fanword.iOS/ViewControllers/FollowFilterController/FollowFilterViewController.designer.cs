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
    [Register ("FollowFilterViewController")]
    partial class FollowFilterViewController
    {
        [Outlet]
        UIKit.UIButton btnSchools { get; set; }


        [Outlet]
        UIKit.UIButton btnSearch { get; set; }


        [Outlet]
        UIKit.UIButton btnSports { get; set; }


        [Outlet]
        UIKit.UIButton btnTeams { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnSchools != null) {
                btnSchools.Dispose ();
                btnSchools = null;
            }

            if (btnSearch != null) {
                btnSearch.Dispose ();
                btnSearch = null;
            }

            if (btnSports != null) {
                btnSports.Dispose ();
                btnSports = null;
            }

            if (btnTeams != null) {
                btnTeams.Dispose ();
                btnTeams = null;
            }
        }
    }
}