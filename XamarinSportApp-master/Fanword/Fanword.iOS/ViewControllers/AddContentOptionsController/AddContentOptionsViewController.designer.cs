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
    [Register ("AddContentOptionsViewController")]
    partial class AddContentOptionsViewController
    {
        [Outlet]
        UIKit.UIButton btnPickPhoto { get; set; }


        [Outlet]
        UIKit.UIButton btnPickVideo { get; set; }


        [Outlet]
        UIKit.UIButton btnTakePhoto { get; set; }


        [Outlet]
        UIKit.UIButton btnTakeVideo { get; set; }


        [Outlet]
        UIKit.UIView vwBackground { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnPickPhoto != null) {
                btnPickPhoto.Dispose ();
                btnPickPhoto = null;
            }

            if (btnPickVideo != null) {
                btnPickVideo.Dispose ();
                btnPickVideo = null;
            }

            if (btnTakePhoto != null) {
                btnTakePhoto.Dispose ();
                btnTakePhoto = null;
            }

            if (btnTakeVideo != null) {
                btnTakeVideo.Dispose ();
                btnTakeVideo = null;
            }

            if (vwBackground != null) {
                vwBackground.Dispose ();
                vwBackground = null;
            }
        }
    }
}