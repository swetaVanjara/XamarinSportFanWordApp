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
    [Register ("SearchCell")]
    partial class SearchCell
    {
        [Outlet]
        UIKit.UIButton btnFollow { get; set; }


        [Outlet]
        UIKit.UIImageView imgProfile { get; set; }


        [Outlet]
        UIKit.UILabel lblFollowers { get; set; }


        [Outlet]
        UIKit.UILabel lblSubtitle { get; set; }


        [Outlet]
        UIKit.UILabel lblTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnFollow != null) {
                btnFollow.Dispose ();
                btnFollow = null;
            }

            if (imgProfile != null) {
                imgProfile.Dispose ();
                imgProfile = null;
            }

            if (lblFollowers != null) {
                lblFollowers.Dispose ();
                lblFollowers = null;
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