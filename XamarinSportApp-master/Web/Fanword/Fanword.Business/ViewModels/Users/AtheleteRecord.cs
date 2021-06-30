using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.Users {
    public class AtheleteRecord {
        public DateTime StartUtc { get; set; }
        public DateTime? EndUtc { get; set; }
        public string UserId { get; set; }
        public string Team { get; set; }
        public string Id { get; set; }
        public bool Verified { get; set; }
    }
}
