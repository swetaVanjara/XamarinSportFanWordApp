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
    [Register ("SportRankingsViewController")]
    partial class SportRankingsViewController
    {
        [Outlet]
        UIKit.UILabel lblDate { get; set; }


        [Outlet]
        UIKit.UITableView tvRankings { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblDate != null) {
                lblDate.Dispose ();
                lblDate = null;
            }

            if (tvRankings != null) {
                tvRankings.Dispose ();
                tvRankings = null;
            }
        }
    }
}