using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Poco.Interfaces;
namespace Fanword.Poco.Models
{
    public class UserProfile : IFollowing
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public bool IsFollowing { get; set; }
        public int Posts { get; set; }
        public int Followers { get; set; }
        public string Athlete { get; set; }
        public string ProfileUrl { get; set; }

    }
}
