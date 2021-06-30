using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.Interfaces;

namespace Fanword.Data.Entities {
    public class RankingTeam :ISaveable<string>,ISaveableCreate{
        public string Id { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public int RankingNumber { get; set; }
        [ForeignKey("Team")]
        public string TeamId { get; set; }
        [Required,ForeignKey("Ranking")]
        public string RankingId { get; set; }

        public virtual Ranking Ranking { get; set; }
        public virtual Team Team { get; set; }
    }
}
