using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.Likes {
    public class LikeRecord {
        public string Id { get; set; }
        public string LikedItem { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedById { get; set; }
        public DateTime DateCreatedUtc { get; set; }
    }
}
