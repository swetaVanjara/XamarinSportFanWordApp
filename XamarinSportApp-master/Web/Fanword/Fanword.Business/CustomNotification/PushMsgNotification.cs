using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Business.CustomNotification;
using System.Web.Script.Serialization;

namespace Fanword.Business.CustomNotification
{
    public class PushMsgNotification
    {
        public async void PushMessageAsync(PushMsgNotificationModel pushMsgNotificationModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            string pushMsgNotificationJson = js.Serialize(new
            {
                data = (pushMsgNotificationModel)
            });
            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;
            outcome = await CustomNotification.Notifications.Instance.Hub.SendGcmNativeNotificationAsync(pushMsgNotificationJson, pushMsgNotificationModel.CreatedById);
            outcome = await CustomNotification.Notifications.Instance.Hub.SendAppleNativeNotificationAsync(pushMsgNotificationJson, pushMsgNotificationModel.CreatedById);
            //CustomNotification.Notifications.Instance.Hub.SendDirectNotificationAsync(new Microsoft.Azure.NotificationHubs.Notification().
            //{

            //});
        }
    }
}
