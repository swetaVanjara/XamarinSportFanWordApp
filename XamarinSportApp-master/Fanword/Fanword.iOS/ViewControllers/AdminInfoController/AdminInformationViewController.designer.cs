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
    [Register ("AdminInformationViewController")]
    partial class AdminInformationViewController
    {
        [Outlet]
        UIKit.UIButton btnBack { get; set; }


        [Outlet]
        UIKit.UIButton btnContact { get; set; }


        [Outlet]
        UIKit.UIButton btnNext { get; set; }


        [Outlet]
        UIKit.UIButton btnQuestion { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnBack != null) {
                btnBack.Dispose ();
                btnBack = null;
            }

            if (btnContact != null) {
                btnContact.Dispose ();
                btnContact = null;
            }

            if (btnNext != null) {
                btnNext.Dispose ();
                btnNext = null;
            }

            if (btnQuestion != null) {
                btnQuestion.Dispose ();
                btnQuestion = null;
            }
        }
    }
}