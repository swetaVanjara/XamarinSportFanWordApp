using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.Enums;
using Fanword.Data.Repository;
using Fanword.Poco.Models;
namespace Fanword.Business.Builders.Mobile.Rankings
{
    public class RankingsBuilder : BaseBuilder
    {
        public RankingsBuilder(ApplicationRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Ranking>> BuildAsync(string userId, FollowingFilterModel filter)
        {
            var sportFollows = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Sports).Select(m => m.Id);
            var schoolFollows = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Schools).Select(m => m.Id);
            var teamFollows = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Teams).Select(m => m.Id);

            var query = _repo.Rankings.Where(m => m.Sport.DateDeletedUtc == null).OrderByDescending(m => m.DateCreatedUtc).SelectMany(m => m.RankingTeams);
            query = query.Where(m => m.Team.School.DateDeletedUtc == null &&  !string.IsNullOrEmpty(m.TeamId) && m.Team.DateDeletedUtc == null);

            if (filter.MySchools || filter.MySports || filter.MyTeams)
            {
                query = query.Where(m =>
                    (filter.MySports && sportFollows.Contains(m.Ranking.SportId)) ||
                    (filter.MyTeams && teamFollows.Contains(m.TeamId)) ||
                    (filter.MySchools && schoolFollows.Contains(m.Team.SchoolId)));
            }

            if (!string.IsNullOrEmpty(filter.SportId))
            {
                query = query.Where(m => m.Team.SportId == filter.SportId);
            }
            if (!string.IsNullOrEmpty(filter.SchoolId))
            {
                query = query.Where(m => m.Team.SchoolId == filter.SchoolId);
            }

            return await SelectRankingsAsync(query, teamFollows);
        }

        //async Task<List<Ranking>> SelectRankingsAsync(IQueryable<Data.Entities.RankingTeam> query, IQueryable<string> teamFollows)
        //{
        //    var rankings = await query.Select(m => new Ranking()
        //    {
        //        Id = m.Id,
        //        SportName = m.Ranking.Sport.Name,
        //        TeamName = m.Team.School.Name,
        //        ProfileUrl = m.Team.ProfilePublicUrl,
        //        DateUpdatedUtc = m.Ranking.DateModifiedUtc,
        //        Rank = m.RankingNumber,
        //        Loses = m.Team.EventTeams.Count(j => j.WinLossTie == WinLossTieEnum.Loss),
        //        Wins = m.Team.EventTeams.Count(j => j.WinLossTie == WinLossTieEnum.Win),
        //        Ties =m.Team.EventTeams.Count(j => j.WinLossTie == WinLossTieEnum.Tie),
        //        IsFollowing = teamFollows.Contains(m.TeamId),
        //        TeamId = m.TeamId,
        //        SportId = m.Team.SportId,
        //    }).OrderByDescending(m => m.DateUpdatedUtc).ThenBy(m => m.Rank).ToListAsync();
        //    return rankings;
        //}

        async Task<List<Ranking>> SelectRankingsAsync(IQueryable<Data.Entities.RankingTeam> query, IQueryable<string> teamFollows)
        {
            var rankings = await query.Select(m => new Ranking()
            {
                Id = m.Id,
                SportName = m.Ranking.Sport.Name,
                TeamName = m.Team.School.Name,
                ProfileUrl = m.Team.ProfilePublicUrl,
                DateUpdatedUtc = m.Ranking.DateModifiedUtc,
                Rank = m.RankingNumber,
                Loses = m.Team.EventTeams.Count(j => j.WinLossTie == WinLossTieEnum.Loss),
                Wins = m.Team.EventTeams.Count(j => j.WinLossTie == WinLossTieEnum.Win),
                Ties = m.Team.EventTeams.Count(j => j.WinLossTie == WinLossTieEnum.Tie),
                IsFollowing = teamFollows.Contains(m.TeamId),
                TeamId = m.TeamId,
                SportId = m.Team.SportId,
                IsActive = m.Team.IsActive


            }).OrderByDescending(m => m.DateUpdatedUtc).ThenBy(m => m.Rank).ToListAsync();
            return rankings;
        }

    }
}
