using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.Rankings {
    public class RankingTeamViewModel {
        public string Id { get; set; }
        public int RankingNumber { get; set; }
        [Required]
        public string TeamId { get; set; }
    }
}
