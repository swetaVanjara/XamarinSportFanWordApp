using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Poco.Models
{
    public class ProfileSearch
    {
        public List<Profile> SchoolProfiles { get; set; }
        public List<Profile> TeamProfiles { get; set; }
        public List<Profile> SportProfile { get; set; }

        public string SearchText { get; set; }
    }
}
