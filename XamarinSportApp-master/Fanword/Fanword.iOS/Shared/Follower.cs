using System;
using System.Threading.Tasks;
using Fanword.Poco.Interfaces;
using Fanword.Poco.Models;
using Fanword.Shared;
using Mobile.Extensions.iOS.Extensions;
using UIKit;

namespace Fanword.iOS.Shared
{
    public class Follower
    {
        public static void ToggleFollow(UIButton btnFollow, IFollowing model, string id, FeedType type, Action<bool> succeeded = null)
        {
            var oldText = btnFollow.Title(UIKit.UIControlState.Normal);
            btnFollow.SetTitle("Loading", UIKit.UIControlState.Normal);
			if (model.IsFollowing)
			{
				Task apiTask = null;
				if (type == FeedType.Team)
					apiTask = new ServiceApi().UnfollowTeam(id);
				else if (type == FeedType.User)
					apiTask = new ServiceApi().UnfollowUser(id);
				else if (type == FeedType.Sport)
					apiTask = new ServiceApi().UnfollowSport(id);
				else if (type == FeedType.ContentSource)
					apiTask = new ServiceApi().UnfollowContentSource(id);
				else if (type == FeedType.School)
					apiTask = new ServiceApi().UnfollowSchool(id);

				apiTask.HandleError(null, true, () =>
				{
					btnFollow.SetTitle(oldText, UIKit.UIControlState.Normal);
				});
				apiTask.OnSucess(response =>
				{
					model.IsFollowing = false;
					Views.SetFollowed(btnFollow, model.IsFollowing);
                    succeeded?.Invoke(false);
				});
			}
			else
			{
				Task apiTask = null;
				if (type == FeedType.Team)
					apiTask = new ServiceApi().FollowTeam(id);
				else if (type == FeedType.User)
					apiTask = new ServiceApi().FollowUser(id);
				else if (type == FeedType.Sport)
					apiTask = new ServiceApi().FollowSport(id);
				else if (type == FeedType.ContentSource)
					apiTask = new ServiceApi().FollowContentSource(id);
				else if (type == FeedType.School)
					apiTask = new ServiceApi().FollowSchool(id);
                apiTask.HandleError(null, true, () =>
				{
					btnFollow.SetTitle(oldText, UIKit.UIControlState.Normal);
				});
				apiTask.OnSucess(response =>
				{
					model.IsFollowing = true;
					Views.SetFollowed(btnFollow, model.IsFollowing);
                    succeeded?.Invoke(true);
				});
			}
        }
    }
}
