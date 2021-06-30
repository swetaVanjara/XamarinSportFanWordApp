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
    [Register ("NotificationCell")]
    partial class NotificationCell
    {
        [Outlet]
        UIKit.UIImageView imgProfile { get; set; }


        [Outlet]
        UIKit.UILabel lblTimeAgo { get; set; }


        [Outlet]
        UIKit.UILabel lblTitle { get; set; }


        [Outlet]
        UIKit.UIView vwIsNew { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgProfile != null) {
                imgProfile.Dispose ();
                imgProfile = null;
            }

            if (lblTimeAgo != null) {
                lblTimeAgo.Dispose ();
                lblTimeAgo = null;
            }

            if (lblTitle != null) {
                lblTitle.Dispose ();
                lblTitle = null;
            }

            if (vwIsNew != null) {
                vwIsNew.Dispose ();
                vwIsNew = null;
            }
        }
    }
}