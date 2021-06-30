using GenericRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Data.Entities
{
	public class ContentSourceRegistrationRequest : ISaveable<string>, ISaveableCreate
	{
		public string Id { get; set; }
		public DateTime DateCreatedUtc { get; set; }
		public string Email { get; set; }
	}
}
