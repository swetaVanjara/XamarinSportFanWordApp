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
    [Register ("ScoresViewController")]
    partial class ScoresViewController
    {
        [Outlet]
        UIKit.UIActivityIndicatorView aiActivity { get; set; }


        [Outlet]
        UIKit.UIButton btnFilter { get; set; }


        [Outlet]
        UIKit.UIButton btnPast { get; set; }


        [Outlet]
        UIKit.UIButton btnToday { get; set; }


        [Outlet]
        UIKit.UIButton btnUpcoming { get; set; }


        [Outlet]
        UIKit.UITableView tvScores { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (aiActivity != null) {
                aiActivity.Dispose ();
                aiActivity = null;
            }

            if (btnFilter != null) {
                btnFilter.Dispose ();
                btnFilter = null;
            }

            if (btnPast != null) {
                btnPast.Dispose ();
                btnPast = null;
            }

            if (btnToday != null) {
                btnToday.Dispose ();
                btnToday = null;
            }

            if (btnUpcoming != null) {
                btnUpcoming.Dispose ();
                btnUpcoming = null;
            }

            if (tvScores != null) {
                tvScores.Dispose ();
                tvScores = null;
            }
        }
    }
}