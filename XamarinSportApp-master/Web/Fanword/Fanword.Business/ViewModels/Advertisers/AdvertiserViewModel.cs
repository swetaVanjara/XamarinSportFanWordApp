using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.Advertisers {
    public class AdvertiserViewModel {
        public string Id { get; set; }
        [Required,Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Required, Display(Name = "Company Contact Name")]
        public string ContactName { get; set; }
        [Required, Display(Name = "Website")]
        public string WebsiteLink { get; set; }
        [Display(Name = "Company Description"), Required]
        public string CompanyDescription { get; set; }
        public string LogoBlob { get; set; }
        public string LogoContainer { get; set; }
        [Required(ErrorMessage = "Logo is required")]
        public string LogoUrl { get; set; }

    }
}
