using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.Enums;
using Fanword.Data.Repository;

namespace Fanword.Business.Builders.Mobile.SportProfile
{
    public class SportProfileBuilder : BaseBuilder
    {
        public SportProfileBuilder(ApplicationRepository repo)
        {
            _repo = repo;
        }
        public async Task<Poco.Models.SportProfile> BuildAsync(string userId, string sportId)
        {
            var followedSports = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Sports.Select(j => j.Id));
            MapperConfiguration config = new MapperConfiguration(k =>
                k.CreateMap<Data.Entities.Sport, Poco.Models.SportProfile>()
                    .ForMember(m => m.Followers, m => m.MapFrom(mm => mm.Followers.Count))
                    .ForMember(m => m.Posts, m => m.MapFrom(mm => mm.Posts.Count(j => j.DateDeletedUtc == null)))
                    .ForMember(m => m.IsFollowing, m => m.MapFrom(mm => followedSports.Contains(mm.Id))));

            return await _repo.Sports.Where(m => m.Id == sportId).ProjectTo<Poco.Models.SportProfile>(config).FirstOrDefaultAsync();
        }
    }
}
