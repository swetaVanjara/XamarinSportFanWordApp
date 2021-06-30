using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Poco.Models
{
    public class Post
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public List<PostImage> Images { get; set; }
        public List<PostVideo> Videos { get; set; }
        public List<PostLink> Links { get; set; }
        public List<string> Schools { get; set; }
        public List<string> Teams { get; set; }
        public List<string> Sports { get; set; }
        public List<string> Events { get; set; }
        public string ContentSourceId { get; set; }
        public string TeamId { get; set; }
        public string SchoolId { get; set; }
        public bool IsShared { get; set; }
        public string SharedPostId { get; set; }
    }
}
