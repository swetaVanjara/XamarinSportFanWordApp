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
    [Register ("TablePopoverViewController")]
    partial class TablePopoverViewController
    {
        [Outlet]
        UIKit.UITableView tvList { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (tvList != null) {
                tvList.Dispose ();
                tvList = null;
            }
        }
    }
}