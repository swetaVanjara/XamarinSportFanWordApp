using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExtensionClasses;
using Fanword.Business.ViewModels.RssKeywordTypes;
using Fanword.Data.Entities;
using Fanword.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.Builders.RssKeywordTypes
{
	public class RssKeywordTypeBuilder
	{
		private ApplicationRepository _repo { get; set; }

		public RssKeywordTypeBuilder(ApplicationRepository repo)
		{
			_repo = repo ?? ApplicationRepository.CreateWithoutOwin();
		}

		private MapperConfiguration MapGridQueries()
		{
			return new MapperConfiguration(config => {
				config.CreateMap<RssKeywordType, RssKeywordTypeViewModel>();
			});
		}
		private MapperConfiguration MapQueries()
		{
			return new MapperConfiguration(config => {
				config.CreateMap<RssKeywordType, RssKeywordTypeViewModel>();
			});
		}
		private MapperConfiguration MapAdd()
		{
			return new MapperConfiguration(config => {
				config.CreateMap<RssKeywordTypeViewModel, RssKeywordType>()
					.ForMember(m => m.IsActive, m => m.UseValue(true))
					.ForMember(m => m.Id, m => m.MapFrom(mm => Guid.NewGuid().ToString()))
					.ForMember(m => m.IsActive, m => m.Ignore());
			});
		}

		private MapperConfiguration MapUpdate()
		{
			return new MapperConfiguration(config => {
				config.CreateMap<RssKeywordTypeViewModel, RssKeywordType>()
					.ForMember(m => m.Id, m => m.Ignore());
			});
		}


		public async Task<List<RssKeywordTypeViewModel>> BuildGridAsync(bool showInactive)
		{
			var result = (await _repo.RssKeywordTypes.Where(m => m.IsActive).ProjectTo<RssKeywordTypeViewModel>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
			return (result);
		}

		public async Task AddAsync(RssKeywordTypeViewModel model)
		{
			var dbItem = MapAdd().CreateMapper().Map<RssKeywordType>(model);
			await _repo.RssKeywordTypes.AddOrUpdateAndSaveAsync(dbItem);
		}

		public async Task UpdateAsync(RssKeywordTypeViewModel model)
		{
			var dbItem = await _repo.RssFeeds.GetByIdAsync(model.Id);
			dbItem = MapUpdate().CreateMapper().Map(model, dbItem);
			await _repo.SaveAsync();
		}

		public async Task<RssKeywordTypeViewModel> BuildSingleAsync(string id)
		{
			return (await _repo.RssFeeds.GetByIdQueryable(id).ProjectTo<RssKeywordTypeViewModel>(MapQueries()).FirstOrDefaultAsync())?.SpecifyDateTimeKind();
		}
	}
}
