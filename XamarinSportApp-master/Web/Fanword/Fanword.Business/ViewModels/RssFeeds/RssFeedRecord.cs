using Fanword.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.RssFeeds {
    public class RssFeedRecord {
        public string Id { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string AssociatedSchoolOrTeam { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? DateDeletedUtc { get; set; }
		public RssFeedStatus RssFeedStatus { get; set; }
		public string ImageUrl { get;set;}
    }
}
