using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.Events {
    public class EventRecord {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Sport { get; set; }
		public string Location { get; set; }
        public DateTime DateOfEventUtc { get; set; }
        public DateTime DateOfEventInTimezone { get; set; }
        public string TimeOfEventInTimezone { get; set; }
        public string Teams => String.Join(", ", TeamNames);
        public string SportId { get; set; }
        public List<string> TeamNames { get; set; }
        public List<string> TeamIds { get; set; }
		public string TimezoneId { get; set; }

    }
}
