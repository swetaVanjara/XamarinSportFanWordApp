using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Fanword.Business.Builders.RssFeeds;
using Fanword.Data.Entities;
using Fanword.Data.Repository;

namespace Fanword.Business.ViewModels.RssFeeds {
    public class RssFeedItem {

        private readonly XElement _item;
        private Post Post { get; set; }
        //public user_stories UserStory { get; set; }
        public string ImageUrl { get; set; }
		public bool ContainsKeywords { get; set; }
        private ApplicationRepository _repo { get; set; }

        public RssFeedItem(XElement item, RssFeed feed, ApplicationRepository repo) {

            _item = item;
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
            // Create post
            Post = new Post() {
               // Id = item.Descendants().FirstOrDefault(m => m.Name.LocalName == "guid" || item.Name.LocalName == "id")?.Value,
                Content = new RssFeedHtmlCleaner().StripHTML(_item.Descendants().FirstOrDefault(d => d.Name.LocalName == feed.MappedBody)?.Value),
                //Title = new RssFeedHtmlCleaner().StripHTML(_item.Descendants().FirstOrDefault(d => d.Name.LocalName == feed.MappedTitle)?.Value),
                DateCreatedUtc = new RssFeedDateTimeParser().Parse(_item.Descendants().FirstOrDefault(d => d.Name.LocalName == feed.MappedCreatedAt)?.Value),
                FeedId = feed.Id,
            };
            
            // Generate GUID 
                Post.Id = new { Post.Content, Post.FeedId, Post.DateCreatedUtc }.GetHashCode().ToString();

            Post.Teams = new List<Team>();
            Post.Schools = new List<School>();
			Post.Sports = new List<Sport>();
            Post.ContentSourceId = feed.ContentSourceId;
           
            if (!String.IsNullOrEmpty(feed.TeamId))
            {
                if (!string.IsNullOrEmpty(Post.ContentSourceId))
                {
                    Post.Teams.Add(_repo.Teams.GetById(feed.TeamId));
                }
                else
                {
                    Post.TeamId = feed.TeamId;
                }
            }
            if (!String.IsNullOrEmpty(feed.SchoolId))
            {
                if (!string.IsNullOrEmpty(Post.ContentSourceId))
                {
                    Post.Schools.Add(_repo.Schools.GetById(feed.SchoolId));
                }
                else
                {
                    Post.SchoolId = feed.SchoolId;
                }
            }
			if (!String.IsNullOrEmpty(feed.SportId))
			{
			    if (!string.IsNullOrEmpty(Post.ContentSourceId))
			    {
			        Post.Sports.Add(_repo.Sports.GetById(feed.SportId));
			    }
                else
			    {
			        Post.SportId = feed.SportId;
			    }
			}
            // Update body to include a link if there is one
            var link = _item.Descendants().FirstOrDefault(i => i.Name.LocalName == "link")?.Value;
            if (!string.IsNullOrEmpty(link)) {

                // If the body is not empty, add a couple new lines to make things look good
                if (!string.IsNullOrEmpty(Post.Content))
                    Post.Content += "\r\n\r\n";

                Post.Content += link;
            }

			RssFeedKeywordParser keywordParser = new RssFeedKeywordParser();

			if(keywordParser.containsKeyword(feed, Post.Content) == true)
			{
				ContainsKeywords = true;
			}
			else
			{
				ContainsKeywords = false;
			}

            ImageUrl = _item.Descendants()
                .FirstOrDefault(
                    i =>
                        (i.Name.LocalName == "enclosure" || i.Name.LocalName == "content" ||
                         i.Attributes().Any(a => a.Name == "type")) && i.Attributes().Any(a => a.Name == "url"))
                ?.Attribute("url")?
                .Value;
            
        }

        public async Task AddToDatabase()
        {
            if (_repo.Context.Posts.Any(i=>i.Id==Post.Id)) return;
            _repo.Posts.AddOrUpdate(Post);

            if (!String.IsNullOrEmpty(ImageUrl))
            {
                var aspectRatio = await GetImageAspectRatio(ImageUrl);
                _repo.PostImages.AddOrUpdate(new PostImage()
                {
                    PostId = Post.Id,
                    ImageAspectRatio = aspectRatio,
                    DateCreatedUtc = DateTime.UtcNow,
                    Url = ImageUrl,
                });
            }
        }

        async Task<float> GetImageAspectRatio(string url)
        {
            HttpClient client = new HttpClient();
            var stream = await client.GetStreamAsync(url);
            var image = System.Drawing.Image.FromStream(stream);
            var ratio = (float)image.Height / (float)image.Width;
            ratio = ratio <= 0 ? 0.5625f : ratio;
            return ratio;
        }

        /// <summary>
        /// Utility method to check names of this item's descendants
        /// </summary>
        /// <returns></returns>
        public List<string> GetDescendantLocalNames() {

            return _item.Descendants().Select(i => i.Name.LocalName).ToList();
        }
    }
}
