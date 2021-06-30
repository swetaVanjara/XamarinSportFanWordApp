using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Poco.Interfaces;

namespace Fanword.Poco.Models
{
    public class TeamProfile : IFollowing
    {
        public TeamProfile() { }

        public string ScheduleUrl { get; set; }
        public string RosterUrl { get; set; }
        public bool IsProfileAdmin { get; set; }
        public string PrimaryColor { get; set; }
        public string InstagramUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string ProfileUrl { get; set; }
        public int Followers { get; set; }
        public int Posts { get; set; }
        public bool IsFollowing { get; set; }
        public int Ties { get; set; }
        public int Loss { get; set; }
        public int Wins { get; set; }
        public string Ranking { get; set; }
        public string SportName { get; set; }
        public string SchoolName { get; set; }
        public string Id { get; set; }
        public string SportId { get; set; }
        public string SchoolId { get; set; }
    }
}
