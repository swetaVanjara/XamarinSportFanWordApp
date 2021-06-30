using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Poco.Interfaces;

namespace Fanword.Poco.Models
{
    public class Follower : IFollowing
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileUrl { get; set; }
        public bool IsFollowing { get; set; }
    }
}
