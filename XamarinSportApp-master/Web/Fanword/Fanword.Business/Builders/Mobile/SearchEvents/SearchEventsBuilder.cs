using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.Entities;
using Fanword.Data.Repository;
using Fanword.Poco.Models;
using Profile = Fanword.Poco.Models.Profile;
using AutoMapper.QueryableExtensions;

namespace Fanword.Business.Builders.Mobile.SearchEvents
{
    public class SearchEventsBuilder : BaseBuilder
    {
        MapperConfiguration MapObjects()
        {
            MapperConfiguration configuration = new MapperConfiguration(config =>

                config.CreateMap<Event, Poco.Models.EventProfile>()
                    .ForMember(m => m.SportName, m => m.MapFrom(mm => mm.Sport.Name))
                    .ForMember(m => m.Team1Name, m => m.MapFrom(mm => mm.EventTeams.OrderBy(j => j.Id).Select(j => j.Team.School.Name).FirstOrDefault()))
                    .ForMember(m => m.Team2Name, m => m.MapFrom(mm => mm.EventTeams.OrderByDescending(j => j.Id).Select(j => j.Team.School.Name).FirstOrDefault()))
                    .ForMember(m => m.Team1Url, m => m.MapFrom(mm => mm.EventTeams.OrderBy(j => j.Id).Select(j => j.Team.ProfilePublicUrl).FirstOrDefault()))
                    .ForMember(m => m.Team2Url, m => m.MapFrom(mm => mm.EventTeams.OrderByDescending(j => j.Id).Select(j => j.Team.ProfilePublicUrl).FirstOrDefault()))
                    .ForMember(m => m.TeamCount, m => m.MapFrom(mm => mm.EventTeams.Count))
                    .ForMember(m => m.Posts, m => m.MapFrom(mm => mm.Posts.Count(j => j.DateDeletedUtc == null)))
            );

            return configuration;
        }
        public SearchEventsBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        public async Task<EventSearch> SearchAsync(DateTime date, string userId)
        {
            var result = new EventSearch();
            result.SearchTime = date;
            var datePlus24Hours = date.AddHours(24);

            result.Events = await _repo.Events.Where(m => m.DateOfEventUtc > date && m.DateOfEventUtc <= datePlus24Hours).ProjectTo<Poco.Models.EventProfile>(MapObjects()).ToListAsync();

            return result;
        }
    }
}
