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
    [Register ("FeedViewController")]
    partial class FeedViewController
    {
        [Outlet]
        UIKit.UIButton btnNewPost { get; set; }


        [Outlet]
        Fanword.iOS.FeedTableView tvFeed { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnNewPost != null) {
                btnNewPost.Dispose ();
                btnNewPost = null;
            }

            if (tvFeed != null) {
                tvFeed.Dispose ();
                tvFeed = null;
            }
        }
    }
}