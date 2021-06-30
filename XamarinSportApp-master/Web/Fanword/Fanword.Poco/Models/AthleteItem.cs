using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Poco.Interfaces;

namespace Fanword.Poco.Models
{
    public class AthleteItem : IFollowing
    {
        public string Id { get; set; }
        public string SchoolName  { get; set; }
        public string SportName { get; set; }
        public string ProfileUrl { get; set; }
        public bool IsFollowing { get; set; }
        public string Name { get; set; }

    }
}
