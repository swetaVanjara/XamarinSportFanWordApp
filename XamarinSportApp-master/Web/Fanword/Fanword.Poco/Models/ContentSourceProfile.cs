using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Poco.Interfaces;

namespace Fanword.Poco.Models
{
    public class ContentSourceProfile : IFollowing
    {
        public string ContentSourceId { get; set; }
        public string Name { get; set; }
        public bool IsFollowing { get; set; }
        public int Posts { get; set; }
        public int Followers { get; set; }
        public string Description { get; set; }
        public string ProfileUrl { get; set; }
        public string WebsiteLink { get; set; }
        public string FacebookLink { get; set; }
        public string TwitterLink { get; set; }
        public string InstagramLink { get; set; }
        public string PrimaryColor { get; set; }
        public string ActionButtonText { get; set; }
        public string ActionButtonLink { get; set; }
    }
}
