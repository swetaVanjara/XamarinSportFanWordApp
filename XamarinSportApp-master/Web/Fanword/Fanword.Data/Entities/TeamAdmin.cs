using Fanword.Data.Enums;
using Fanword.Data.IdentityConfig.User;
using GenericRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Data.Entities
{
	public class TeamAdmin : ISaveable<string>, ISaveableDelete
	{
		public string Id { get; set; }
		[Required, ForeignKey("User")]
		public string UserId { get; set; }
		[Required, ForeignKey("Team")]
		public string TeamId { get; set; }
		[Required]
		public AdminStatus AdminStatus { get; set; }
		public DateTime? DateDeletedUtc { get; set; }

		public virtual ApplicationUser User { get; set; }
		public virtual Team Team { get; set; }
	}
}
