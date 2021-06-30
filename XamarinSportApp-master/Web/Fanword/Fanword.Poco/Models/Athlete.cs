using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Poco.Models
{
    public class Athlete
    {
        public string Id { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime? EndUtc { get; set; }
        public string TeamId { get; set; }
        public string ApplicationUserId { get; set; }

    }
}
