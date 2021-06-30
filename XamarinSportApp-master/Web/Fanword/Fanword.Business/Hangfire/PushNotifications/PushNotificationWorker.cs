using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Data.Context;
using Fanword.Data.Repository;
using Fanword.Poco.Models;
using Hangfire;
using Notifications.Data.Enum;
using Notifications.Service;
using Fanword.Business.CustomNotification;

namespace Fanword.Business.Hangfire.PushNotifications {
    public class PushNotificationWorker {


        [Queue("push_notifications")]
        public void SendPush(string id) {
            var repo = new ApplicationRepository(new ApplicationDbContext());
            var model = repo.NewsNotifications.FirstOrDefault(m => m.Id == id);
            
            var metaData = new Dictionary<string, string>();
            metaData.Add(MetaDataKeys.UserNotificationType, UserNotificationType.NewsNotification.ToString());
            if (!string.IsNullOrEmpty(model.ContentSourceId))
            {
                metaData.Add(MetaDataKeys.FromId, model.ContentSourceId);
                metaData.Add(MetaDataKeys.FromIdType, FeedType.ContentSource.ToString());
            }
            else if (!string.IsNullOrEmpty(model.TeamId))
            {
                metaData.Add(MetaDataKeys.FromId, model.TeamId);
                metaData.Add(MetaDataKeys.FromIdType, FeedType.Team.ToString());
            }
            else if (!string.IsNullOrEmpty(model.SchoolId))
            {
                metaData.Add(MetaDataKeys.FromId, model.SchoolId);
                metaData.Add(MetaDataKeys.FromIdType, FeedType.School.ToString());
            }
            else if (!string.IsNullOrEmpty(model.SportId))
            {
                metaData.Add(MetaDataKeys.FromId, model.SportId);
                metaData.Add(MetaDataKeys.FromIdType, FeedType.Sport.ToString());
            }
            metaData.Add(MetaDataKeys.NewsNotificationMessage, model.Content);

            PushMsgNotificationModel pushMsgNotificationModel = new PushMsgNotificationModel();
            pushMsgNotificationModel.title = model.Title;
            pushMsgNotificationModel.type = NotificationType.UsersSuppliedAtNotificationEvent;
            pushMsgNotificationModel.metaData = metaData;
            pushMsgNotificationModel.content = model.Content;

            new PushMsgNotification().PushMessageAsync(pushMsgNotificationModel);

            new NotificationService(model.Title, NotificationType.UsersSuppliedAtNotificationEvent).AddMetaData(metaData).AddContent(model.Content).Send();
        }
    }
}

