using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Shared.Models
{
    public class UserNotification
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string ProfileUrl { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public Dictionary<string, string> MetaData { get; set; }
        public Dictionary<string, string> UserMetaData { get; set; }
    
    }
}
