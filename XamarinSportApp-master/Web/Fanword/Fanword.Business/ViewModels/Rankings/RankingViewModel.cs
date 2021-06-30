using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.Rankings {
    public class RankingViewModel {
        public string Id { get; set; }
        public string SportName { get; set; }
        public List<RankingTeamViewModel> RankingTeams { get; set; }
    }
}
