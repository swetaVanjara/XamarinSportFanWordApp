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
    [Register ("MainViewController")]
    partial class MainViewController
    {
        [Outlet]
        UIKit.UIView vwContent { get; set; }


        [Outlet]
        UIKit.UIView vwDarkenView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (vwContent != null) {
                vwContent.Dispose ();
                vwContent = null;
            }

            if (vwDarkenView != null) {
                vwDarkenView.Dispose ();
                vwDarkenView = null;
            }
        }
    }
}