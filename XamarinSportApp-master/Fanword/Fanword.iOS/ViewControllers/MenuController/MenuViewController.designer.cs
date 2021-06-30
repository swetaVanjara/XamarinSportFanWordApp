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
    [Register ("MenuViewController")]
    partial class MenuViewController
    {
        [Outlet]
        UIKit.UIImageView imgProfile { get; set; }


        [Outlet]
        UIKit.UILabel lblAthlete { get; set; }


        [Outlet]
        UIKit.UILabel lblFollowers { get; set; }


        [Outlet]
        UIKit.UILabel lblFollowing { get; set; }


        [Outlet]
        UIKit.UILabel lblName { get; set; }


        [Outlet]
        UIKit.UILabel lblPosts { get; set; }


        [Outlet]
        UIKit.UITableView tvMenu { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgProfile != null) {
                imgProfile.Dispose ();
                imgProfile = null;
            }

            if (lblAthlete != null) {
                lblAthlete.Dispose ();
                lblAthlete = null;
            }

            if (lblFollowers != null) {
                lblFollowers.Dispose ();
                lblFollowers = null;
            }

            if (lblFollowing != null) {
                lblFollowing.Dispose ();
                lblFollowing = null;
            }

            if (lblName != null) {
                lblName.Dispose ();
                lblName = null;
            }

            if (lblPosts != null) {
                lblPosts.Dispose ();
                lblPosts = null;
            }

            if (tvMenu != null) {
                tvMenu.Dispose ();
                tvMenu = null;
            }
        }
    }
}