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
using Fanword.Business.ViewModels.Schools;
using Fanword.Business.ViewModels.Sports;
using Fanword.Data.Entities;
using Fanword.Data.Repository;
using FileUploads.Azure;
using Fanword.Data.Enums;

namespace Fanword.Business.Builders.Schools {
    public class SchoolsBuilder {
        private ApplicationRepository _repo { get; set; }

        public SchoolsBuilder(ApplicationRepository repo) {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        private MapperConfiguration MapGridQueries() {
            return new MapperConfiguration(config => {
                config.CreateMap<School, SchoolRecord>()
                    .ForMember(m => m.NumberOfAthletes, m => m.MapFrom(mm => mm.Teams.SelectMany(i => i.Atheletes).Count(i=>i.ApplicationUser.DateDeletedUtc==null)))
                    .ForMember(m => m.NumberOfTeams, m => m.MapFrom(mm => mm.Teams.Count(i=>i.DateDeletedUtc ==null)))
                    .ForMember(m => m.NumberOfPosts, m => m.MapFrom(mm => mm.Posts.Count(i=>i.DateDeletedUtc==null)));
            });
        }

        private MapperConfiguration MapQueries() {
            return new MapperConfiguration(config => {
                config.CreateMap<School, SchoolViewModel>();
            });
        }

        private MapperConfiguration MapAddUpdate(bool isUpdate) {
            return new MapperConfiguration(config => {
                if (isUpdate) {
                    config.CreateMap<SchoolViewModel, School>()
                        .ForMember(m => m.Id, m => m.Ignore());
                }
                else {
                    config.CreateMap<SchoolViewModel, School>()
                        .ForMember(m => m.Id, m => m.MapFrom(mm => Guid.NewGuid().ToString()));
                }
            });
        }


        public async Task<List<SchoolRecord>> BuildGridAsync(bool showInactive) {
			if (showInactive == true)
			{
				return (await _repo.Schools.Where(m => !m.IsActive && m.DateDeletedUtc == null).AsQueryable().ProjectTo<SchoolRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
			}
			else
			{
				return (await _repo.Schools.Where(m => m.IsActive && m.DateDeletedUtc == null).AsQueryable().ProjectTo<SchoolRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
			}
        }

        public async Task<SchoolViewModel> BuildSingleAsync(string id) {
            return (await _repo.Schools.GetByIdQueryable(id).ProjectTo<SchoolViewModel>(MapQueries()).FirstOrDefaultAsync())?.SpecifyDateTimeKind();
        }


        public async Task<SchoolViewModel> AddAsync(SchoolViewModel model) {
            var result = await _repo.Schools.AddOrUpdateAndSaveAsync(MapAddUpdate(false).CreateMapper().Map<School>(model));
            return await BuildSingleAsync(result.Id);
        }

        public async Task UpdateAsync(SchoolViewModel model) {
            var dbSport = await _repo.Schools.GetByIdAsync(model.Id);
            if (model.ProfileBlob != dbSport.ProfileBlob) {
                //delete current one
                new AzureStorage(dbSport.ProfileContainer, true).DeleteFile(dbSport.ProfileContainer);
            }
            if (model.IsActive == false)
            {
                var teams = await _repo.Teams.Where(m => m.SchoolId == model.Id && m.DateDeletedUtc == null).ToListAsync();
                foreach (var team in teams)
                {
                    team.IsActive = false;
                }
                var feeds = await _repo.RssFeeds.Where(m => m.SchoolId == model.Id && m.DateDeletedUtc == null).ToListAsync();
                foreach (var feed in feeds)
                {
                    feed.IsActive = false;
                }
            }
            await _repo.SaveAsync();
            await _repo.Schools.AddOrUpdateAndSaveAsync(MapAddUpdate(true).CreateMapper().Map(model, dbSport));

        }
		public async Task DeleteAsync(string id)
		{
			await _repo.Schools.DeleteAndSaveAsync(id);
			
			var feeds = await _repo.RssFeeds.Where(m => m.SchoolId == id).ToListAsync();
			foreach (var feed in feeds)
			{
				feed.DateDeletedUtc = DateTime.UtcNow;
				feed.RssFeedStatus = RssFeedStatus.Denied;
			}
			await _repo.SaveAsync();

			var schoolAdmins = await _repo.SchoolAdmins.Where(m => m.SchoolId == id).ToListAsync();
			foreach (var admin in schoolAdmins)
			{
                var userId = admin.UserId;
                var user = _repo.Users.GetById(userId);
                if (user.SchoolAdmins.Where(m => m.DateDeletedUtc == null).Count() == 1)
                {
                    await _repo.UserManager.RemoveFromRoleAsync(userId, "SchoolAdmin");
                }
                await _repo.SchoolAdmins.DeleteAndSaveAsync(admin);
			}

			var posts = await _repo.Posts.Where(m =>m.SchoolId == id || m.Schools.Any(i => i.Id == id)).Include(i => i.Schools).ToListAsync();
			foreach (var post in posts)
			{
				if (post.DateDeletedUtc == null && post.SchoolId == id)
				{
					await _repo.Posts.DeleteAndSaveAsync(post.Id);
				}
				var schoolTag = post.Schools.FirstOrDefault(m => m.Id == id);
                if (schoolTag != null)
                {
                    post.Schools.Remove(schoolTag);
                }
			}

            var teamIds = await _repo.Teams.Where(m => m.SchoolId == id).Select(m => m.Id).ToListAsync();
            foreach (var teamId in teamIds)
			{
			    await new TeamsBuilder(_repo).DeleteAsync(teamId);
			}
			await _repo.SaveAsync();
		}
    }
}
