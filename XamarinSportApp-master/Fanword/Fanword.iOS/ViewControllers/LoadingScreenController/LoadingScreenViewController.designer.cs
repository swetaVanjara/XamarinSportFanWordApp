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
    [Register ("LoadingScreenViewController")]
    partial class LoadingScreenViewController
    {
        [Outlet]
        UIKit.UIImageView imgImage { get; set; }


        [Outlet]
        UIKit.UILabel lblTitle { get; set; }


        [Outlet]
        UIKit.UIView vwContainer { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgImage != null) {
                imgImage.Dispose ();
                imgImage = null;
            }

            if (lblTitle != null) {
                lblTitle.Dispose ();
                lblTitle = null;
            }

            if (vwContainer != null) {
                vwContainer.Dispose ();
                vwContainer = null;
            }
        }
    }
}