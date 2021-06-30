using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.Teams {
    public class TeamViewModel {
        
        public string Id { get; set; }
        [Required(ErrorMessage = "Profile Picture is Required")]
        public string ProfileContainer { get; set; }
        [Required(ErrorMessage = "Profile Picture is Required")]
        public string ProfileBlob { get; set; }
        [Required(ErrorMessage = "Profile Picture is Required")]
        public string ProfilePublicUrl { get; set; }
        public bool IsActive { get; set; }
        [StringLength(100)]
        public string Nickname { get; set; }
        [Required, StringLength(10)]
        public string PrimaryColor { get; set; }
        [StringLength(10)]
        public string SecondaryColor { get; set; }
        [Required(ErrorMessage = "School is Required")]
        public string SchoolId { get; set; }
        [Required(ErrorMessage = "Sport is Required")]
        public string SportId { get; set; }

        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string RosterUrl { get; set; }
        public string ScheduleUrl { get; set; }

    }
}
