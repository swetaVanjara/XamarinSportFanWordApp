using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.Interfaces;
using Fanword.Data.Enums;
using Fanword.Data.IdentityConfig.User;

namespace Fanword.Data.Entities {
    public class NewsNotification  :ISaveable<string>, ISaveableCreate{
        public string Id { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        [Required]
        public string Title { get; set; }
        [StringLength(150)]
        public string Content { get; set; }
        [ForeignKey("School")]
        public string SchoolId { get; set; }
        [ForeignKey("Team")]
        public string TeamId { get; set; }
        [ForeignKey("Sport")]
        public string SportId { get; set; }
        public string HangfireTaskId { get; set; }
        public DateTime PushDateUtc { get; set; }
		public NewsNotificationStatus NewsNotificationStatus { get; set; }
		[ForeignKey("ContentSource")]
		public string ContentSourceId { get; set; }
		[Required, ForeignKey("User")]
		public string CreatedById { get; set; }

		public ApplicationUser User { get; set; }
		public ContentSource ContentSource { get; set; }
        public School School { get; set; }
        public Team Team { get; set; }
        public Sport Sport { get; set; }
    }
}
