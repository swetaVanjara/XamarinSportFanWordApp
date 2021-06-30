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
    [Register ("CommentCell")]
    partial class CommentCell
    {
        [Outlet]
        UIKit.UIButton btnLike { get; set; }


        [Outlet]
        UIKit.UIButton btnReply { get; set; }


        [Outlet]
        UIKit.UIImageView imgProfile { get; set; }


        [Outlet]
        UIKit.UILabel lblContent { get; set; }


        [Outlet]
        UIKit.UILabel lblLikes { get; set; }


        [Outlet]
        UIKit.UILabel lblName { get; set; }


        [Outlet]
        UIKit.UILabel lblReplies { get; set; }


        [Outlet]
        UIKit.UILabel lblResponseTo { get; set; }


        [Outlet]
        UIKit.UILabel lblTimeAgo { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnLike != null) {
                btnLike.Dispose ();
                btnLike = null;
            }

            if (btnReply != null) {
                btnReply.Dispose ();
                btnReply = null;
            }

            if (imgProfile != null) {
                imgProfile.Dispose ();
                imgProfile = null;
            }

            if (lblContent != null) {
                lblContent.Dispose ();
                lblContent = null;
            }

            if (lblLikes != null) {
                lblLikes.Dispose ();
                lblLikes = null;
            }

            if (lblName != null) {
                lblName.Dispose ();
                lblName = null;
            }

            if (lblReplies != null) {
                lblReplies.Dispose ();
                lblReplies = null;
            }

            if (lblResponseTo != null) {
                lblResponseTo.Dispose ();
                lblResponseTo = null;
            }

            if (lblTimeAgo != null) {
                lblTimeAgo.Dispose ();
                lblTimeAgo = null;
            }
        }
    }
}