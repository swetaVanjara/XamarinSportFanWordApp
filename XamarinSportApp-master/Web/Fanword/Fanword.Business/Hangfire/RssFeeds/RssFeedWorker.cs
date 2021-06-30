using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Business.Builders.RssFeeds;
using Fanword.Data.Repository;
using Hangfire;
using Fanword.Data.Entities;
using Fanword.Data.Enums;
using System.Data.Entity;

namespace Fanword.Business.Hangfire.RssFeeds {
    public class RssFeedWorker {
        private ApplicationRepository _repo { get; set; }

        public RssFeedWorker() {
            _repo = ApplicationRepository.CreateWithoutOwin();
        }
        
        [AutomaticRetry(Attempts = 1),Queue("rss_feed")]
        public async Task SyncRssFeed(string id) {
            try
            {
                var dbFeed = _repo.RssFeeds.Where(m => ((m.Team.IsActive && m.Team.DateDeletedUtc == null) || 
                                                        (m.Sport.IsActive && m.Sport.DateDeletedUtc == null) || 
                                                        (m.School.IsActive && m.School.DateDeletedUtc == null) ||
                                                        (m.ContentSourceId != null && m.TeamId == null && m.SportId == null && m.SchoolId == null)) &&
                                                        (m.RssFeedStatus == RssFeedStatus.Approved && (m.Id == id))).FirstOrDefault();

                //var dbFeed = _repo.RssFeeds.GetById(id);
                //System.Diagnostics.Debug.WriteLine(dbFeed.RssKeywords);
                if (dbFeed != null)
                {
                    await new RssFeedXmlGenerator(dbFeed.Url).CreatePosts(dbFeed);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }


        [Queue("rss_feed")]
        public void StartSyncAll() {
            var feeds = _repo.RssFeeds.Where(m => m.IsActive && (m.DateDeletedUtc == null)).Select(i => i.Id).ToList();
            //var feeds = _repo.RssFeeds.Where(m => m.IsActive && (m.DateDeletedUtc == null) && ((m.Team.IsActive && m.Team.DateDeletedUtc == null) || (m.Sport.IsActive && m.Sport.DateDeletedUtc == null) || (m.School.IsActive && m.School.DateDeletedUtc == null) || (m.ContentSourceId != null))).Select(i=>i.Id).ToList();
            if (!feeds.Any()) return;
            try {
                BatchJob.StartNew(client =>
                {
                    feeds.ForEach(feedId =>
                    {
                        client.Enqueue(() => SyncRssFeed(feedId));
                    });
                }, "All Rss Feed Sync Process");
            }catch(Exception ex)
            {
                return;
            }
        }
	}
}
