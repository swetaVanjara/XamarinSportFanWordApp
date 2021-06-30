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
    [Register ("SportRankingCell")]
    partial class SportRankingCell
    {
        [Outlet]
        UIKit.UIButton btnFollow { get; set; }


        [Outlet]
        UIKit.UIImageView imgProfile { get; set; }


        [Outlet]
        UIKit.UILabel lblName { get; set; }


        [Outlet]
        UIKit.UILabel lblRank { get; set; }


        [Outlet]
        UIKit.UILabel lblRecord { get; set; }

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

            if (lblName != null) {
                lblName.Dispose ();
                lblName = null;
            }

            if (lblRank != null) {
                lblRank.Dispose ();
                lblRank = null;
            }

            if (lblRecord != null) {
                lblRecord.Dispose ();
                lblRecord = null;
            }
        }
    }
}