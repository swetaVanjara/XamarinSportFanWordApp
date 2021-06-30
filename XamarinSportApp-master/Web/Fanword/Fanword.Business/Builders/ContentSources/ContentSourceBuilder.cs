using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using AutoMapper.QueryableExtensions;
using ExtensionClasses;
using Fanword.Business.ViewModels.Advertisers;
using Fanword.Business.ViewModels.Registration;
using Fanword.Data.Entities;
using Fanword.Data.Enums;
using Fanword.Data.Repository;
using Fanword.Business.ViewModels.ContentSources;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace Fanword.Business.Builders.ContentSources
{
	public class ContentSourceBuilder
	{
		private ApplicationRepository _repo { get; set; }

		public ContentSourceBuilder(ApplicationRepository repo)
		{
			_repo = repo ?? ApplicationRepository.CreateWithoutOwin();
		}

		private MapperConfiguration MapGridQueries()
		{
			return new MapperConfiguration(config => {
				config.CreateMap<ContentSource, ContentSourceRecord>()
				.ForMember(m => m.NumberPendingRssFeeds, m => m.MapFrom(mm => mm.RssFeeds.Count(i => i.RssFeedStatus == RssFeedStatus.Pending && i.DateDeletedUtc == null)));
			});
		}

		private MapperConfiguration MapAdd()
		{
			return new MapperConfiguration(config => {
				config.CreateMap<ContentSourceRegistrationViewModel, ContentSource>()
				.ForMember(m => m.ContactName, m => m.MapFrom(mm => mm.FirstName + " " + mm.LastName)); ;
			});
		}

		private MapperConfiguration MapQueries()
		{
			return new MapperConfiguration(config => {
				config.CreateMap<ContentSource, ContentSourceViewModel>();
			});
		}

		private MapperConfiguration MapUpdate()
		{
			return new MapperConfiguration(config => {
				config.CreateMap<ContentSourceViewModel, ContentSource>();
			});
		}


		public async Task<List<ContentSourceRecord>> BuildGridAsync()
		{
			//return (await _repo.Advertisers.AsQueryable().Map<AdvertiserRecord>(MapGridQueries).ToListAsync()).SpecifyDateTimeKind();
			return (await _repo.ContentSources.AsQueryable().ProjectTo<ContentSourceRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
		}



		public async Task<ContentSourceViewModel> BuildSingleAsync(string id)
		{
			var result2 = _repo.Users.Where(m => m.Id == id).Include(m => m.ContentSource).FirstOrDefault();
			var result3 = result2.ContentSource;
			var result = (await _repo.ContentSources.GetByIdQueryable(result3.Id).ProjectTo<ContentSourceViewModel>(MapQueries()).FirstOrDefaultAsync())?.SpecifyDateTimeKind();
			return (result);
		}

		public async Task<ContentSourceViewModel> BuildSingleAdminAsync(string id)
		{
			var result = await _repo.ContentSources.GetByIdQueryable(id).ProjectTo<ContentSourceViewModel>(MapQueries()).FirstOrDefaultAsync();
			return result;
		}

		public async Task<string> AddAsync(ContentSourceRegistrationViewModel model)
		{
			var dbContentSource = MapAdd().CreateMapper().Map<ContentSource>(model);
			try
			{
				await _repo.ContentSources.AddOrUpdateAndSaveAsync(dbContentSource);
			}catch(DbEntityValidationException ex) {
				foreach (var validationErrors in ex.EntityValidationErrors)
				{
					foreach (var validationError in validationErrors.ValidationErrors)
					{
						Trace.TraceInformation("Property: {0} Error: {1}",
												validationError.PropertyName,
												validationError.ErrorMessage);
					}
				}
			}
			return dbContentSource.Id;
		}

		public async Task UpdateAsync(ContentSourceViewModel model)
		{
			var dbContentSource = await _repo.ContentSources.GetByIdAsync(model.Id);
			
			dbContentSource = MapUpdate().CreateMapper().Map(model, dbContentSource);
			await _repo.SaveAsync();
		}
	}
}
