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
    [Register ("FavoriteViewController")]
    partial class FavoriteViewController
    {
        [Outlet]
        UIKit.UITableView tvFavorites { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (tvFavorites != null) {
                tvFavorites.Dispose ();
                tvFavorites = null;
            }
        }
    }
}