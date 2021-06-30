using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.RssKeywords
{
	public class RssKeywordRecord
	{
		public string Id { get; set; }
		public string Keyword { get; set; }
		public bool Status { get; set; }
		public string RssFeedId { get; set; }
	}
}
