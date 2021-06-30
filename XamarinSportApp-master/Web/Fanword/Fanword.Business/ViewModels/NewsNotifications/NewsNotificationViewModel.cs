using Fanword.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.NewsNotifications {
    public class NewsNotificationViewModel {
        public string Id { get; set; }
        public DateTime PushDateUtc { get; set; }
        public string TeamId { get; set; }
        public string SchoolId { get; set; }
        public string SportId { get; set; }
        [Required]
        public string Title { get; set; }
        [StringLength(150)]
        public string Content { get; set; }
		public NewsNotificationStatus Status { get; set; }
		public string ContentSourceId { get; set; }
		public string HangfireTaskId { get; set; }
		public string CreatedById { get; set; }
	}
}
