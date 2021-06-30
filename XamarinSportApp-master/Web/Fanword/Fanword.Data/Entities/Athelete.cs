using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Data.IdentityConfig.User;
using GenericRepository.Interfaces;

namespace Fanword.Data.Entities {
    public class Athelete : ISaveable<string>, ISaveableStart{
        public string Id { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime? EndUtc { get; set; }
        public bool Verified { get; set; }
        [Required,ForeignKey("Team")]
        public string TeamId { get; set; }
        [Required,ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }

        public virtual Team Team { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
