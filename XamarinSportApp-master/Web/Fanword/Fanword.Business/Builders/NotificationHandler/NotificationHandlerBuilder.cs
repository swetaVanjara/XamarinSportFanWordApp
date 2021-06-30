using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionClasses;
using Fanword.Data.Context;
using Fanword.Data.Repository;
using Fanword.Poco.Models;
using Newtonsoft.Json;
using Notifications.Data.Entities;
using Notifications.Data.ViewModels;
using PushNotifications;
using PushNotifications.Azure;
using PushNotifications.Azure.Extensions;

namespace Fanword.Business.Builders.NotificationHandler
{
    public class NotificationHandlerBuilder
    {
        public async Task NotificationTriggered(NotificationViewModel model)
        {
            var notification = new {Id = model.Id, alert = model.Title, Title = model.Title, Messsage = model.Content, MetaData = JsonConvert.SerializeObject(model.MetaData)};
            await new PushNotification(notification)
                .SendToAndroidDevices()
                .SendToAppleDevices()
                .SendToTags(new List<string>() { model.UserId })
                .SendAsync(new AzurePushService());
        }

        public List<string> NotifySpecificUsers(Notification notification)
        { 
            var notifiationType = notification.NotificationMetaDatas.FirstOrDefault(m => m.DictionaryKey == MetaDataKeys.UserNotificationType);
            if (notifiationType == null)
                return new List<string>();

            var repo = new ApplicationRepository(new ApplicationDbContext());

            if (notifiationType.DictionaryValue == UserNotificationType.NewsNotification.ToString())
            {
                var fromIdType = notification.NotificationMetaDatas.FirstOrDefault(m => m.DictionaryKey == MetaDataKeys.FromIdType).DictionaryValue;
                var fromId = notification.NotificationMetaDatas.FirstOrDefault(m => m.DictionaryKey == MetaDataKeys.FromId).DictionaryValue;

                if (fromIdType == FeedType.ContentSource.ToString())
                {
                    return repo.ContentSources.Where(m => m.Id == fromId).SelectMany(m => m.Followers.Select(mm => mm.Id)).ToList();
                }
                if (fromIdType == FeedType.School.ToString())
                {
                    return repo.Schools.Where(m => m.Id == fromId).SelectMany(m => m.Followers.Select(mm => mm.Id)).ToList();
                }
                if (fromIdType == FeedType.Team.ToString())
                {
                    return repo.Teams.Where(m => m.Id == fromId).SelectMany(m => m.Followers.Select(mm => mm.Id)).ToList();
                }
                if (fromIdType == FeedType.Sport.ToString())
                {
                    return repo.Sports.Where(m => m.Id == fromId).SelectMany(m => m.Followers.Select(mm => mm.Id)).ToList();
                }
            }

            return new List<string>();
        }

        public Dictionary<string,string> AddAdditionalUserNotificationData(NotificationViewModel model)
        {
            var notifiationType = model.MetaData.GetValue(MetaDataKeys.UserNotificationType);
            var data = new Dictionary<string,string>();
            if (notifiationType == null)
                return data;

            var repo = new ApplicationRepository(new ApplicationDbContext());

            if (notifiationType == UserNotificationType.NewsNotification.ToString())
            {
                var fromIdType = model.MetaData.GetValue(MetaDataKeys.FromIdType);
                var fromId = model.MetaData.GetValue(MetaDataKeys.FromId);

                if (fromIdType == FeedType.ContentSource.ToString())
                {
                    var profielUrl = repo.ContentSources.Where(m => m.Id == fromId).Select(m => m.LogoUrl).FirstOrDefault();
                    data.Add("ProfileUrl", profielUrl);
                }
                if (fromIdType == FeedType.School.ToString())
                {
                    var profielUrl = repo.Schools.Where(m => m.Id == fromId).Select(m => m.ProfilePublicUrl).FirstOrDefault();
                    data.Add("ProfileUrl", profielUrl);
                }
                if (fromIdType == FeedType.Team.ToString())
                {
                    var profielUrl = repo.Teams.Where(m => m.Id == fromId).Select(m => m.ProfilePublicUrl).FirstOrDefault();
                    data.Add("ProfileUrl", profielUrl);
                }
                if (fromIdType == FeedType.Sport.ToString())
                {
                    var profielUrl = repo.Sports.Where(m => m.Id == fromId).Select(m => m.IconPublicUrl).FirstOrDefault();
                    data.Add("ProfileUrl", profielUrl);
                }
            }

            if (notifiationType == UserNotificationType.Like.ToString() || notifiationType == UserNotificationType.CommentLike.ToString())
            {
                var fromId = model.MetaData.GetValue(MetaDataKeys.FromId);
                var profielUrl = repo.Users.Where(m => m.Id == fromId).Select(m => m.ProfileUrl).FirstOrDefault();
                data.Add("ProfileUrl", profielUrl);
            }
            if (notifiationType == UserNotificationType.Follow.ToString())
            {
                var fromId = model.MetaData.GetValue(MetaDataKeys.FromId);

                var profielUrl = repo.Users.Where(m => m.Id == fromId).Select(m => m.ProfileUrl).FirstOrDefault();
                data.Add("ProfileUrl", profielUrl);
            }
            if (notifiationType == UserNotificationType.Comment.ToString())
            {
                var fromId = model.MetaData.GetValue(MetaDataKeys.FromId);
                var profielUrl = repo.Users.Where(m => m.Id == fromId).Select(m => m.ProfileUrl).FirstOrDefault();
                data.Add("ProfileUrl", profielUrl);
            }
            if (notifiationType == UserNotificationType.Share.ToString())
            {
                var fromId = model.MetaData.GetValue(MetaDataKeys.FromId);
                var profielUrl = repo.Users.Where(m => m.Id == fromId).Select(m => m.ProfileUrl).FirstOrDefault();
                data.Add("ProfileUrl", profielUrl);
            }
            return data;
        }
    }
}
