// This file has been autogenerated from a class added in the UI designer.

using System;
using Fanword.Poco.Models;
using Foundation;
using UIKit;
using Fanword.Shared.Helpers;
using Fanword.iOS.Shared;
using FFImageLoading.Transformations;

namespace Fanword.iOS
{
	public partial class SearchCell : UITableViewCell
	{
        bool IsNew = true;
        GlobalSearchItem model;
		ImageLoaderHelper imageTask;
        UINavigationController navigationController;
		public SearchCell (IntPtr handle) : base (handle)
		{
		}

		public void SetData(GlobalSearchItem item, UINavigationController navigationController)
		{
            this.navigationController = navigationController;
            model = item;
            if(IsNew)
            {
                btnFollow.TouchUpInside += (sender, e) => 
                {
                    Shared.Follower.ToggleFollow(btnFollow, model, model.Id, model.Type);
                };
                imgProfile.UserInteractionEnabled = true;
                imgProfile.AddGestureRecognizer(new UITapGestureRecognizer((obj) => GoToProfile()));
				lblTitle.UserInteractionEnabled = true;
				lblTitle.AddGestureRecognizer(new UITapGestureRecognizer((obj) => GoToProfile()));
                IsNew = false;
            }
			lblTitle.Text = item.Title;
            lblSubtitle.Text = item.Subtitle;
            lblSubtitle.Hidden = string.IsNullOrEmpty(item.Subtitle);
            lblFollowers.Text = LargeValueHelper.GetString(item.Followers) + " followers";
            Views.SetFollowed(btnFollow, item.IsFollowing);

			imageTask?.Cancel(item.ProfileUrl);
			if (!string.IsNullOrEmpty(item.ProfileUrl))
			{
				imageTask = new ImageLoaderHelper(item.ProfileUrl, imgProfile, "DefaultProfile", null, new CircleTransformation());
			}
		}

        void GoToProfile()
        {
			if (model.Type == FeedType.Team)
			{
				Navigator.GoToTeamProfile(navigationController, model.Id, false);
			}
			if (model.Type == FeedType.School)
			{
				Navigator.GoToSchoolProfile(navigationController, model.Id, false);
			}
			if (model.Type == FeedType.Sport)
			{
				Navigator.GoToSportProfile(navigationController, model.Id, false);
			}
			if (model.Type == FeedType.ContentSource)
			{
				Navigator.GoToContentSourceProfile(navigationController, model.Id);
			}
			if (model.Type == FeedType.User)
			{
				Navigator.GoToUserProfile(navigationController, model.Id);
			}
        }
	}
}