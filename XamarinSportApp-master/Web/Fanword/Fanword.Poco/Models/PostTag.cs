using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Poco.Models
{
    public class PostTag
    {
        public string Id { get; set; }
        public FeedType Type { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string ProfileUrl { get; set; }
    }
}
