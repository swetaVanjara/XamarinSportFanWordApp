using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Poco.Interfaces;
namespace Fanword.Poco.Models
{
    public class SportProfile : IFollowing
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IconPublicUrl { get; set; }
        public bool IsFollowing { get; set; }
        public int Posts { get; set; }
        public int Followers { get; set; }
    }
}
