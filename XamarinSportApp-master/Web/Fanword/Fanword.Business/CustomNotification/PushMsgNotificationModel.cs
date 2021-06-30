using Notifications.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.CustomNotification
{
    public class PushMsgNotificationModel
    {
        public string NotificationUniqueId { get; private set; }
        public string title { get; set; }
        public NotificationType type { get; set; }
        public string CreatedById { get; set; }
        public Dictionary<string, string> metaData { get; set; }
        public string content { get; set; }

        public PushMsgNotificationModel()
        {
            NotificationUniqueId = System.Guid.NewGuid().ToString();
        }
    }
}
