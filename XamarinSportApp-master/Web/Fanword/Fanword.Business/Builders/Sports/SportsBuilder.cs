using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExtensionClasses;
using Fanword.Business.Builders.Teams;
using Fanword.Business.ViewModels.Sports;
using Fanword.Data.Entities;
using Fanword.Data.Repository;
using FileUploads.Azure;
using Fanword.Data.Enums;

namespace Fanword.Business.Builders.Sports {
    public class SportsBuilder {
        private ApplicationRepository _repo { get; set; }

        public SportsBuilder(ApplicationRepository repo) {
            _repo = repo;
        }

        private MapperConfiguration MapGridQueries() {
            return new MapperConfiguration(config => {
                config.CreateMap<Sport, SportRecord>();
            });
        }

        private MapperConfiguration MapQueries() {
            return new MapperConfiguration(config => {
                config.CreateMap<Sport, SportViewModel>();
            });
        }

        private MapperConfiguration MapAddUpdate(bool isUpdate) {
            return new MapperConfiguration(config => {
                if (isUpdate) {
                    config.CreateMap<SportViewModel, Sport>()
                        .ForMember(m => m.Id, m => m.Ignore());
                }
                else {
                    config.CreateMap<SportViewModel, Sport>()
                        .ForMember(m => m.Id, m => m.MapFrom(mm=>Guid.NewGuid().ToString()));
                }
            });
        }

        public async Task<List<SportRecord>> BuildGridAsync(bool showInactive) {
			if (showInactive)
			{
				return (await _repo.Sports.Where(m => !m.IsActive && m.DateDeletedUtc == null).AsQueryable().ProjectTo<SportRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
			} else {
				return (await _repo.Sports.Where(m => m.IsActive && m.DateDeletedUtc == null).AsQueryable().ProjectTo<SportRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
			}
        }

        public async Task<SportViewModel> BuildSingleAsync(string id) {
            return (await _repo.Sports.GetByIdQueryable(id).ProjectTo<SportViewModel>(MapQueries()).FirstOrDefaultAsync())?.SpecifyDateTimeKind();
        }


        public async Task AddAsync(SportViewModel model) {
            await _repo.Sports.AddOrUpdateAndSaveAsync(MapAddUpdate(false).CreateMapper().Map<Sport>(model));
        }

        public async Task UpdateAsync(SportViewModel model) {
            var dbSport = await _repo.Sports.GetByIdAsync(model.Id);
            if (model.IconBlobName != dbSport.IconBlobName) {
                //delete current one
                new AzureStorage(dbSport.IconContainer, true).DeleteFile(dbSport.IconBlobName);
            }
            if (model.IsActive == false)
            {
                var teams = await _repo.Teams.Where(m => m.SportId == model.Id && m.DateDeletedUtc == null).ToListAsync();
                foreach (var team in teams)
                {
                    team.IsActive = false;
                }
                var feeds = await _repo.RssFeeds.Where(m => m.SportId == model.Id && m.DateDeletedUtc == null).ToListAsync();
                foreach (var feed in feeds)
                {
                    feed.IsActive = false;
                }
            }
            
            await _repo.SaveAsync();
            await _repo.Sports.AddOrUpdateAndSaveAsync(MapAddUpdate(true).CreateMapper().Map(model, dbSport));
        }

		public async Task DeleteAsync(string id)
		{
			await _repo.Sports.DeleteAndSaveAsync(id);
			var feeds = await _repo.RssFeeds.Where(m => m.SportId == id).ToListAsync();
			foreach (var feed in feeds)
			{
				feed.DateDeletedUtc = DateTime.UtcNow;
				feed.RssFeedStatus = RssFeedStatus.Denied;
			}
			await _repo.SaveAsync();

            var posts = await  _repo.Posts.Where(m => m.Feed.SportId == id || m.Sports.Any(i => i.Id == id)).Include(i => i.Schools).Include(mm => mm.Sports).Include(ii => ii.Feed).ToListAsync();
            foreach (var post in posts)
            {
                if (post.Feed?.SportId == id)
                {
                    post.DateDeletedUtc = DateTime.UtcNow;
                }
                var sportTag = post.Sports.FirstOrDefault(m => m.Id == id);
                post.Sports.Remove(sportTag);
            }
            await _repo.SaveAsync();
            var teamIds = await _repo.Teams.Where(m => m.SportId == id).Select(m => m.Id).ToListAsync();
			foreach (var teamId in teamIds)
			{
			    await new TeamsBuilder(_repo).DeleteAsync(teamId);
			}
			await _repo.SaveAsync();
		}
    }
}
