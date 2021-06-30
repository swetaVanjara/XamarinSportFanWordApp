using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Fanword.Business.ViewModels.Events {
    public class EventTeamViewModel {
        public string Id { get; set; }
        [Required(ErrorMessage = "Team is Required")]
        public string TeamId { get; set; }
        public string ScorePointsOrPlace { get; set; }
        public DateTime? DateCreatedUtc { get; set; }
        public WinLossTieEnum? WinLossTie { get; set; }
        public bool IsDeleted { get; set; }

    }
}
