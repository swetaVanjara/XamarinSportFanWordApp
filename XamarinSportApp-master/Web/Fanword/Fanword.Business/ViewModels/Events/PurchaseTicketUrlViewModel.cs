using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.Events
{
    public class PurchaseTicketUrlViewModel
    {
        [Url]
        public string PurchaseTicketsUrl { get; set; }
    }
}
