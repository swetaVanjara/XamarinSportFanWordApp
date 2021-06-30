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
    public class UserNotification : ISaveable<string>{
        public string Id { get; set; }
        [Required,ForeignKey("ForUser")]
        public string ForUserId { get; set; }
        [ForeignKey("ByUser")]
        public string ByUserId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? DateReadUtc { get; set; }
        [ForeignKey("Post")]
        public string PostId { get; set; }
        [ForeignKey("Team")]
        public string TeamId { get; set; }
        [ForeignKey("School")]
        public string SchoolId { get; set; }
        [ForeignKey("Sport")]
        public string SportId { get; set; }

        public virtual ApplicationUser ForUser { get; set; }
        public virtual ApplicationUser ByUser { get; set; }
        public virtual Sport Sport { get; set; }
        public virtual Team Team { get; set; }
        public virtual School School { get; set; }
        public virtual Post Post { get; set; }
    }
}
