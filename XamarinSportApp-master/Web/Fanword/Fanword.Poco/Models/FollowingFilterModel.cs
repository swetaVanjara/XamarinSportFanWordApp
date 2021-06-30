using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Poco.Models
{
    public class FollowingFilterModel
    {
        public bool MyTeams { get; set; }
        public bool MySchools { get; set; }
        public bool MySports { get; set; }
        public string SportId { get; set; }
        public string SchoolId { get; set; }
    }
}
