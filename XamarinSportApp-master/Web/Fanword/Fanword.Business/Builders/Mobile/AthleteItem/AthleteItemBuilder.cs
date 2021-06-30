using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.IdentityConfig.User;
using Fanword.Data.Repository;
using Fanword.Poco.Models;

namespace Fanword.Business.Builders.Mobile.AthleteItem
{
    public class AthleteItemBuilder : BaseBuilder
    {
        public AthleteItemBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        public async Task<List<Poco.Models.AthleteItem>> BuildBySportAsync(string userId, string sportId)
        {
            var followedUsers = FollowedUsers(userId);
            //return await Select(_repo.Users.Where(m => m.Atheletes.Any(j => j.Team.SportId == sportId)), followedUsers);
            return await Select(_repo.Users.Where(m => m.Atheletes.Any(j => j.Verified && j.Team.SportId == sportId)), followedUsers);
        }

        public async Task<List<Poco.Models.AthleteItem>> BuildBySchoolAsync(string userId, string schoolId)
        {
            var followedUsers = FollowedUsers(userId);
            return await Select(_repo.Users.Where(m => m.Atheletes.Any(j => j.Verified && j.Team.SchoolId == schoolId)), followedUsers);
        }
        public async Task<List<Poco.Models.AthleteItem>> BuildByTeamAsync(string userId, string teamId)
        {
            var followedUsers = FollowedUsers(userId);
            return await Select(_repo.Users.Where(m => m.Atheletes.Any(j => j.Verified && j.TeamId == teamId)), followedUsers);
        }

        IQueryable<string> FollowedUsers(string userId)
        {
            return _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Users.Select(j => j.Id));
        }

        async Task<List<Poco.Models.AthleteItem>> Select(IQueryable<ApplicationUser> query, IQueryable<string> followedUsers)
        {
            return await query.Select(m => new Poco.Models.AthleteItem()
            {
                Id = m.Id,
                ProfileUrl = m.ProfileUrl,
                IsFollowing = followedUsers.Contains(m.Id),
                SchoolName = m.Atheletes.OrderByDescending(j => j.StartUtc).Select(j => j.Team.School.Name).FirstOrDefault(),
                SportName = m.Atheletes.OrderByDescending(j => j.StartUtc).Select(j => j.Team.Sport.Name).FirstOrDefault(),
                Name = m.FirstName + " " + m.LastName,
            }).ToListAsync();
        }
    }
}
