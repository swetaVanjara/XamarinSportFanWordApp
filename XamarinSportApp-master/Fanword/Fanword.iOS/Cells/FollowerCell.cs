// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;
using Fanword.Poco.Models;
using FFImageLoading.Transformations;

namespace Fanword.iOS
{
	public partial class FollowerCell : UITableViewCell
	{
		ImageLoaderHelper imageTask;
        bool isNew = true;
        Follower model;
		public FollowerCell (IntPtr handle) : base (handle)
		{
		}

		public void SetData(Follower item)
		{
            model = item;
            if(isNew)
            {
				btnFollow.TouchUpInside += (sender, e) =>
				{
                    Shared.Follower.ToggleFollow(btnFollow, model, model.Id, FeedType.User);
				};
            }

			lblName.Text = item.FirstName + " " + item.LastName;
			imageTask?.Cancel(item.ProfileUrl);
			if (!string.IsNullOrEmpty(item.ProfileUrl))
			{
				imageTask = new ImageLoaderHelper(item.ProfileUrl, imgProfile, "DefaultProfile", null, new CircleTransformation());
			}
		}
	}
}
