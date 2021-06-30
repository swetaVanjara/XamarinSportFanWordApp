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
    [Register ("PostOptionsViewController")]
    partial class PostOptionsViewController
    {
        [Outlet]
        UIKit.UIButton btnDeletePost { get; set; }


        [Outlet]
        UIKit.UIButton btnEditPost { get; set; }


        [Outlet]
        UIKit.UIView vwBackground { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnDeletePost != null) {
                btnDeletePost.Dispose ();
                btnDeletePost = null;
            }

            if (btnEditPost != null) {
                btnEditPost.Dispose ();
                btnEditPost = null;
            }

            if (vwBackground != null) {
                vwBackground.Dispose ();
                vwBackground = null;
            }
        }
    }
}