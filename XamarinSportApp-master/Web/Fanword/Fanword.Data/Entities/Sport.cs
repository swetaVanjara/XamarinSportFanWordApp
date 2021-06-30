using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Data.Enums;
using Fanword.Data.IdentityConfig.User;
using GenericRepository.Interfaces;

namespace Fanword.Data.Entities {
    public class Sport :ISaveable<string>,ISaveableCreate,ISaveableDelete, ISaveableActive{
        public string Id { get; set; }
        public DateTime? DateDeletedUtc { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public bool IsActive { get; set; }
        [Required,StringLength(100)]
        public string Name { get; set; }
        [Required,StringLength(500)]
        public string IconContainer { get; set; }
        [Required,StringLength(500)]
        public string IconBlobName { get; set; }
        [Required]
        public string IconPublicUrl { get; set; }

		public virtual ICollection<RssFeed> RssFeeds { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<UserNotification> UserNotifications { get; set; }
        public virtual ICollection<NewsNotification> NewsNotifications { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<ApplicationUser> Followers { get; set; }
        public virtual ICollection<Ranking> Rankings { get; set; }
        public virtual ICollection<Post> PostsBySport { get; set; }

    }
}
