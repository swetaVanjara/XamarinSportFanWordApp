using System.ComponentModel.DataAnnotations;

namespace Fanword.Business.ViewModels.Schools {
    public class SchoolViewModel {
        public string Id { get; set; }
        [Required(ErrorMessage = "Profile Picture is Required")]
        public string ProfileContainer { get; set; }
        [Required(ErrorMessage = "Profile Picture is Required")]
        public string ProfileBlob { get; set; }
        [Required(ErrorMessage = "Profile Picture is Required")]
        public string ProfilePublicUrl { get; set; }
        public bool IsActive { get; set; }
        [Required,StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Nickname { get; set; }
        [Required, StringLength(10)]
        public string PrimaryColor { get; set; }
        [StringLength(10)]
        public string SecondaryColor { get; set; }

        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string WebsiteUrl { get; set; }
    }
}
