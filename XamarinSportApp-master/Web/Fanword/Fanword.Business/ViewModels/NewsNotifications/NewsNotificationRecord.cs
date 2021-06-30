using Fanword.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.NewsNotifications {
    public class NewsNotificationRecord {
        public string Id { get; set; }
        public DateTime PushDateUtc { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
		public string CreatedBy { get; set; }
		public NewsNotificationStatus NewsNotificationStatus { get; set; }
    }
}
