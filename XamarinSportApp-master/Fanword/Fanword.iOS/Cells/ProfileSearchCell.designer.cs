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
    [Register ("ProfileSearchCell")]
    partial class ProfileSearchCell
    {
        [Outlet]
        UIKit.UIImageView imgProfile { get; set; }


        [Outlet]
        UIKit.UIImageView imgTagged { get; set; }


        [Outlet]
        UIKit.UILabel lblSubTitle { get; set; }


        [Outlet]
        UIKit.UILabel lblTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgProfile != null) {
                imgProfile.Dispose ();
                imgProfile = null;
            }

            if (imgTagged != null) {
                imgTagged.Dispose ();
                imgTagged = null;
            }

            if (lblSubTitle != null) {
                lblSubTitle.Dispose ();
                lblSubTitle = null;
            }

            if (lblTitle != null) {
                lblTitle.Dispose ();
                lblTitle = null;
            }
        }
    }
}