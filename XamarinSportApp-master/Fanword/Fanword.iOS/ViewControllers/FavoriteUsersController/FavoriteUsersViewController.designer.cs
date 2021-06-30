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
    [Register ("FavoriteUsersViewController")]
    partial class FavoriteUsersViewController
    {
        [Outlet]
        UIKit.UIButton btnFollowers { get; set; }


        [Outlet]
        UIKit.UIButton btnFollowing { get; set; }


        [Outlet]
        UIKit.UITableView tvFavorites { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnFollowers != null) {
                btnFollowers.Dispose ();
                btnFollowers = null;
            }

            if (btnFollowing != null) {
                btnFollowing.Dispose ();
                btnFollowing = null;
            }

            if (tvFavorites != null) {
                tvFavorites.Dispose ();
                tvFavorites = null;
            }
        }
    }
}