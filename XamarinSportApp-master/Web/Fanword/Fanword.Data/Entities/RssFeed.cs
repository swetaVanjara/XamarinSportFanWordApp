using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Data.IdentityConfig.User;
using GenericRepository.Interfaces;
using Fanword.Data.Enums;

namespace Fanword.Data.Entities {
	public class RssFeed : ISaveable<string>, ISaveableCreate, ISaveableTracking, ISaveableUserTracking<string> {
		public string Id { get; set; }
		public bool IsActive { get; set; }
		public DateTime DateCreatedUtc { get; set; }
		public DateTime DateLastModifiedUtc { get; set; }
		public DateTime? DateDeletedUtc { get; set; }
		[Required, ForeignKey("CreatedBy")]
		public string CreatedById { get; set; }
		[Required, ForeignKey("LastModifiedBy")]
		public string LastModifiedById { get; set; }
		[Required, StringLength(300)]
		public string Name { get; set; }
		[Required, StringLength(500)]
		public string MappedBody { get; set; }
		[Required, StringLength(500)]
		public string MappedCreatedAt { get; set; }
		[Required]
		public string Url { get; set; }
		public RssFeedStatus RssFeedStatus { get; set;}
		[ForeignKey("Team")]
		public string TeamId { get; set; }
		[ForeignKey("School")]
		public string SchoolId { get; set; }
		[ForeignKey("Sport")]
		public string SportId { get; set; }
		[ForeignKey("ContentSource")]
		public string ContentSourceId { get; set; }

		public ContentSource ContentSource { get; set; }
		public Sport Sport { get; set; }
		public Team Team { get; set; }
		public School School { get; set; }
		public ApplicationUser CreatedBy { get; set; }
		public ApplicationUser LastModifiedBy { get; set; }

		public virtual ICollection<Post> Posts { get; set; }
		public virtual ICollection<RssKeyword> RssKeywords{get;set;}

        public RssFeed() {
            IsActive = true;
        }
    }
}
