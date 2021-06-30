using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Data.IdentityConfig.User;
using GenericRepository.Interfaces;

namespace Fanword.Data.Entities {
    public class Advertiser  :ISaveable<string>, ISaveableCreate{
        public string Id { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        [Required,StringLength(500)]
        public string CompanyName { get; set; }
        [Required,StringLength(500)]
        public string ContactName { get; set; }
        [Required]
        public string CompanyDescription { get; set; }
        
        public string WebsiteLink { get; set; }
        [Required]
        public string LogoUrl { get; set; }
        [Required,StringLength(500)]
        public string LogoContainer { get; set; }
        [Required,StringLength(500)]
        public string LogoBlob { get; set; }
        

        public virtual ICollection<ApplicationUser> AdvertiserUsers { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }
    }
}
