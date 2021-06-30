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
    [Register ("SettingsViewController")]
    partial class SettingsViewController
    {
        [Outlet]
        UIKit.UIButton btnBack { get; set; }


        [Outlet]
        UIKit.UIView vwBasicProfile { get; set; }


        [Outlet]
        UIKit.UIView vwStudenAthlete { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnBack != null) {
                btnBack.Dispose ();
                btnBack = null;
            }

            if (vwBasicProfile != null) {
                vwBasicProfile.Dispose ();
                vwBasicProfile = null;
            }

            if (vwStudenAthlete != null) {
                vwStudenAthlete.Dispose ();
                vwStudenAthlete = null;
            }
        }
    }
}