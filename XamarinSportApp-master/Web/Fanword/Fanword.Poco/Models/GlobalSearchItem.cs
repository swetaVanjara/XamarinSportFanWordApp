using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Poco.Interfaces;

namespace Fanword.Poco.Models
{
    public class GlobalSearchItem : IFollowing
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int Followers { get; set; }
        public string ProfileUrl { get; set; }
        public bool IsFollowing { get; set; }
        public FeedType Type { get; set; }
    }
}
