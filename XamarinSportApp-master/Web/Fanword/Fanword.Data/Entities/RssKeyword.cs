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
	public class RssKeyword :ISaveable<string>, ISaveableActive
	{
		public string Id { get; set; }
		public bool IsActive { get; set; }
		[Required]
		public string Keyword { get; set; }
		[Required, ForeignKey("RssKeywordType")]
		public string RssKeywordTypeId { get; set; }
		public RssKeywordType RssKeywordType { get; set; }
		[ForeignKey("RssFeed")]
		public string RssFeedId { get; set; }
		public RssFeed RssFeed { get; set; }

	}
}
