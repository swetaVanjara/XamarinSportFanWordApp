using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExtensionClasses.AutoMapper.Mappers;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.Entities;
using Fanword.Data.Repository;
using Fanword.Poco.Models;
using Profile = Fanword.Poco.Models.Profile;

namespace Fanword.Business.Builders.Mobile.SearchProfiles
{
    public class SearchProfileBuilder : BaseBuilder
    {

        MapperConfiguration MapObjects()
        {
            MapperConfiguration configuration = new MapperConfiguration(config =>
            
                config.CreateMap<Team, Profile>()
                .ForMember(m => m.Name, m => m.MapFrom(mm => mm.School.Name))
                .ForMember(m => m.SubTitle, m => m.MapFrom(mm => mm.Sport.Name))
            );

            return configuration;
        }

        MapperConfiguration MapSports()
        {
            MapperConfiguration configuration = new MapperConfiguration(config =>

                config.CreateMap<Sport, Profile>()
                    .ForMember(m => m.ProfilePublicUrl, m => m.MapFrom(mm => mm.IconPublicUrl))
            );

            return configuration;
        }

        public SearchProfileBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        public async Task<ProfileSearch> SearchAsync(string text, string userId, string id, FeedType type)
        {
            var result = new ProfileSearch();
            result.SearchText = text ?? "";
            // At the moment this is only getting called for people posting as an admin or a content source so we are showing all profiles
            if (string.IsNullOrEmpty(text))
            {
                if (type == FeedType.School)
                {
                    result.SchoolProfiles = await _repo.Schools.Where(m => m.IsActive && m.Id == id).Map<Profile>().ToListAsync();
                    result.SportProfile = new List<Profile>();
                    result.TeamProfiles = await _repo.Teams.Where(m => m.IsActive && m.SchoolId == id).ProjectTo<Profile>(MapObjects()).ToListAsync();
                }
                else if (type == FeedType.User)
                {
                    var teamId = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Atheletes).FirstOrDefault().TeamId;
                    result.SchoolProfiles = new List<Profile>();
                    result.SportProfile = new List<Profile>();
                    result.TeamProfiles = await _repo.Teams.Where(m => m.IsActive && m.Id == teamId).ProjectTo<Profile>(MapObjects()).ToListAsync();
                }
                else if(type == FeedType.ContentSource)
                {
                    result.SchoolProfiles = await _repo.Schools.Where(m => m.IsActive).Map<Profile>().ToListAsync();
                    result.SportProfile = await _repo.Sports.Where(m => m.IsActive).ProjectTo<Profile>(MapSports()).ToListAsync();
                    result.TeamProfiles = await _repo.Teams.Where(m => m.IsActive && m.School.DateDeletedUtc == null && m.Sport.DateDeletedUtc == null).ProjectTo<Profile>(MapObjects()).ToListAsync();
                }
            }
            else
            {
                var lower = text.ToLower();
                if (type == FeedType.School)
                {
                    result.SchoolProfiles = await _repo.Schools.Where(m => m.IsActive && m.Name.Contains(lower) && m.Id == id).Map<Profile>().ToListAsync();
                    result.SportProfile = new List<Profile>();
                    result.TeamProfiles = await _repo.Teams.Where(m => m.IsActive && (m.School.Name + " " + m.Sport.Name).Contains(lower) && m.SchoolId == id).ProjectTo<Profile>(MapObjects()).ToListAsync();
                }
                else if (type == FeedType.User)
                {
                    var teamId = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Atheletes).FirstOrDefault().TeamId;
                    result.SchoolProfiles = new List<Profile>();
                    result.SportProfile = new List<Profile>();
                    result.TeamProfiles = await _repo.Teams.Where(m => m.IsActive && (m.School.Name + " " + m.Sport.Name).Contains(lower) && m.Id == teamId).ProjectTo<Profile>(MapObjects()).ToListAsync();
                }
                else if (type == FeedType.ContentSource)
                {
                    result.SchoolProfiles = await _repo.Schools.Where(m => m.IsActive && m.Name.Contains(lower)).Map<Profile>().ToListAsync();
                    result.SportProfile = await _repo.Sports.Where(m => m.IsActive && m.Name.Contains(lower)).ProjectTo<Profile>(MapSports()).ToListAsync();
                    result.TeamProfiles = await _repo.Teams.Where(m => m.IsActive && m.School.DateDeletedUtc == null && m.Sport.DateDeletedUtc == null && (m.School.Name + " " + m.Sport.Name).Contains(lower)).ProjectTo<Profile>(MapObjects()).ToListAsync();
                }
            }
            return result;
        }
    }
}
