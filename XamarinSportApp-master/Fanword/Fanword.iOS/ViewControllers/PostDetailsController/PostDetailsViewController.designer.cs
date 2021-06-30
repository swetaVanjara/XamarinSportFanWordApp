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
    [Register ("PostDetailsViewController")]
    partial class PostDetailsViewController
    {
        [Outlet]
        UIKit.UIButton btnBack { get; set; }


        [Outlet]
        UIKit.UIButton btnComments { get; set; }


        [Outlet]
        UIKit.UIButton btnLikes { get; set; }


        [Outlet]
        UIKit.UIButton btnShares { get; set; }


        [Outlet]
        UIKit.UIButton btnTags { get; set; }


        [Outlet]
        UIKit.UILabel lblCount { get; set; }


        [Outlet]
        UIKit.UILabel lblHeader { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint lcIndicatorLeading { get; set; }


        [Outlet]
        UIKit.UIScrollView svScroller { get; set; }


        [Outlet]
        UIKit.UIView vwIndicator { get; set; }


        [Outlet]
        UIKit.UIView vwTopIcons { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnBack != null) {
                btnBack.Dispose ();
                btnBack = null;
            }

            if (btnComments != null) {
                btnComments.Dispose ();
                btnComments = null;
            }

            if (btnLikes != null) {
                btnLikes.Dispose ();
                btnLikes = null;
            }

            if (btnShares != null) {
                btnShares.Dispose ();
                btnShares = null;
            }

            if (btnTags != null) {
                btnTags.Dispose ();
                btnTags = null;
            }

            if (lblCount != null) {
                lblCount.Dispose ();
                lblCount = null;
            }

            if (lblHeader != null) {
                lblHeader.Dispose ();
                lblHeader = null;
            }

            if (lcIndicatorLeading != null) {
                lcIndicatorLeading.Dispose ();
                lcIndicatorLeading = null;
            }

            if (svScroller != null) {
                svScroller.Dispose ();
                svScroller = null;
            }

            if (vwIndicator != null) {
                vwIndicator.Dispose ();
                vwIndicator = null;
            }

            if (vwTopIcons != null) {
                vwTopIcons.Dispose ();
                vwTopIcons = null;
            }
        }
    }
}