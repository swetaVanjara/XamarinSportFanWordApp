using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Data.IdentityConfig.User;
using GenericRepository.Interfaces;

namespace Fanword.Data.Entities {
    public class Post  :ISaveable<string>, ISaveableCreate, ISaveableTracking, ISaveableUserTracking<string>, ISaveableDelete{
        public string Id { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        [ForeignKey("Feed")]
        public string FeedId { get; set; }
        public DateTime DateLastModifiedUtc { get; set; }
        [ForeignKey("CreatedBy")]
        public string CreatedById { get; set; }
        [ForeignKey("LastModifiedBy")]
        public string LastModifiedById { get; set; }
        public DateTime? DateDeletedUtc { get; set; }
        public string Content { get; set; }
        [ForeignKey("ContentSource")]
        public string ContentSourceId { get; set; }
        [ForeignKey("Team"), InverseProperty("PostsByTeam")]
        public string TeamId { get; set; }
        [ForeignKey("School"), InverseProperty("PostsBySchool")]
        public string SchoolId { get; set; }
        [ForeignKey("Sport"), InverseProperty("PostsBySport")]
        public string SportId { get; set; }
        [ForeignKey("SharedFromPost")]
        public string SharedFromPostId { get; set; }


        public virtual RssFeed Feed { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<School> Schools { get; set; }
        public virtual ICollection<Sport> Sports { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<PostImage> PostImages { get; set; }
        public virtual ICollection<PostVideo> PostVideos { get; set; }
        public virtual ICollection<PostLink> PostLinks { get; set; }
        public virtual ICollection<UserNotification> UserNotifications { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual ApplicationUser LastModifiedBy { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<PostLike> Likes { get; set; }
        public virtual ContentSource ContentSource { get; set; }
        public virtual School School { get; set; }
        public virtual Team Team { get; set; }
        public virtual Sport Sport { get; set; }
        public virtual ICollection<PostShare> Shares { get; set; }
        public virtual Post SharedFromPost { get; set; }
        public virtual ICollection<Post> ChildPostShares { get;set; }
    }
}
