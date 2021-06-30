using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.Events
{
    public class EventManagementViewModel
    {
        public int EventCount { get; set; }
        public int TotalPages { get; set; }
        public List<EventViewModel> Events { get; set; }
    }
}
