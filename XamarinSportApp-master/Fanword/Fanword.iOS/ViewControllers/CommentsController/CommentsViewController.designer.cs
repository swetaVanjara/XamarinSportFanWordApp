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
    [Register ("CommentsViewController")]
    partial class CommentsViewController
    {
        [Outlet]
        UIKit.UIButton btnSend { get; set; }


        [Outlet]
        UIKit.UITableView tvComments { get; set; }


        [Outlet]
        UIKit.UITextField txtComment { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnSend != null) {
                btnSend.Dispose ();
                btnSend = null;
            }

            if (tvComments != null) {
                tvComments.Dispose ();
                tvComments = null;
            }

            if (txtComment != null) {
                txtComment.Dispose ();
                txtComment = null;
            }
        }
    }
}