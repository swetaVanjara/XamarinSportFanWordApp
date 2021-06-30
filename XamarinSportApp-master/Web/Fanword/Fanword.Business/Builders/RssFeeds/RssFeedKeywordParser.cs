using Fanword.Data.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.Builders.RssFeeds
{
	class RssFeedKeywordParser
	{
		public bool containsKeyword(RssFeed feed, string content)
		{
			if(feed.RssKeywords.Count() == 0)
			{
				return true;
			}
			foreach (var keyword in feed.RssKeywords)
			{
				string tempKeyword = keyword.Keyword;
				
				if (content.ToLower().Contains(tempKeyword.ToLower()))
				{
					return true;
				}
			}
			return false;
		}
	}
}
