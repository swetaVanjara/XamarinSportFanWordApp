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
    public class Comment : ISaveable<string>, ISaveableCreate, ISaveableDelete
    {
        public string Id { get; set; }
        public string Content { get; set; }
        [ForeignKey("ParentComment")]
        public string ParentCommentId { get; set; }
        [Required, ForeignKey("CreatedBy")]
        public string CreatedById { get; set; }
        [Required, ForeignKey("Post")]
        public string PostId { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public DateTime? DateDeletedUtc { get; set; }

        public virtual Post Post { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual Comment ParentComment { get; set; }
        public virtual List<CommentLike> Likes { get; set; }
    }
}
