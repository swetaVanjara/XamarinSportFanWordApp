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
    [Register ("TeamProfileRankingsViewController")]
    partial class TeamProfileRankingsViewController
    {
        [Outlet]
        UIKit.UIButton btnRankings { get; set; }


        [Outlet]
        UIKit.UILabel lblRecord { get; set; }


        [Outlet]
        UIKit.UITableView tvRankings { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnRankings != null) {
                btnRankings.Dispose ();
                btnRankings = null;
            }

            if (lblRecord != null) {
                lblRecord.Dispose ();
                lblRecord = null;
            }

            if (tvRankings != null) {
                tvRankings.Dispose ();
                tvRankings = null;
            }
        }
    }
}