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
    [Register ("ShareOptionsViewController")]
    partial class ShareOptionsViewController
    {
        [Outlet]
        UIKit.UIButton btnFacebook { get; set; }


        [Outlet]
        UIKit.UIButton btnFanword { get; set; }


        [Outlet]
        UIKit.UIButton btnOther { get; set; }


        [Outlet]
        UIKit.UIView vwBackground { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnFacebook != null) {
                btnFacebook.Dispose ();
                btnFacebook = null;
            }

            if (btnFanword != null) {
                btnFanword.Dispose ();
                btnFanword = null;
            }

            if (btnOther != null) {
                btnOther.Dispose ();
                btnOther = null;
            }

            if (vwBackground != null) {
                vwBackground.Dispose ();
                vwBackground = null;
            }
        }
    }
}