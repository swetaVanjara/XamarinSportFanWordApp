using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.ContentSources
{
	public class ContentSourceRecord
	{
		public string Id { get; set; }
		public string ContentSourceName { get; set; }
		public string NumberPendingRssFeeds { get; set; }
		public bool IsApproved { get; set; }
	}
}
