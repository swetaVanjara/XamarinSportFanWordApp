using GenericRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Data.Entities
{
	public class RssKeywordType : ISaveable<string>, ISaveableActive
	{
		public string Id { get; set; }
		[Required]
		public string Name { get; set; }
		public bool IsActive { get; set; }

		public virtual ICollection<RssKeyword> RssKeywords { get; set; }
	}
}
