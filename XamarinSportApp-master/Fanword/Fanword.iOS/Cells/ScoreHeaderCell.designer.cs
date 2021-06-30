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
    [Register ("ScoreHeaderCell")]
    partial class ScoreHeaderCell
    {
        [Outlet]
        UIKit.UILabel lblCount { get; set; }


        [Outlet]
        UIKit.UILabel lblDate { get; set; }


        [Outlet]
        UIKit.UIView vwSpacer { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblCount != null) {
                lblCount.Dispose ();
                lblCount = null;
            }

            if (lblDate != null) {
                lblDate.Dispose ();
                lblDate = null;
            }

            if (vwSpacer != null) {
                vwSpacer.Dispose ();
                vwSpacer = null;
            }
        }
    }
}