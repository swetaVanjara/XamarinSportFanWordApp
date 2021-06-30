using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.Users {
    public class UserViewModel {
        public string Id { get; set; }
        [Required,Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
        [EmailAddress,Required]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public DateTime? DateDeletedUtc { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Profile Photo Required")]
        public string ProfileUrl { get; set; }
        public string ProfileContainer { get; set; }
        public string ProfileBlob { get; set; }
		public string ContentSourceId { get; set; }
    }
}
