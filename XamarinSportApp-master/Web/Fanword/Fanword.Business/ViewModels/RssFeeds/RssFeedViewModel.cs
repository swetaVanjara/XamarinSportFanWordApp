using Fanword.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace Fanword.Business.ViewModels.RssFeeds {
    public class RssFeedViewModel {
        public string Id { get; set; }
        [Required]
        public string Url { get; set; }
        [Required,StringLength(300)]
        public string Name { get; set; }
		public DateTime? DateDeletedUtc { get; set; }
        public bool IsActive { get; set; }
        public string TeamId { get; set; }
        public string SchoolId { get; set; }
		public string SportId { get; set; }
        public string ContentSourceId { get; set; }
        [Required(ErrorMessage = "Body mapping is required")]
        public string MappedBody { get; set; }
        [Required(ErrorMessage = "Date Created mapping is required")]
        public string MappedCreatedAt { get; set; }
		public string CreatedByName { get; set; }
		public RssFeedStatus RssFeedStatus { get; set; }
		public List<RssKeywords.RssKeywordViewModel> RssKeywords { get; set; }

	}
}
