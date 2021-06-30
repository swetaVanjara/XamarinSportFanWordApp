using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Fanword.Business.ViewModels.RssFeeds;
using Fanword.Data.Entities;
using Fanword.Data.Repository;
using System.Data.Entity;

namespace Fanword.Business.Builders.RssFeeds {
    public class RssFeedXmlGenerator {
        private ApplicationRepository _repo { get; set; }
        private XDocument _feedXml { get; set; }
        public RssFeedXmlGenerator(ApplicationRepository repo) {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
            _feedXml = new XDocument();
        }

        public RssFeedXmlGenerator(string url):this(ApplicationRepository.CreateWithoutOwin()) {
            DownloadXml(url);
        }

        public List<MappingOption> MappingOptions() {
            // Assume 'item' is a valid descendant
            var item = _feedXml.Descendants().FirstOrDefault(i => i.Name.LocalName == "item" || i.Name.LocalName == "entry");

            if (item == null)                return new List<MappingOption>();

            return item.Descendants().Select(i => new MappingOption(i.Name.LocalName, new RssFeedHtmlCleaner().StripHTML(i.Value))).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task CreatePosts(RssFeed feed) {
			var feedWKeywords = await _repo.RssFeeds.GetByIdAsync(feed.Id, query => query.Include(i => i.RssKeywords));

			//Debug.WriteLine(feedWKeywords.RssKeywords.ToList());

            var feedItems =_feedXml.Descendants().Where(i => i.Name.LocalName == "item" || i.Name.LocalName == "entry").ToList().Select(i => new RssFeedItem(i,feedWKeywords, _repo)).ToList();
			//System.Diagnostics.Debug.WriteLine(feed.RssKeywords + "blhe");


            foreach (var feedItem in feedItems)
            {
                if (feedItem.ContainsKeywords)
                {
                    await feedItem.AddToDatabase();
                }
            }

            try
            {
                _repo.Save();
            }
            catch (Exception ex)
            {
                //Debugger.Break();
            }
        }

        private void DownloadXml(string url) {
            using (var client = new WebClient()) {
                // Some RSS Feeds will not work without a UserAgent header specified
                client.Headers.Add(HttpRequestHeader.UserAgent, Guid.NewGuid().ToString());
                try {
                    _feedXml = XDocument.Parse(client.DownloadString(url));
                }
                catch { }
            }
        }


    }
}
