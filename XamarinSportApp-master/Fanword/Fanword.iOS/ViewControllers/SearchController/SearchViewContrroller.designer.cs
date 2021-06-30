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
    [Register ("SearchViewContrroller")]
    partial class SearchViewContrroller
    {
        [Outlet]
        UIKit.UIButton btnBack { get; set; }


        [Outlet]
        UIKit.UIImageView imgSearch { get; set; }


        [Outlet]
        UIKit.UITableView tvSearch { get; set; }


        [Outlet]
        UIKit.UITextField txtSearch { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnBack != null) {
                btnBack.Dispose ();
                btnBack = null;
            }

            if (imgSearch != null) {
                imgSearch.Dispose ();
                imgSearch = null;
            }

            if (tvSearch != null) {
                tvSearch.Dispose ();
                tvSearch = null;
            }

            if (txtSearch != null) {
                txtSearch.Dispose ();
                txtSearch = null;
            }
        }
    }
}