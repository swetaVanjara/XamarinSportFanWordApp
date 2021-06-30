using System;
using UIKit;
using Mobile.Extensions.iOS.Extensions;
using Plugin.Settings;
using Mobile.Extensions.Extensions;
using Fanword.Poco.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace Fanword.iOS.Shared
{
    public class Navigator
    {
        public static void GoToSportProfile(UINavigationController navigationController, string sportId, bool toRankings)
        {
			var controller = navigationController.Storyboard.InstantiateViewController<SportProfileViewController>();
			controller.SportId = sportId;
            controller.GoToRankings = toRankings;
			navigationController.PushViewController(controller, true);
        }

		public static void GoToContentSourceProfile(UINavigationController navigationController, string contentSourceId)
		{
			var controller = navigationController.Storyboard.InstantiateViewController<ContentSourceProfileViewController>();
			controller.ContentSourceId = contentSourceId;
			navigationController.PushViewController(controller, true);
		}

		public static void GoToTeamProfile(UINavigationController navigationController, string teamId, bool toRankings)
		{
			var controller = navigationController.Storyboard.InstantiateViewController<TeamProfileViewController>();
			controller.TeamId = teamId;
			controller.GoToRankings = toRankings;
			navigationController.PushViewController(controller, true);
		}

		public static void GoToSchoolProfile(UINavigationController navigationController, string schoolId, bool toRankings)
		{
			var controller = navigationController.Storyboard.InstantiateViewController<SchoolProfileViewController>();
			controller.SchoolId = schoolId;
			controller.GoToRankings = toRankings;
			navigationController.PushViewController(controller, true);
		}

		public static void GoToEventProfile(UINavigationController navigationController, string eventId)
		{
			var controller = navigationController.Storyboard.InstantiateViewController<EventProfileViewController>();
			controller.EventId = eventId;
			navigationController.PushViewController(controller, true);
		}

		public static void GoToUserProfile(UINavigationController navigationController, string userId)
		{
            var user = CrossSettings.Current.GetValueOrDefaultJson<User>("User");
            if(userId == user.Id)
            {
				var controller = navigationController.Storyboard.InstantiateViewController<MyProfileViewController>();
				navigationController.PushViewController(controller, true);
            }
            else
            {
				var controller = navigationController.Storyboard.InstantiateViewController<UserProfileViewController>();
				controller.UserId = userId;
				navigationController.PushViewController(controller, true);
            }

		}

		public static void HandleNotificationTap(UINavigationController navigationController, Dictionary<string, string> metaData, string title, string message)
        {
			if (metaData[MetaDataKeys.UserNotificationType] == UserNotificationType.Like.ToString())
			{
				var controller = navigationController.Storyboard.InstantiateViewController<ViewPostViewController>();
				controller.PostId = metaData[MetaDataKeys.PostId];
				navigationController.PushViewController(controller, true);
			}
			else if (metaData[MetaDataKeys.UserNotificationType] == UserNotificationType.Comment.ToString())
			{
				var controller = navigationController.Storyboard.InstantiateViewController<PostDetailsViewController>();
                controller.PostId = metaData[MetaDataKeys.PostId];
                controller.Controller = "Comments";
				navigationController.PushViewController(controller, true);
			}
			else if (metaData[MetaDataKeys.UserNotificationType] == UserNotificationType.Follow.ToString())
			{
				GoToUserProfile(navigationController, metaData[MetaDataKeys.FromId]);
			}
            else
            {
				try
                {
                    var controller = navigationController.Storyboard.InstantiateViewController<ViewNotificationViewController>();
                    controller.NotificationTitle = title;
                    var content = metaData[MetaDataKeys.NewsNotificationMessage];
                    controller.NotificationMessage = content;
                    navigationController.PushViewController(controller, true);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
				}
            }
        }
    }
}
