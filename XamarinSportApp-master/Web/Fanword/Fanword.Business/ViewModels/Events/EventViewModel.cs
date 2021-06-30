using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.Events {
    public  class EventViewModel {
        public string Id { get; set; }
        public string TimezoneId { get; set; }
        public string Name { get; set; }
        public string PurchaseTicketsUrl { get; set; }
        public bool ShowEventTeams { get; set; }
        public DateTime DateOfEventUtc { get; set; }
        [Required(ErrorMessage = "Event date is required")]
        public DateTime StringConversionDate { get; set; }
        public DateTime DateOfEventInTimezone { get; set; } //USED FOR QUERIES ONLY
        [Required, StringLength(255)]
        public string Location { get; set; }
        [Required(ErrorMessage = "Sport is required")]
        public string SportId { get; set; }
        public string SportDisplay { get; set; }
        public bool IsDeleted { get; set; }
        public bool EditEvent { get; set; }  

        public List<EventTeamViewModel> EventTeams { get; set; }
    }
}
