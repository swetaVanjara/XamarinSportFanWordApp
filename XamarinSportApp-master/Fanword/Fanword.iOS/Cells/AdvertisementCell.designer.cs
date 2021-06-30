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
    [Register ("AdvertisementCell")]
    partial class AdvertisementCell
    {
        [Outlet]
        UIKit.UIImageView imgImage { get; set; }


        [Outlet]
        UIKit.UIImageView imgProfile { get; set; }


        [Outlet]
        UIKit.UILabel lblContent { get; set; }


        [Outlet]
        UIKit.UILabel lblName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgImage != null) {
                imgImage.Dispose ();
                imgImage = null;
            }

            if (imgProfile != null) {
                imgProfile.Dispose ();
                imgProfile = null;
            }

            if (lblContent != null) {
                lblContent.Dispose ();
                lblContent = null;
            }

            if (lblName != null) {
                lblName.Dispose ();
                lblName = null;
            }
        }
    }
}