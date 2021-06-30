using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Poco.Models
{
    public class EventProfile
    {
        public string Location { get; set; }
        public string TicketUrl { get; set; }
        public int Posts { get; set; }
        public string WinningTeamUrl { get; set; }
        public string SportName { get; set; }
        public int TeamCount { get; set; }
        public string Name { get; set; }
        public string Team2Score { get; set; }
        public string Team1Score { get; set; }
        public string Team2Url { get; set; }
        public string Team1Url { get; set; }
        public string Team2Name { get; set; }
        public string Team1Name { get; set; }
        public DateTime DateOfEventUtc { get; set; }
        public string Id { get; set; }
        public string TimezoneId { get; set; }
        public bool IsTbd { get; set; }
    }
}

