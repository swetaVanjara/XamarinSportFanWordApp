using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.Repository;
using Fanword.Poco.Models;

namespace Fanword.Business.Builders.Mobile.UserProfiles
{
    public class UserProfileBuilder : BaseBuilder
    {
        public UserProfileBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        public async Task<UserProfile> BuildAsync(string targetUserId, string userId)
        {
            var followedUsers = _repo.Users.Where(m => m.Id == targetUserId).SelectMany(m => m.Users.Select(j => j.Id));

            return _repo.Users.Where(m => m.Id == userId).Select(m => new UserProfile()
            {
                Followers = m.Followers.Count,
                Posts = m.CreatedByPosts.Count(j => string.IsNullOrEmpty(j.ContentSourceId) && j.DateDeletedUtc == null),
                Name = m.FirstName + " " + m.LastName,
                Athlete = m.Atheletes.Select(j => j.Team.School.Name + " - " + j.Team.Sport.Name).FirstOrDefault(),
                UserId = m.Id,
                ProfileUrl = m.ProfileUrl,
                IsFollowing = followedUsers.Contains(m.Id),
            }).FirstOrDefault();
        }
    }
}
