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
    [Register ("PostAsViewController")]
    partial class PostAsViewController
    {
        [Outlet]
        UIKit.UIImageView imgContentSource { get; set; }


        [Outlet]
        UIKit.UIImageView imgProfile { get; set; }


        [Outlet]
        UIKit.UILabel lblContentSource { get; set; }


        [Outlet]
        UIKit.UILabel lblName { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint lcTableHeight { get; set; }


        [Outlet]
        UIKit.UITableView tvAdmins { get; set; }


        [Outlet]
        UIKit.UIView vwBackground { get; set; }


        [Outlet]
        UIKit.UIView vwContentSource { get; set; }


        [Outlet]
        UIKit.UIView vwUser { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgContentSource != null) {
                imgContentSource.Dispose ();
                imgContentSource = null;
            }

            if (imgProfile != null) {
                imgProfile.Dispose ();
                imgProfile = null;
            }

            if (lblContentSource != null) {
                lblContentSource.Dispose ();
                lblContentSource = null;
            }

            if (lblName != null) {
                lblName.Dispose ();
                lblName = null;
            }

            if (lcTableHeight != null) {
                lcTableHeight.Dispose ();
                lcTableHeight = null;
            }

            if (tvAdmins != null) {
                tvAdmins.Dispose ();
                tvAdmins = null;
            }

            if (vwBackground != null) {
                vwBackground.Dispose ();
                vwBackground = null;
            }

            if (vwContentSource != null) {
                vwContentSource.Dispose ();
                vwContentSource = null;
            }

            if (vwUser != null) {
                vwUser.Dispose ();
                vwUser = null;
            }
        }
    }
}