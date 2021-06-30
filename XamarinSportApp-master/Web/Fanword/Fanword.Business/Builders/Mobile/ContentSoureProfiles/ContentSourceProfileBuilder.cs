using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.Repository;
using Fanword.Poco.Models;

namespace Fanword.Business.Builders.Mobile.ContentSoureProfiles
{
    public class ContentSourceProfileBuilder : BaseBuilder
    {
        public ContentSourceProfileBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        public async Task<ContentSourceProfile> BuildAsync(string userId, string contentSourceId)
        {
            var followedContentSources = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.ContentSources.Select(j => j.Id));

            return _repo.ContentSources.Where(m => m.Id == contentSourceId).Select(m => new ContentSourceProfile()
            {
                Followers = m.Followers.Count,
                Posts = m.Posts.Count(j => j.DateDeletedUtc == null),
                Name = m.ContentSourceName,
                Description = m.ContentSourceDescription,
                ContentSourceId = m.Id,
                ProfileUrl = m.LogoUrl,
                IsFollowing = followedContentSources.Contains(m.Id),
                FacebookLink = m.FacebookLink,
                InstagramLink = m.InstagramLink,
                PrimaryColor = m.PrimaryColor,
                TwitterLink = m.TwitterLink,
                WebsiteLink = m.WebsiteLink,
                ActionButtonLink = m.ActionLink,
                ActionButtonText = m.ActionText,
            }).FirstOrDefault();
        }
    }
}
