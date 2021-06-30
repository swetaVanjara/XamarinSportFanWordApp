using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Data.Enums;

namespace Fanword.Business.ViewModels.Sports {
    public class SportViewModel {
        public string Id { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Profile Picture is Required")]
        public string IconContainer { get; set; }
        [Required(ErrorMessage = "Profile Picture is Required")]
        public string IconBlobName { get; set; }
        [Required(ErrorMessage = "Profile Picture is Required")]
        public string IconPublicUrl { get; set; }
    }
}
