using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Data.IdentityConfig.User;
using GenericRepository.Interfaces;

namespace Fanword.Data.Entities
{
    public class CommentLike : ISaveable<string>, ISaveableCreate
    {
        public string Id { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        [Required, ForeignKey("CreatedBy")]
        public string CreatedById { get; set; }
        [Required, ForeignKey("Comment")]
        public string CommentId { get; set; }
        
        public virtual Comment Comment { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }
    }
}
