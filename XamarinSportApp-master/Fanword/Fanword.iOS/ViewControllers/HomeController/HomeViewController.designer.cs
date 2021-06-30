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
    [Register ("HomeViewController")]
    partial class HomeViewController
    {
        [Outlet]
        UIKit.UIButton btnExplore { get; set; }


        [Outlet]
        UIKit.UIButton btnFeed { get; set; }


        [Outlet]
        UIKit.UIButton btnMenu { get; set; }


        [Outlet]
        UIKit.UIButton btnNotifications { get; set; }


        [Outlet]
        UIKit.UIButton btnRankings { get; set; }


        [Outlet]
        UIKit.UIButton btnScores { get; set; }


        [Outlet]
        UIKit.UIButton btnSearch { get; set; }


        [Outlet]
        UIKit.UICollectionView cvPager { get; set; }


        [Outlet]
        UIKit.UILabel lblNotificationCount { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint lcIndicatorLeading { get; set; }


        [Outlet]
        UIKit.UIView vwIndicator { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnExplore != null) {
                btnExplore.Dispose ();
                btnExplore = null;
            }

            if (btnFeed != null) {
                btnFeed.Dispose ();
                btnFeed = null;
            }

            if (btnMenu != null) {
                btnMenu.Dispose ();
                btnMenu = null;
            }

            if (btnNotifications != null) {
                btnNotifications.Dispose ();
                btnNotifications = null;
            }

            if (btnRankings != null) {
                btnRankings.Dispose ();
                btnRankings = null;
            }

            if (btnScores != null) {
                btnScores.Dispose ();
                btnScores = null;
            }

            if (btnSearch != null) {
                btnSearch.Dispose ();
                btnSearch = null;
            }

            if (cvPager != null) {
                cvPager.Dispose ();
                cvPager = null;
            }

            if (lblNotificationCount != null) {
                lblNotificationCount.Dispose ();
                lblNotificationCount = null;
            }

            if (lcIndicatorLeading != null) {
                lcIndicatorLeading.Dispose ();
                lcIndicatorLeading = null;
            }

            if (vwIndicator != null) {
                vwIndicator.Dispose ();
                vwIndicator = null;
            }
        }
    }
}