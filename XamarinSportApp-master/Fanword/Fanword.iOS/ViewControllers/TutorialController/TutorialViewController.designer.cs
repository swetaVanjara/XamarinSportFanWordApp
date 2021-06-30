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
    [Register ("TutorialViewController")]
    partial class TutorialViewController
    {
        [Outlet]
        UIKit.UIButton btnOk { get; set; }


        [Outlet]
        UIKit.UIImageView imgImage { get; set; }


        [Outlet]
        UIKit.UILabel lblSubtitle { get; set; }


        [Outlet]
        UIKit.UILabel lblTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnOk != null) {
                btnOk.Dispose ();
                btnOk = null;
            }

            if (imgImage != null) {
                imgImage.Dispose ();
                imgImage = null;
            }

            if (lblSubtitle != null) {
                lblSubtitle.Dispose ();
                lblSubtitle = null;
            }

            if (lblTitle != null) {
                lblTitle.Dispose ();
                lblTitle = null;
            }
        }
    }
}