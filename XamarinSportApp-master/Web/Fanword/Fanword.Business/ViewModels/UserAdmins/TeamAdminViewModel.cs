using Fanword.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.UserAdmins
{
	public class TeamAdminViewModel
	{
		public string Id { get; set; }
		[Required]
		public string UserId { get; set; }
		[Required]
		public string TeamId { get; set;}
		[Required]
		public string ContactEmail { get; set; }
		[Required]
		public AdminStatus AdminStatus { get; set; }
	}
}
