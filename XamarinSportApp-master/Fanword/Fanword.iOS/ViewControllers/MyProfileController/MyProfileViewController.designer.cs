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
    [Register ("MyProfileViewController")]
    partial class MyProfileViewController
    {
        [Outlet]
        UIKit.UIButton btnBack { get; set; }


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
        UIKit.UILabel lblTitle { get; set; }


        [Outlet]
        UIKit.UIView vwHeader { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnBack != null) {
                btnBack.Dispose ();
                btnBack = null;
            }

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

            if (lblTitle != null) {
                lblTitle.Dispose ();
                lblTitle = null;
            }

            if (vwHeader != null) {
                vwHeader.Dispose ();
                vwHeader = null;
            }
        }
    }
}