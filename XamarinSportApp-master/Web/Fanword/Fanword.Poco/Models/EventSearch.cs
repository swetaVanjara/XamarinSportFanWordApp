using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Poco.Models
{
    public class EventSearch
    {
        public List<EventProfile> Events { get; set; }
        public DateTime SearchTime { get; set; }
    }
}
