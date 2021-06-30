using System;
using UIKit;

namespace Fanword.iOS
{
	public interface IFeedCell
	{
		UIButton BtnLike { get; }
		UIImageView ImgProfile { get; }
		UILabel LblName { get; }
		UIButton BtnOptions { get; }
		UILabel LblLikes { get; }
		UIButton BtnComment { get; }
		UIButton BtnShare { get; }
        UIButton BtnTag { get; }
		UILabel LblContent { get; }
        UITextView TxtContent { get; }
		UILabel LblTimeAgo { get; }
		UILabel LblComments { get; }
		UILabel LblShares { get; }
		UILabel LblLinkHost { get; }
		UILabel LblLinkTitle { get; }
		UIView VwLinkDetails { get; }
		UIImageView ImgImage { get; }
		UIView VwMedia { get; }
        UIView VwContent { get; }
		UIButton BtnPlay { get; }
		UIButton BtnFacebook { get; }
		UIButton BtnTwitter { get; }
		UIButton BtnInstagram { get; }
		ImageLoaderHelper ImageTask { get; set; }
        ImageLoaderHelper ProfileTask { get; set; }
		UILabel LblTags { get; }
		bool IsCommonNew { get; set; }
		int Position { get; set; }
		string PostId { get; set; }
        NSLayoutConstraint ImgAspectRatio { get; }
        UIView VwShare { get; }
        UILabel LblNameShares { get; }
	}
}
