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
    [Register ("SportAboutViewController")]
    partial class SportAboutViewController
    {
        [Outlet]
        UIKit.UIButton btnAthletes { get; set; }


        [Outlet]
        UIKit.UIButton btnTeams { get; set; }


        [Outlet]
        UIKit.UITableView tvTable { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnAthletes != null) {
                btnAthletes.Dispose ();
                btnAthletes = null;
            }

            if (btnTeams != null) {
                btnTeams.Dispose ();
                btnTeams = null;
            }

            if (tvTable != null) {
                tvTable.Dispose ();
                tvTable = null;
            }
        }
    }
}