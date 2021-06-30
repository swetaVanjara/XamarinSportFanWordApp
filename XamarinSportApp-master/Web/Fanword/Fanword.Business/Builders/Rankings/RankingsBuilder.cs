using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExtensionClasses;
using Fanword.Business.ViewModels.Rankings;
using Fanword.Data.Entities;
using Fanword.Data.Repository;

namespace Fanword.Business.Builders.Rankings {
    public class RankingsBuilder {
        private ApplicationRepository _repo { get; set; }

        public RankingsBuilder(ApplicationRepository repo) {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        private MapperConfiguration MapQueries() {
            return new MapperConfiguration(config => {
                config.CreateMap<Ranking, RankingViewModel>()
                    .ForMember(m => m.RankingTeams, m => m.MapFrom(mm => mm.RankingTeams.OrderBy(i => i.RankingNumber)));
                config.CreateMap<RankingTeam, RankingTeamViewModel>();
            });
        }

        public async Task<RankingViewModel> BuildSingleAsync(string sportId) {
            await GenerateDefaultRankingRecordsAsync(sportId);
            return (await _repo.Rankings.Where(m => m.SportId == sportId).ProjectTo<RankingViewModel>(MapQueries()).FirstOrDefaultAsync())?.SpecifyDateTimeKind();
        }


        private async Task GenerateDefaultRankingRecordsAsync(string sportId) {
            if (!await _repo.Rankings.Where(m => m.SportId == sportId).AnyAsync()) {
                var id = Guid.NewGuid().ToString();
                var dbRanking = new Ranking() {
                    Id = id,
                    DateCreatedUtc = DateTime.UtcNow,
					DateModifiedUtc = DateTime.UtcNow,
                    SportId = sportId,
                    RankingTeams = new List<RankingTeam>(),
                };

                for (var i = 1; i <= 25; i++) {
                    dbRanking.RankingTeams.Add(new RankingTeam() {
                        DateCreatedUtc = DateTime.UtcNow,
                        Id = Guid.NewGuid().ToString(),
                        RankingId = id,
                        RankingNumber = i,
                    });
                }
                var ranking = await _repo.Rankings.AddOrUpdateAndSaveAsync(dbRanking);
            }
        }


        public async Task UpdateAsync(RankingViewModel model) {
            var dbRankings = await _repo.RankingTeams.Where(m => m.RankingId == model.Id).ToListAsync();
			var overallRanking = await _repo.Rankings.Where(m => m.Id == model.Id).FirstOrDefaultAsync();
            foreach (var ranking in dbRankings) {
                var thisRanking = model.RankingTeams.FirstOrDefault(m => m.Id == ranking.Id);
                if(thisRanking == null) continue;
                ranking.TeamId = thisRanking.TeamId;
            }
			overallRanking.DateModifiedUtc = DateTime.UtcNow;

			await _repo.SaveAsync();
        }
    }
}
