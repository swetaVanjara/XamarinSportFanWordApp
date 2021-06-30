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
using Fanword.Business.ViewModels.RssKeywords;
using Fanword.Data.Entities;
using Fanword.Data.Repository;
using Fanword.Business.Builders.RssKeywords;
using Fanword.Data.Enums;
using System.Security.Claims;

namespace Fanword.Business.Builders.RssFeeds {
    public class RssFeedBuilder {
        private ApplicationRepository _repo { get; set; }

        public RssFeedBuilder(ApplicationRepository repo) {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        private MapperConfiguration MapGridQueries() {
            return new MapperConfiguration(config => {
                config.CreateMap<RssFeed, RssFeedRecord>()
					.ForMember(m => m.CreatedBy, m=> m.MapFrom(mm => mm.ContentSource.ContentSourceName))
                    .ForMember(m => m.Status, m => m.MapFrom(mm => mm.IsActive ? "Active" : "Inactive"))
                    .ForMember(m => m.AssociatedSchoolOrTeam, m => m.MapFrom(mm => mm.School != null ? mm.School.Name : mm.Team != null ? mm.Team.School.Name + " - " + mm.Team.Sport.Name : mm.Sport != null ? mm.Sport.Name : ""));
            });
        }

        private MapperConfiguration MapQueries() {
            return new MapperConfiguration(config => {
				config.CreateMap<RssFeed, RssFeedViewModel>()
					.ForMember(m => m.CreatedByName, m => m.MapFrom(mm => mm.ContentSource.ContentSourceName));
				config.CreateMap<RssKeyword, RssKeywordViewModel>();
			});
        }
        private MapperConfiguration MapAdd(string contentId, string userId) {
            return new MapperConfiguration(config => {
				config.CreateMap<RssFeedViewModel, RssFeed>()
					.ForMember(m => m.IsActive, m => m.UseValue(true))
					.ForMember(m => m.Id, m => m.MapFrom(mm => Guid.NewGuid().ToString()))
					.ForMember(m => m.DateCreatedUtc, m => m.MapFrom(mm => DateTime.UtcNow))
					.ForMember(m => m.CreatedById, m => m.UseValue(userId))
					.ForMember(m => m.ContentSourceId, m => m.UseValue(contentId))
					.ForMember(m => m.IsActive, m => m.Ignore());
				config.CreateMap<RssKeywordViewModel, RssKeyword>()
					.ForMember(m => m.IsActive, m => m.MapFrom(mm => true))
					.ForMember(m => m.Id, m => m.MapFrom(mm => Guid.NewGuid().ToString()));
				//config.CreateMap<RssKeywordViewModel, RssKeyword>()
				//	.ForMember(m => m.IsActive, m => m.MapFrom(mm => true))
				//	.ForMember(m => m.Id, m => m.MapFrom(mm => Guid.NewGuid().ToString()));
			});
        }
		private MapperConfiguration MapAdminAdd(string userId)
		{
			return new MapperConfiguration(config => {
				config.CreateMap<RssFeedViewModel, RssFeed>()
					.ForMember(m => m.IsActive, m => m.UseValue(true))
					.ForMember(m => m.Id, m => m.MapFrom(mm => Guid.NewGuid().ToString()))
					.ForMember(m => m.DateCreatedUtc, m => m.MapFrom(mm => DateTime.UtcNow))
					.ForMember(m => m.CreatedById, m => m.UseValue(userId))
					.ForMember(m => m.ContentSourceId, m => m.Ignore())
					.ForMember(m => m.IsActive, m => m.Ignore())
					.ForMember(m => m.RssFeedStatus, m => m.UseValue(RssFeedStatus.Approved));
				config.CreateMap<RssKeywordViewModel, RssKeyword>()
					.ForMember(m => m.IsActive, m => m.MapFrom(mm => true))
					.ForMember(m => m.Id, m => m.MapFrom(mm => Guid.NewGuid().ToString()));
				//config.CreateMap<RssKeywordViewModel, RssKeyword>()
				//	.ForMember(m => m.IsActive, m => m.MapFrom(mm => true))
				//	.ForMember(m => m.Id, m => m.MapFrom(mm => Guid.NewGuid().ToString()));
			});
		}

		private MapperConfiguration MapUpdate() {
            return new MapperConfiguration(config => {
                config.CreateMap<RssFeedViewModel, RssFeed>()
                    .ForMember(m=>m.ContentSourceId,m=>m.UseDestinationValue())
                    .ForMember(m => m.Id, m => m.Ignore());
				config.CreateMap<RssKeywordViewModel, RssKeyword>()
					.ForMember(m => m.IsActive, m => m.MapFrom(mm => true))
					.ForMember(m => m.Id, m => m.MapFrom(mm => Guid.NewGuid().ToString()));
			});
        }


		public async Task<List<RssFeedRecord>> BuildGridAsync(bool showInactive, string createdById, bool isAdmin, bool showPending)
		{
			var currentUser = _repo.Users.Where(m => m.Id == createdById).Include("ContentSource").Include(m => m.SchoolAdmins).Include(m => m.TeamAdmins).FirstOrDefault();
			var teamAdmins = currentUser.TeamAdmins.Select(m => m.TeamId).ToList();
			var schoolAdmins = currentUser.SchoolAdmins.Select(m => m.SchoolId).ToList();

		    var query = _repo.RssFeeds.Where(m => (showPending ? m.RssFeedStatus == RssFeedStatus.Pending : m.RssFeedStatus != RssFeedStatus.Pending) && showInactive == !m.IsActive && m.DateDeletedUtc == null && (m.DateDeletedUtc == null && m.Team.DateDeletedUtc == null && m.School.DateDeletedUtc == null && m.Sport.DateDeletedUtc == null));
            
            if (isAdmin)
			{
                return (await query.ProjectTo<RssFeedRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
			}

			if (currentUser.ContentSourceId != null)
			{
			    return (await query.Where(m => (m.ContentSourceId == currentUser.ContentSource.Id)).ProjectTo<RssFeedRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
			}

            if (teamAdmins.Any())
			{
                return (await query.Where(m => teamAdmins.Contains(m.TeamId)).ProjectTo<RssFeedRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();				
			}

			if(schoolAdmins.Any())
			{
			    return (await query.Where(m => schoolAdmins.Contains(m.SchoolId)).ProjectTo<RssFeedRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();				
			}
			return (await  _repo.RssFeeds.Where(m => !m.IsActive && m.RssFeedStatus != RssFeedStatus.Pending && (m.DateDeletedUtc == null && m.Team.DateDeletedUtc == null && m.School.DateDeletedUtc == null && m.Sport.DateDeletedUtc == null) && (m.ContentSourceId == currentUser.ContentSource.Id)).ProjectTo<RssFeedRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
			
		}
		public async Task<List<RssFeedRecord>> BuildSingleGridAsync(string contentSourceId)
		{
			return (await _repo.RssFeeds.Where(m => (m.ContentSource.Id == contentSourceId) && (m.DateDeletedUtc == null)).ProjectTo<RssFeedRecord>(MapGridQueries()).ToListAsync());
		}

        public async Task AddAsync(RssFeedViewModel model, string userId) {
			//model.CreatedBy = userId;
			var currentUser = _repo.Users.Where(m => m.Id == userId).Include("ContentSource").FirstOrDefault();
            var idType = 0;

            if(model.TeamId != null)
            {
                idType = 1;
            }else if(model.SchoolId != null)
            {
                idType = 2;
            }else 
            {
                idType = 3;
            }
            
			//model.CreatedBy = currentUser.ContentSource.Id;
			if(ClaimsPrincipal.Current.IsInRole(AppRoles.SystemAdmin))
			{
				var dbItem2 = MapAdminAdd(userId).CreateMapper().Map<RssFeed>(model);
                if(idType == 1)
                {
                    dbItem2.SchoolId = null;
                    dbItem2.SportId = null;
                }else if(idType == 2)
                {
                    dbItem2.TeamId = null;
                    dbItem2.SportId = null;
                }
                else
                {
                    dbItem2.TeamId = null;
                    dbItem2.SchoolId = null;
                }

				await _repo.RssFeeds.AddOrUpdateAndSaveAsync(dbItem2);
				return;

			}

			var dbItem = MapAdd(currentUser.ContentSourceId, userId).CreateMapper().Map<RssFeed>(model);
            if (idType == 1)
            {
                dbItem.SchoolId = null;
                dbItem.SportId = null;
            }
            else if (idType == 2)
            {
                dbItem.TeamId = null;
                dbItem.SportId = null;
            }
            else
            {
                dbItem.TeamId = null;
                dbItem.SchoolId = null;
            }
            await _repo.RssFeeds.AddOrUpdateAndSaveAsync(dbItem);
        }

        public async Task UpdateAsync(RssFeedViewModel model) {
			try
			{
				//Delete Keywords from Database
				var dbItem = await _repo.RssFeeds.GetByIdAsync(model.Id, m => m.Include(mm => mm.RssKeywords));
				if (model.DateDeletedUtc != null)
				{
					var dbPosts = await _repo.Posts.Where(m => m.FeedId == model.Id).ToListAsync();

					foreach(var post in dbPosts)
					{
						post.DateDeletedUtc = DateTime.UtcNow;
					}
					foreach (var post in dbPosts)
					{
						await _repo.Posts.AddOrUpdateAndSaveAsync(post);
					}
				}

				while (dbItem.RssKeywords.Count > 0)
				{
					dbItem.RssKeywords.Remove(dbItem.RssKeywords.FirstOrDefault());
				}
				await _repo.SaveAsync();
                var idType = 0;
                if (dbItem.TeamId == null && model.TeamId != null )
                {
                    idType = 1;
                }
                else if (dbItem.SchoolId == null && model.SchoolId != null)
                {
                    idType = 2;
                }
                else if(dbItem.SportId == null && model.SportId != null)
                {
                    idType = 3;
                }
                else if(model.SportId == null && model.TeamId == null && model.SchoolId == null)
                {
                    idType = 4;
                }

                dbItem = MapUpdate().CreateMapper().Map(model, dbItem);
                if (idType == 1)
                {
                    dbItem.SchoolId = null;
                    dbItem.SportId = null;
                }
                else if (idType == 2)
                {
                    dbItem.TeamId = null;
                    dbItem.SportId = null;
                }
                else if (idType == 3)
                {
                    dbItem.SchoolId = null;
                    dbItem.TeamId = null;
                }
                else if(idType == 4)
                {
                    dbItem.TeamId = null;
                    dbItem.SportId = null;
                    dbItem.SchoolId = null;
                }


                await _repo.RssFeeds.AddOrUpdateAndSaveAsync(dbItem);

			}
			catch (Exception ex)
			{
				throw;
			}
            
        }

        public async Task<RssFeedViewModel> BuildSingleAsync(string id) {
            return (await _repo.RssFeeds.GetByIdQueryable(id).ProjectTo<RssFeedViewModel>(MapQueries()).FirstOrDefaultAsync())?.SpecifyDateTimeKind();
        }

    }
}