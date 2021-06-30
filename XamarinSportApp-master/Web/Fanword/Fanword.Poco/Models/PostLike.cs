using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Poco.Models
{
    public class PostLike
    {
        public string Id { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public string Username { get; set; }
        public string ProfileUrl { get; set; }
    }
}
