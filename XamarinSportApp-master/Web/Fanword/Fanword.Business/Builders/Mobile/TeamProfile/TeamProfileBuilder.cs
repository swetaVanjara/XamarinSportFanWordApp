using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.Entities;
using Fanword.Data.Enums;
using Fanword.Data.Repository;
using Fanword.Poco.Models;

namespace Fanword.Business.Builders.Mobile.TeamProfile
{
    public class TeamProfileBuilder : BaseBuilder
    {
        public TeamProfileBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }


        public async Task<Poco.Models.TeamProfile> BuildAsync(string userId, string teamId)
        {
            var followedTeams = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Teams.Select(j => j.Id));
            MapperConfiguration config = new MapperConfiguration(k =>
                k.CreateMap<Data.Entities.Team, Poco.Models.TeamProfile>()
                    .ForMember(m => m.Followers, m => m.MapFrom(mm => mm.Followers.Count))
                    .ForMember(m => m.ProfileUrl, m => m.MapFrom(mm => mm.ProfilePublicUrl))
                    .ForMember(m => m.SportName, m => m.MapFrom(mm => mm.Sport.Name))
                    .ForMember(m => m.SchoolName, m => m.MapFrom(mm => mm.School.Name))
                    .ForMember(m => m.Posts, m => m.MapFrom(mm => mm.PostsByTeam.Count(j => j.DateDeletedUtc == null)))
                    .ForMember(m => m.IsFollowing, m => m.MapFrom(mm => followedTeams.Contains(mm.Id)))
                    .ForMember(m => m.IsProfileAdmin, m => m.MapFrom(mm => mm.Admins.Any(j => j.UserId == userId && j.AdminStatus == AdminStatus.Approved)))
                    .ForMember(m => m.Ranking, m => m.MapFrom(mm => mm.Sport.Rankings.SelectMany(j => j.RankingTeams).FirstOrDefault(j => j.TeamId == mm.Id).RankingNumber))
                    .ForMember(m => m.Wins, m => m.MapFrom(mm => mm.EventTeams.Count(j => j.WinLossTie == WinLossTieEnum.Win)))
                    .ForMember(m => m.Ties, m => m.MapFrom(mm => mm.EventTeams.Count(j => j.WinLossTie == WinLossTieEnum.Tie)))
                    .ForMember(m => m.Loss, m => m.MapFrom(mm => mm.EventTeams.Count(j => j.WinLossTie == WinLossTieEnum.Loss))));

            return await _repo.Teams.Where(m => m.Id == teamId).ProjectTo<Poco.Models.TeamProfile>(config).FirstOrDefaultAsync();
        }

        //public async Task<List<Poco.Models.TeamProfile>> BuildBySportAsync(string userId, string sportId)
        //{
        //    var followedTeams = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Teams.Select(j => j.Id));
        //    return await Select(_repo.Teams.Where(m => m.SportId == sportId), followedTeams);
        //}

        public async Task<List<Poco.Models.TeamProfile>> BuildBySportAsync(string userId, string sportId)
        {
            var followedTeams = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Teams.Select(j => j.Id));
            return await Select(_repo.Teams.Where(m => m.SportId == sportId && m.IsActive), followedTeams);
        }

        public async Task<List<Poco.Models.TeamProfile>> BuildBySchoolAsync(string userId, string schoolId)
        {
            var followedTeams = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Teams.Select(j => j.Id));
            return await Select(_repo.Teams.Where(m => m.SchoolId == schoolId), followedTeams);
        }

        async Task<List<Poco.Models.TeamProfile>> Select(IQueryable<Team> query, IQueryable<string> followedTeams)
        {
            return await query.Select(m => new Poco.Models.TeamProfile()
            {
                Id = m.Id,
                SchoolName = m.School.Name,
                SportName = m.Sport.Name,
                ProfileUrl = m.ProfilePublicUrl,
                IsFollowing = followedTeams.Contains(m.Id),
            }).ToListAsync();
        }

        public async Task RequestAdminAsync(string userId, string teamId)
        {
            var existingAdmin = await _repo.TeamAdmins.FirstOrDefaultAsync(m => m.UserId == userId && m.TeamId == teamId);
            if (existingAdmin == null)
            {
                existingAdmin = new TeamAdmin() { UserId = userId, TeamId = teamId, AdminStatus = AdminStatus.Pending};
                await _repo.TeamAdmins.AddOrUpdateAndSaveAsync(existingAdmin);
            }
        }
    }
}
