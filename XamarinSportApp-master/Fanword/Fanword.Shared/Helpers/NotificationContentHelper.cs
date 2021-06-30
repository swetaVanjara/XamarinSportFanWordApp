using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Poco.Models;
using Notifications.Mobile.Models;

namespace Fanword.Shared.Helpers
{
    public class NotificationContentHelper
    {
        public static string GetContentFromNotification(Notification notification)
        {
            var notificationType = notification.MetaData[MetaDataKeys.UserNotificationType];
            if (notificationType == UserNotificationType.NewsNotification.ToString())
            {
                return notification.Content;
            }
            else if(notificationType == UserNotificationType.Like.ToString())
            {
                return notification.MetaData[MetaDataKeys.UserFullName] + " " + notification.Content;
            }
            else if (notificationType == UserNotificationType.Follow.ToString())
            {
                return notification.MetaData[MetaDataKeys.UserFullName] + " " + notification.Content;
            }
            else if (notificationType == UserNotificationType.Comment.ToString())
            {
                return notification.MetaData[MetaDataKeys.UserFullName] + " " + notification.Content;
            }

            return notification.Content;
        }
    }
}
