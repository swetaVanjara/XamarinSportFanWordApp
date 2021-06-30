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
using Fanword.Poco.Models;

namespace Fanword.Business.Builders.Mobile.EventProfile
{
    public class EventProfileBuilder : BaseBuilder
    {
        public EventProfileBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        public async Task<Poco.Models.EventProfile> BuildAsync(string userId, string eventId)
        {
            //var followedSchools = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Schools.Select(j => j.Id));
            MapperConfiguration config = new MapperConfiguration(k =>
                k.CreateMap<Data.Entities.Event, Poco.Models.EventProfile>()
                    .ForMember(m => m.Posts, m => m.MapFrom(mm => mm.Posts.Count(j => j.DateDeletedUtc == null)))
                    .ForMember(m => m.Team1Name, m => m.MapFrom(mm => mm.EventTeams.OrderBy(j => j.DateCreatedUtc).Select(j => j.Team.School.Name).FirstOrDefault()))
                    .ForMember(m => m.Team2Name, m => m.MapFrom(mm => mm.EventTeams.OrderByDescending(j => j.DateCreatedUtc).Select(j => j.Team.School.Name).FirstOrDefault()))
                    .ForMember(m => m.Team1Url, m => m.MapFrom(mm => mm.EventTeams.OrderBy(j => j.DateCreatedUtc).Select(j => j.Team.ProfilePublicUrl).FirstOrDefault()))
                    .ForMember(m => m.Team2Url, m => m.MapFrom(mm => mm.EventTeams.OrderByDescending(j => j.DateCreatedUtc).Select(j => j.Team.ProfilePublicUrl).FirstOrDefault()))
                    .ForMember(m => m.Team1Score, m => m.MapFrom(mm => mm.EventTeams.OrderBy(j => j.DateCreatedUtc).Select(j => j.ScorePointsOrPlace).FirstOrDefault()))
                    .ForMember(m => m.Team2Score, m => m.MapFrom(mm => mm.EventTeams.OrderByDescending(j => j.DateCreatedUtc).Select(j => j.ScorePointsOrPlace).FirstOrDefault()))
                    .ForMember(m => m.TicketUrl, m => m.MapFrom(mm => mm.PurchaseTicketsUrl))
                    .ForMember(m => m.WinningTeamUrl, m => m.MapFrom(mm => mm.EventTeams.Where(j => j.WinLossTie == WinLossTieEnum.Win).Select(j => j.Team.ProfilePublicUrl).FirstOrDefault()))
                    .ForMember(m => m.TeamCount, m => m.MapFrom(mm => mm.EventTeams.Count()))
                    .ForMember(m => m.Posts, m => m.MapFrom(mm => mm.Posts.Count(j => j.DateDeletedUtc == null))));

            var eventModel = await _repo.Events.Where(m => m.Id == eventId).ProjectTo<Poco.Models.EventProfile>(config).FirstOrDefaultAsync();

            // This is happening here because apparently C# running on mobile doesn't use the same Ids for time zoens as normal C#
            var date = TimeZoneInfo.ConvertTimeFromUtc(eventModel.DateOfEventUtc, TimeZoneInfo.FindSystemTimeZoneById(eventModel.TimezoneId));
            eventModel.IsTbd = date.Hour == 0;
            return eventModel;
        }
    }
}
