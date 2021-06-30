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

namespace Fanword.Business.Builders.Mobile.EventTeams
{
    public class EventTeamsBuilder : BaseBuilder
    {
        public EventTeamsBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        public async Task<List<Poco.Models.EventTeam>> BuildAsync(string eventId)
        {
            MapperConfiguration config = new MapperConfiguration(k =>
                k.CreateMap<Data.Entities.EventTeam, Poco.Models.EventTeam>()
                    .ForMember(m => m.TeamId, m => m.MapFrom(mm => mm.TeamId))
                    .ForMember(m => m.SchoolName, m => m.MapFrom(mm => mm.Team.School.Name))
                    .ForMember(m => m.SportName, m => m.MapFrom(mm => mm.Team.Sport.Name))
                    .ForMember(m => m.ProfileUrl, m => m.MapFrom(mm => mm.Team.ProfilePublicUrl))
                    .ForMember(m => m.Score, m => m.MapFrom(mm => mm.ScorePointsOrPlace)));

            return await _repo.Events.Where(m => m.Id == eventId).SelectMany(m => m.EventTeams).ProjectTo<Poco.Models.EventTeam>(config).ToListAsync();
        }
    }
}
