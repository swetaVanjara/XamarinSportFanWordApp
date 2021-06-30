using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.RssKeywords
{
	public class RssKeywordViewModel
	{
		public string Id { get; set; }
		[Required]
		public string Keyword { get; set; }
		public bool IsActive { get; set; }
		public string RssKeywordTypeId { get; set; }
		public string RssFeedId { get; set; }

	}
}
