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
    [Register ("TagProfileViewController")]
    partial class TagProfileViewController
    {
        [Outlet]
        UIKit.UIButton btnBack { get; set; }


        [Outlet]
        UIKit.UIButton btnNext { get; set; }


        [Outlet]
        UIKit.UILabel lblProfiles { get; set; }


        [Outlet]
        UIKit.UITableView tvProfiles { get; set; }


        [Outlet]
        UIKit.UITextField txtSearch { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnBack != null) {
                btnBack.Dispose ();
                btnBack = null;
            }

            if (btnNext != null) {
                btnNext.Dispose ();
                btnNext = null;
            }

            if (lblProfiles != null) {
                lblProfiles.Dispose ();
                lblProfiles = null;
            }

            if (tvProfiles != null) {
                tvProfiles.Dispose ();
                tvProfiles = null;
            }

            if (txtSearch != null) {
                txtSearch.Dispose ();
                txtSearch = null;
            }
        }
    }
}