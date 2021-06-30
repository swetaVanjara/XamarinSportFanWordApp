using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Poco.Models
{
    public class AthleteTeamSearch
    {
        public string SearchText { get; set; }
        public List<AthleteTeam> AthleteTeams { get; set; }
    }
}
