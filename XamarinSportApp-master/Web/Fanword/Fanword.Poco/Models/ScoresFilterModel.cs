using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Poco.Models
{
    public class ScoresFilterModel
    {
        public DateFilter DateFilter { get; set; }
        public DateTime Today { get; set; }
        public FollowingFilterModel FollowFilter { get; set; }
        public string TeamId { get; set; }
        public string SchoolId { get; set; }
        public string SportId { get; set; }
    }
}
