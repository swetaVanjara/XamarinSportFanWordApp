using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.Registration {
    public class AdvertiserRegistrationViewModel {
        public string Email { get; set; }
        [Required,Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required,Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required,Display(Name = "Website")]
        public string WebsiteLink { get; set; }
        [Required,Display(Name = "Company name")]
        public string CompanyName { get; set; }
        [Display(Name = "Company Description"), Required]
        public string CompanyDescription { get; set; }

        public bool IsAccountCreated { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string LogoBlob { get; set; }
        public string LogoContainer { get; set; }
        [Required(ErrorMessage = "Logo is required")]
        public string LogoUrl { get; set; }

    }
}
