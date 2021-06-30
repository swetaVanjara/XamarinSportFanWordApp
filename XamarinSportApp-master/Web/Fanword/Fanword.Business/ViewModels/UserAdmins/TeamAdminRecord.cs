using Fanword.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.UserAdmins
{
	public class TeamAdminRecord
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string TeamName { get; set; }
		public AdminStatus Status { get; set; }
	}
}
