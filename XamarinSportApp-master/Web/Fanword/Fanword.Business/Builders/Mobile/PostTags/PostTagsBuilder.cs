using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.Repository;
using Fanword.Poco.Models;

namespace Fanword.Business.Builders.Mobile.PostTags
{
    public class PostTagsBuilder : BaseBuilder
    {
        public PostTagsBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        public async Task<List<PostTag>> BuildAsync(string postId)
        {
            var post = _repo.Posts.Where(m => m.Id == postId);
            List<PostTag> tags = new List<PostTag>();
            tags.AddRange(await post.SelectMany(m => m.Teams).Select(m => new PostTag() { Id = m.Id, ProfileUrl = m.ProfilePublicUrl, Title = m.School.Name, Subtitle = m.Sport.Name, Type = FeedType.Team}).ToListAsync());
            tags.AddRange(await post.SelectMany(m => m.Schools).Select(m => new PostTag() { Id = m.Id, ProfileUrl = m.ProfilePublicUrl, Title = m.Name, Type = FeedType.School }).ToListAsync());
            tags.AddRange(await post.SelectMany(m => m.Sports).Select(m => new PostTag() { Id = m.Id, ProfileUrl = m.IconPublicUrl, Title = m.Name, Type = FeedType.Sport }).ToListAsync());
            tags.AddRange(await post.SelectMany(m => m.Events).Select(m => new PostTag() { Id = m.Id, Title = m.Name, Subtitle = m.Sport.Name, Type = FeedType.Event }).ToListAsync());
            return tags;
        }
    }
}
