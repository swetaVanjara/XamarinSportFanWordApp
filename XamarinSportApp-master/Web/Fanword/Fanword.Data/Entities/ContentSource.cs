using Fanword.Data.IdentityConfig.User;
using GenericRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Data.Entities
{
	public class ContentSource : ISaveable<string>, ISaveableCreate
	{
	
		public string ContactName { get; set; }
		public string Id { get; set; }
		public DateTime DateCreatedUtc { get; set; }
		[Required, StringLength(500)]
		public string ContentSourceName { get; set; }
		[Required]
		public string ContentSourceDescription { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string WebsiteLink { get; set; }
		public string FacebookLink { get; set; }
		public bool FacebookShow { get; set; }
		public string TwitterLink { get; set; }
		public bool TwitterShow { get; set; }
		public string InstagramLink { get; set; }
		public bool InstagramShow { get; set; }
		public string ActionText { get; set; }
		public string ActionLink { get; set; }
		public string PrimaryColor { get; set; }
		[Required]
		public bool IsApproved { get; set; }
		[Required]
		public string LogoUrl { get; set; }
		[Required, StringLength(500)]
		public string LogoContainer { get; set; }
		[Required, StringLength(500)]
		public string LogoBlob { get; set; }

		public virtual ICollection<RssFeed> RssFeeds { get; set; }
		public virtual ICollection<ApplicationUser> ContentSourceUsers { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<ApplicationUser> Followers { get; set; }
	}
}
