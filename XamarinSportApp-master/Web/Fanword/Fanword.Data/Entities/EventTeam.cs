using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Data.Enums;
using GenericRepository.Interfaces;

namespace Fanword.Data.Entities {
    public class EventTeam : ISaveable<string>{
        public string Id { get; set; }
        [Required,ForeignKey("Team")]
        public string TeamId { get; set; }
        [Required,ForeignKey("Event")]
        public string EventId { get; set; }

        public WinLossTieEnum? WinLossTie { get; set; }
        [StringLength(15)]
        public string ScorePointsOrPlace { get; set; }
        public DateTime? DateCreatedUtc { get; set; }

        public virtual Team Team { get; set; }
        public virtual Event Event { get; set; }

    }
}
