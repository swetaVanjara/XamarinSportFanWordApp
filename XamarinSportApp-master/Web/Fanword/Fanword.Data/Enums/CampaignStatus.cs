using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Data.Enums {
    public enum CampaignStatus {
        [Display(Name = "Pending")]
        Pending,
        [Display(Name = "Approved")]
        Approved,
        [Display(Name = "Denied")]
        Denied
    }
}
