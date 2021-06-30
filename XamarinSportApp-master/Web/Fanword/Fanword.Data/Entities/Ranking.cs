using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.Interfaces;

namespace Fanword.Data.Entities {
    public class Ranking :ISaveable<string>, ISaveableCreate{
        public string Id { get; set; }
        public DateTime DateCreatedUtc { get; set; }
		public DateTime DateModifiedUtc { get; set; }
        [Required,ForeignKey("Sport")]
        public string SportId { get; set; }
        public virtual Sport Sport { get; set; }
        public virtual ICollection<RankingTeam> RankingTeams { get; set; }

    }
}
