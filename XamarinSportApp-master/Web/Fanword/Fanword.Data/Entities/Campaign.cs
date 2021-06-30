using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Data.Enums;
using GenericRepository.Interfaces;

namespace Fanword.Data.Entities {
    public class Campaign :ISaveable<string>, ISaveableCreate, ISaveableStart,ISaveableEnd, ISaveableDelete{
        public string Id { get; set; }
        public DateTime? DateDeletedUtc { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public CampaignStatus CampaignStatus { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string ImageBlob { get; set; }
        [Required]
        public string ImageContainer { get; set; }
        [Required,StringLength(200)]
        public string Title { get; set; }
        public string Description { get; set; }
        public int Weight { get; set; }
        [Required,ForeignKey("Advertiser")]
        public string AdvertiserId { get; set; }
        public float ImageAspectRatio { get; set; }

        public virtual Advertiser Advertiser { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<Sport> Sports { get; set; }
        public virtual ICollection<School> Schools { get; set; }
    }
}
