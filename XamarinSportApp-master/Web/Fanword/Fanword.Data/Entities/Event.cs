using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.Interfaces;

namespace Fanword.Data.Entities {
    public class Event :ISaveable<string>, ISaveableCreate, ISaveableDelete{
        public string Id { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public DateTime DateOfEventUtc { get; set; }
        public DateTime? DateDeletedUtc { get; set; }
        [Required]
        public string TimezoneId { get; set; }
        public string Name { get; set; }
        public string PurchaseTicketsUrl { get; set; }

        //[Required,ForeignKey("Facility")]
        //public string FacilityId { get; set; }
        [ForeignKey("Sport")]
        public string SportId { get; set; }
        [Required, StringLength(255)]
        public string Location { get; set; }
        public virtual Sport Sport { get; set; }
        //public virtual Facility Facility { get; set; }
        public virtual  ICollection<EventTeam> EventTeams { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
