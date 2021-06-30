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
    [Register ("PostSharesViewController")]
    partial class PostSharesViewController
    {
        [Outlet]
        UIKit.UITableView tvShares { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (tvShares != null) {
                tvShares.Dispose ();
                tvShares = null;
            }
        }
    }
}