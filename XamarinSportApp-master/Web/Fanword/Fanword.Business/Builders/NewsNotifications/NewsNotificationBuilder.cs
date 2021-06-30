using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExtensionClasses;
using Fanword.Business.Hangfire.PushNotifications;
using Fanword.Business.ViewModels.NewsNotifications;
using Fanword.Data.Entities;
using Fanword.Data.Repository;
using Hangfire;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Fanword.Data.Enums;

namespace Fanword.Business.Builders.NewsNotifications {
    public class NewsNotificationBuilder {
        private ApplicationRepository _repo { get; set; }

        public NewsNotificationBuilder(ApplicationRepository repo) {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        private MapperConfiguration MapGridQueries() {
            return new MapperConfiguration(config => {
				config.CreateMap<NewsNotification, NewsNotificationRecord>()
				.ForMember(m => m.CreatedBy, m => m.MapFrom(mm => mm.Sport == null ? mm.ContentSource == null ? mm.Team == null ? mm.School == null ? "System Admin" : mm.School.Name + " Admin": mm.Team.School.Name + " " + mm.Team.Sport.Name + " Admin" : mm.ContentSource.ContentSourceName : mm.Sport.Name));
            });
        }

		private MapperConfiguration MapQueries()
		{
			return new MapperConfiguration(config =>
			{
				config.CreateMap<NewsNotification, NewsNotificationViewModel>()
				.ForMember(m => m.Status, m=> m.MapFrom(mm => mm.NewsNotificationStatus));
			});
		}

        private MapperConfiguration MapAdd() {
            return new MapperConfiguration(config => {
                config.CreateMap<NewsNotificationViewModel, NewsNotification>()
                    .ForMember(m => m.DateCreatedUtc, m => m.MapFrom(mm => DateTime.UtcNow))
                    .ForMember(m => m.PushDateUtc, m => m.MapFrom(mm => mm.PushDateUtc.ToUniversalTime()))
					.ForMember(m => m.ContentSourceId, m => m.MapFrom(mm => mm.ContentSourceId))
					.ForMember(m => m.NewsNotificationStatus, m => m.MapFrom(mm => mm.Status));
            });
        }

		private MapperConfiguration MapUpdate()
		{
			return new MapperConfiguration(config =>
			{
				config.CreateMap<NewsNotificationViewModel, NewsNotification>()
				.ForMember(m => m.DateCreatedUtc, m => m.MapFrom(mm => DateTime.UtcNow))
				.ForMember(m => m.NewsNotificationStatus, m => m.MapFrom(mm => mm.Status))
				.ForMember(m => m.PushDateUtc, m => m.MapFrom(mm => mm.PushDateUtc.ToUniversalTime()));
			});
		}

        public async Task<List<NewsNotificationRecord>> BuildGridAsync() {
			var query = _repo.NewsNotifications.AsQueryable();
			if (ClaimsPrincipal.Current.IsInRole("ContentSource") && (!ClaimsPrincipal.Current.IsInRole("System_Admin")))
			{
				var contentSourceId = _repo.Users.GetByIdQueryable(ClaimsPrincipal.Current.Identity.GetUserId()).Select(i => i.ContentSourceId).FirstOrDefault();
				query = query.Where(i => i.ContentSourceId == contentSourceId);
			}
			if (ClaimsPrincipal.Current.IsInRole("TeamAdmin"))
			{
				var userId = ClaimsPrincipal.Current.Identity.GetUserId();
				var teamId = _repo.TeamAdmins.Where(m => m.UserId == userId).Select(i => i.TeamId).FirstOrDefault();
				query = query.Where(i => i.TeamId == teamId && i.CreatedById == userId);
			}
			if (ClaimsPrincipal.Current.IsInRole("SchoolAdmin"))
			{
				var userId = ClaimsPrincipal.Current.Identity.GetUserId();
				var schoolId = _repo.SchoolAdmins.Where(m => m.UserId == userId).Select(i => i.SchoolId).FirstOrDefault();
				query = query.Where(i => i.SchoolId == schoolId && (i.CreatedById == userId));
			}
            return (await query.ProjectTo<NewsNotificationRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
        }
		
		public async Task<NewsNotificationViewModel> BuildSingleAsync(string notificationId)
		{
			return (await _repo.NewsNotifications.GetByIdQueryable(notificationId).ProjectTo<NewsNotificationViewModel>(MapQueries()).FirstOrDefaultAsync())?.SpecifyDateTimeKind();
		}

		public async Task UpdateAsync(NewsNotificationViewModel model)
		{
			var record = MapUpdate().CreateMapper().Map<NewsNotification>(model);
			if ((model.Status == NewsNotificationStatus.Approved) && (model.HangfireTaskId == null))
			{
				if (DateTime.Compare(model.PushDateUtc, DateTime.UtcNow) < 0)
				{
					BackgroundJob.Enqueue(() => new PushNotificationWorker().SendPush(model.Id));
				}
				else
				{
					var job = BackgroundJob.Schedule(() => new PushNotificationWorker().SendPush(model.Id), model.PushDateUtc - DateTime.UtcNow);

					record.HangfireTaskId = job;
				}
			}
			else
			{
			    if (!String.IsNullOrEmpty(record.HangfireTaskId)) {
			        BackgroundJob.Delete(record.HangfireTaskId);
			        record.HangfireTaskId = null;
                }
			}

			await _repo.NewsNotifications.AddOrUpdateAndSaveAsync(record);

		}
		public async Task AddAsync(NewsNotificationViewModel model) {
			model.CreatedById = ClaimsPrincipal.Current.Identity.GetUserId();
            var record = MapAdd().CreateMapper().Map<NewsNotification>(model);
            
            await _repo.NewsNotifications.AddOrUpdateAndSaveAsync(record);

		    if (model.Status == NewsNotificationStatus.Approved)
		    {
		        var job = BackgroundJob.Schedule(() => new PushNotificationWorker().SendPush(record.Id), model.PushDateUtc - DateTime.UtcNow);
		        record.HangfireTaskId = job;
		    }

		    await _repo.NewsNotifications.AddOrUpdateAndSaveAsync(record);
        }
    }
}
