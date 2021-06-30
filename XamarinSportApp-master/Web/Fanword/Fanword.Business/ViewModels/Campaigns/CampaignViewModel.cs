using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Data.Enums;

namespace Fanword.Business.ViewModels.Campaigns {
    public class CampaignViewModel {
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Url { get; set; }
        public string Description { get; set; }
        public string AdvertiserId { get; set; }
        [Required(ErrorMessage = "Start is required")]
        public DateTime? StartUtc { get; set; }
        [Required(ErrorMessage = "End is required")]
        public DateTime? EndUtc { get; set; }
        [Range(1, int.MaxValue)]
        public int Weight { get; set; }
        public List<string> SchoolIds { get; set; }
        public List<string> TeamIds { get; set; }
        public List<string> SportIds { get; set; }
        [Required(ErrorMessage = "Campaign image is required")]
        public string ImageUrl { get; set; }
        public string ImageBlob { get; set; }
        public string ImageContainer { get; set; }
        public CampaignStatus CampaignStatus { get; set; }
    }
}
