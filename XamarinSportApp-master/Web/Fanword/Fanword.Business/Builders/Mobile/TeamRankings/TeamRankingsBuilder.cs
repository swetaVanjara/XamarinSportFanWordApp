using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.Repository;
using Fanword.Poco.Models;

namespace Fanword.Business.Builders.Mobile.TeamRankings
{
    public class TeamRankingsBuilder : BaseBuilder
    {
        public TeamRankingsBuilder(ApplicationRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<TeamRanking>> BuildAsync(string userId, string teamId)
        {
            var query = _repo.Rankings.AsQueryable().OrderByDescending(m => m.DateCreatedUtc).SelectMany(m => m.RankingTeams);
            query = query.Where(m => !string.IsNullOrEmpty(m.TeamId) && m.TeamId == teamId && m.Team.DateDeletedUtc == null);

            return await query.Select(m => new TeamRanking() { DateUpdatedUtc = m.Ranking.DateModifiedUtc, Rank = m.RankingNumber }).OrderByDescending(m => m.DateUpdatedUtc).ToListAsync();
        }
    }   
}
