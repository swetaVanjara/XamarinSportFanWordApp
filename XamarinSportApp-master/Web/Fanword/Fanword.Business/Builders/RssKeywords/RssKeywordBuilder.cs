using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExtensionClasses;
using Fanword.Business.ViewModels.RssFeeds;
using Fanword.Data.Entities;
using Fanword.Data.Repository;
using Fanword.Business.ViewModels.RssKeywords;

namespace Fanword.Business.Builders.RssKeywords
{
	public class RssKeywordBuilder
	{
		private ApplicationRepository _repo { get; set; }

		public RssKeywordBuilder(ApplicationRepository repo)
		{
			_repo = repo ?? ApplicationRepository.CreateWithoutOwin();
		}

		private MapperConfiguration MapGridQueries()
		{
			return new MapperConfiguration(config =>
			{
				config.CreateMap<RssKeyword, RssKeywordViewModel>()
					.ForMember(m => m.IsActive, m => m.UseValue(true))
					.ForMember(m => m.IsActive, m => m.Ignore());
			});
		}

		private MapperConfiguration MapQueries()
		{
			return new MapperConfiguration(config => 
			{
				config.CreateMap<RssKeyword, RssKeywordViewModel>();
			});
		}

		private MapperConfiguration MapAdd()
		{
			return new MapperConfiguration(config => {
				config.CreateMap<RssKeywordViewModel, RssKeyword>()
					.ForMember(m => m.IsActive, m => m.MapFrom(mm => true))
					.ForMember(m => m.Id, m => m.MapFrom(mm=> Guid.NewGuid().ToString()));
			});
		}
		private MapperConfiguration MapUpdate()
		{
			return new MapperConfiguration(config => {
				config.CreateMap<RssFeedViewModel, RssFeed>()
					.ForMember(m => m.Id, m => m.Ignore())
					.ForMember(m => m.IsActive, m => m.Ignore());
			});
		}
		public async Task<List<RssKeywordViewModel>> BuildGridAsync(string id, bool showInactive)
		{
			var query = await _repo.RssKeywords.Where(m => m.RssFeedId == id).ProjectTo<RssKeywordViewModel>(MapGridQueries()).ToListAsync();
			var result =  (query).SpecifyDateTimeKind();

			return (result);
			//return new List<RssKeywordViewModel>();
		}

		public async Task AddAsync(RssKeywordViewModel model)
		{
			var dbItem = MapAdd().CreateMapper().Map<RssKeyword>(model);
			await _repo.RssKeywords.AddOrUpdateAndSaveAsync(dbItem);
		}

		public async Task UpdateAsync(RssKeywordViewModel model)
		{
			var dbItem = await _repo.RssKeywords.GetByIdAsync(model.Id);
			dbItem = MapUpdate().CreateMapper().Map(model, dbItem);
			await _repo.SaveAsync();
		}

		public async Task Delete(RssKeywordViewModel model)
		{
			await _repo.RssKeywords.DeleteAndSaveAsync(model.Id);
		}
		public async Task<RssKeywordViewModel> BuildSingleAsync(string id)
		{
			return (await _repo.RssKeywords.GetByIdQueryable(id).ProjectTo<RssKeywordViewModel>(MapQueries()).FirstOrDefaultAsync())?.SpecifyDateTimeKind();
		}
	}
}
