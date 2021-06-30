using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.Comments {
    public class CommentRecord {
        public string Id { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedById { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public string PostId { get; set; }
        public string Content { get; set; }
    }
}
