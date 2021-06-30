using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.Repository;

namespace Fanword.Business.Builders.Mobile.MyProfileDetails
{
    public class MyProfileDetailsBuilder : BaseBuilder
    {
        public MyProfileDetailsBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        public async Task<Poco.Models.MyProfileDetails> BuildAsync(string userId)
        {
            return _repo.Users.Where(m => m.Id == userId).Select(m => new Poco.Models.MyProfileDetails()
            {
                Followers = m.Followers.Count,
                Posts = m.CreatedByPosts.Count(j => string.IsNullOrEmpty(j.ContentSourceId) && string.IsNullOrEmpty(j.TeamId) && string.IsNullOrEmpty(j.SchoolId) && j.DateDeletedUtc == null),
                Following = m.Users.Count(j => j.DateDeletedUtc == null) + m.Teams.Count(j => j.DateDeletedUtc == null) + m.Sports.Count(j => j.DateDeletedUtc == null) + m.ContentSources.Count + m.Schools.Count(j => j.DateDeletedUtc == null)
            }).FirstOrDefault();
        }


    }
}
