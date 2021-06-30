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
    [Register ("ContentSourceAboutViewController")]
    partial class ContentSourceAboutViewController
    {
        [Outlet]
        UIKit.UIButton btnFacebook { get; set; }


        [Outlet]
        UIKit.UIButton btnInstagram { get; set; }


        [Outlet]
        UIKit.UIButton btnTwitter { get; set; }


        [Outlet]
        UIKit.UIButton btnWebsite { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnFacebook != null) {
                btnFacebook.Dispose ();
                btnFacebook = null;
            }

            if (btnInstagram != null) {
                btnInstagram.Dispose ();
                btnInstagram = null;
            }

            if (btnTwitter != null) {
                btnTwitter.Dispose ();
                btnTwitter = null;
            }

            if (btnWebsite != null) {
                btnWebsite.Dispose ();
                btnWebsite = null;
            }
        }
    }
}