using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Poco.Interfaces;

namespace Fanword.Poco.Models
{
    public class Ranking : IFollowing
    {

        public string Id { get; set; }
        public string TeamName { get; set; }
        public string SportName { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
        public int Ties { get; set; }
        public int Rank { get; set; }
        public string ProfileUrl { get; set; }
        public bool IsFollowing { get; set; }
        public DateTime DateUpdatedUtc { get; set; }
        public string TeamId { get; set; }
        public string SportId { get; set; }
        public bool IsActive { get; set; }

    }
}
