using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.Events
{
    public class EventManagementPinViewModel
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Pin number is required")]
        public string PinNumber { get; set;}
        public DateTime DateCreatedUtc { get; set;}


    }
}
