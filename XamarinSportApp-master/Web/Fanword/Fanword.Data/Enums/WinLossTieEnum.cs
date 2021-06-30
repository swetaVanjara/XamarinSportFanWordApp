using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Data.Enums {
    public enum WinLossTieEnum {
        [Display(Name = "Win")]
        Win,
        [Display(Name = "Loss")]
        Loss,
        [Display(Name = "Tie")]
        Tie,
    }
}
