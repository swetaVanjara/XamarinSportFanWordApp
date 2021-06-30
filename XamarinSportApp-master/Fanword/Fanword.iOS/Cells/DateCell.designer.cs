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
    [Register ("DateCell")]
    partial class DateCell
    {
        [Outlet]
        UIKit.UILabel lblDate { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblDate != null) {
                lblDate.Dispose ();
                lblDate = null;
            }
        }
    }
}