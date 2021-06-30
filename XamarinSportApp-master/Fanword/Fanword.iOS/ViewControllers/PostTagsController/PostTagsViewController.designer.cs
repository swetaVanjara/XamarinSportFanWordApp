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
    [Register ("PostTagsViewController")]
    partial class PostTagsViewController
    {
        [Outlet]
        UIKit.UITableView tvTags { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (tvTags != null) {
                tvTags.Dispose ();
                tvTags = null;
            }
        }
    }
}