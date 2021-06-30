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
    [Register ("TeamRankingCell")]
    partial class TeamRankingCell
    {
        [Outlet]
        UIKit.UIImageView imgChange { get; set; }


        [Outlet]
        UIKit.UILabel lblChange { get; set; }


        [Outlet]
        UIKit.UILabel lblDate { get; set; }


        [Outlet]
        UIKit.UILabel lblRank { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgChange != null) {
                imgChange.Dispose ();
                imgChange = null;
            }

            if (lblChange != null) {
                lblChange.Dispose ();
                lblChange = null;
            }

            if (lblDate != null) {
                lblDate.Dispose ();
                lblDate = null;
            }

            if (lblRank != null) {
                lblRank.Dispose ();
                lblRank = null;
            }
        }
    }
}