using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.IdentityConfig.User;
using Fanword.Data.Repository;
using Fanword.Poco.Models;

namespace Fanword.Business.Builders.Mobile.Followers
{
    public class FollowersBuilder : BaseBuilder
    {
        public FollowersBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        MapperConfiguration MapObjects(string userId)
        {
            var followedUsers = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Users.Select(j => j.Id));
            MapperConfiguration config = new MapperConfiguration(j =>
                j.CreateMap<ApplicationUser, Follower>()
                    .ForMember(m => m.IsFollowing, m => m.MapFrom(mm => followedUsers.Contains(mm.Id))));
            return config;
        }

        public async Task<List<Follower>> BuildForUserAsync(string userId, string forUserId)
        {
            return await _repo.Users.Where(m => m.Id == forUserId).SelectMany(m => m.Followers).Where(m => m.IsActive).ProjectTo<Follower>(MapObjects(userId)).ToListAsync();
        }

        public async Task<List<Follower>> BuildForTeamAsync(string userId, string teamId)
        {
            return await _repo.Teams.Where(m => m.Id == teamId).SelectMany(m => m.Followers).Where(m => m.IsActive).ProjectTo<Follower>(MapObjects(userId)).ToListAsync();
        }

        public async Task<List<Follower>> BuildForSchoolAsync(string userId, string schoolId)
        {
            return await _repo.Schools.Where(m => m.Id == schoolId).SelectMany(m => m.Followers).Where(m => m.IsActive).ProjectTo<Follower>(MapObjects(userId)).ToListAsync();
        }

        public async Task<List<Follower>> BuildForSportAsync(string userId, string sportId)
        {
            return await _repo.Sports.Where(m => m.Id == sportId).SelectMany(m => m.Followers).Where(m => m.IsActive).ProjectTo<Follower>(MapObjects(userId)).ToListAsync();
        }

        public async Task<List<Follower>> BuildForContentSourceAsync(string userId, string contentSourceId)
        {
            return await _repo.ContentSources.Where(m => m.Id == contentSourceId).SelectMany(m => m.Followers).Where(m => m.IsActive).ProjectTo<Follower>(MapObjects(userId)).ToListAsync();
        }
    }
}
