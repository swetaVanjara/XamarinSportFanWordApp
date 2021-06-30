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
    [Register ("EventProfileCell")]
    partial class EventProfileCell
    {
        [Outlet]
        UIKit.UIImageView imgTagged { get; set; }


        [Outlet]
        UIKit.UIImageView imgTeam1 { get; set; }


        [Outlet]
        UIKit.UIImageView imgTeam2 { get; set; }


        [Outlet]
        UIKit.UILabel lblDate { get; set; }


        [Outlet]
        UIKit.UILabel lblEventName { get; set; }


        [Outlet]
        UIKit.UILabel lblSportName { get; set; }


        [Outlet]
        UIKit.UILabel lblTeam1 { get; set; }


        [Outlet]
        UIKit.UILabel lblTeam2 { get; set; }


        [Outlet]
        UIKit.UIView vwEventName { get; set; }


        [Outlet]
        UIKit.UIView vwTeam1 { get; set; }


        [Outlet]
        UIKit.UIView vwTeam2 { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgTagged != null) {
                imgTagged.Dispose ();
                imgTagged = null;
            }

            if (imgTeam1 != null) {
                imgTeam1.Dispose ();
                imgTeam1 = null;
            }

            if (imgTeam2 != null) {
                imgTeam2.Dispose ();
                imgTeam2 = null;
            }

            if (lblDate != null) {
                lblDate.Dispose ();
                lblDate = null;
            }

            if (lblEventName != null) {
                lblEventName.Dispose ();
                lblEventName = null;
            }

            if (lblSportName != null) {
                lblSportName.Dispose ();
                lblSportName = null;
            }

            if (lblTeam1 != null) {
                lblTeam1.Dispose ();
                lblTeam1 = null;
            }

            if (lblTeam2 != null) {
                lblTeam2.Dispose ();
                lblTeam2 = null;
            }

            if (vwEventName != null) {
                vwEventName.Dispose ();
                vwEventName = null;
            }

            if (vwTeam1 != null) {
                vwTeam1.Dispose ();
                vwTeam1 = null;
            }

            if (vwTeam2 != null) {
                vwTeam2.Dispose ();
                vwTeam2 = null;
            }
        }
    }
}