using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Poco.Interfaces;

namespace Fanword.Poco.Models
{
    public class SchoolProfile : IFollowing
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsFollowing { get; set; }
        public int Posts { get; set; }
        public int Followers { get; set; }
        public string ProfileUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string PrimaryColor { get; set; }
        public bool IsProfileAdmin { get; set; }
        public string ScheduleUrl { get; set; }
    }
}
