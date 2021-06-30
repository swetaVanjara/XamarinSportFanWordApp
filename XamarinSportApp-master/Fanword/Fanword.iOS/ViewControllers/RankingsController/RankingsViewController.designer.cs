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
    [Register ("RankingsViewController")]
    partial class RankingsViewController
    {
        [Outlet]
        UIKit.UIButton btnFilter { get; set; }


        [Outlet]
        UIKit.UITableView tvRankings { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnFilter != null) {
                btnFilter.Dispose ();
                btnFilter = null;
            }

            if (tvRankings != null) {
                tvRankings.Dispose ();
                tvRankings = null;
            }
        }
    }
}