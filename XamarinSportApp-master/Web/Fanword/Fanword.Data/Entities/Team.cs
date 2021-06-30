using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Data.IdentityConfig.User;
using GenericRepository.Interfaces;

namespace Fanword.Data.Entities {
    public class Team : ISaveable<string>, ISaveableCreate, ISaveableDelete, ISaveableActive {
        public string Id { get; set; }
        [Required, ForeignKey("School")]
        public string SchoolId { get; set; }
        [Required, ForeignKey("Sport")]
        public string SportId { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public DateTime? DateDeletedUtc { get; set; }
        public bool IsActive { get; set; }

        [StringLength(100)]
        public string Nickname { get; set; }

        [Required, StringLength(10)]
        public string PrimaryColor { get; set; }
        [StringLength(10)]
        public string SecondaryColor { get; set; }
        [Required, StringLength(500)]
        public string ProfileContainer { get; set; }
        [Required, StringLength(500)]
        public string ProfileBlob { get; set; }
        [Required]
        public string ProfilePublicUrl { get; set; }

        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string RosterUrl { get; set; }
        public string ScheduleUrl { get; set; }

        public virtual School School { get; set; }
        public virtual Sport Sport { get; set; }
        public virtual ICollection<RssFeed> RssFeeds { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<UserNotification> UserNotifications { get; set; }
        public virtual ICollection<NewsNotification> NewsNotifications { get; set; }
        public virtual ICollection<EventTeam> EventTeams { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<Athelete> Atheletes { get; set; }
        public virtual ICollection<ApplicationUser> Followers { get; set; }
        public virtual ICollection<TeamAdmin> Admins { get; set; }
        public virtual ICollection<Post> PostsByTeam { get; set; }
    }
}
