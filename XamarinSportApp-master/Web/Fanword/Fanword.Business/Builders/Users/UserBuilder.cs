using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExtensionClasses;
using Fanword.Business.ViewModels.Users;
using Fanword.Data.Entities;
using Fanword.Data.Enums;
using Fanword.Data.IdentityConfig.User;
using Fanword.Data.Repository;
using Microsoft.AspNet.Identity;

namespace Fanword.Business.Builders.Users {
    public class UserBuilder {
        private ApplicationRepository _repo { get; set; }

        public UserBuilder(ApplicationRepository repo) {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        private MapperConfiguration MapGridQueries() {
            return new MapperConfiguration(config => {
                config.CreateMap<ApplicationUser, UserRecord>()
                    .ForMember(m => m.IsStudentAthlete, m => m.MapFrom(mm => mm.Atheletes.Any()))
                    .ForMember(m => m.Posts, m => m.MapFrom(mm => mm.CreatedByPosts.Count(i => i.DateDeletedUtc == null)))
                    .ForMember(m => m.ProfilePictureUrl, m => m.MapFrom(mm => mm.ProfileUrl ?? "https://fanword.blob.core.windows.net/appimages/DefProfPic.png"))
					.ForMember(m => m.IsDeleted, m => m.MapFrom(mm => mm.DateDeletedUtc != null))
                    .ForMember(m => m.Name, m => m.MapFrom(mm => mm.FirstName + " " + mm.LastName))
					.ForMember(m => m.ContentSource, m => m.MapFrom(mm => mm.ContentSource.ContentSourceName))
					.ForMember(m => m.Followers, m => m.MapFrom(mm => mm.Followers.Count()));
            });
        }

        private MapperConfiguration MapQueries() {
            return new MapperConfiguration(config => {
                config.CreateMap<ApplicationUser, UserViewModel>()
                    .ForMember(m => m.Password, m => m.Ignore())
					.ForMember(m => m.ContentSourceId, m => m.MapFrom(mm => mm.ContentSource.Id));
            });
        }

        private MapperConfiguration MapAtheleteGrid() {
            return new MapperConfiguration(config => {
                config.CreateMap<Athelete, AtheleteRecord>()
                    .ForMember(m=>m.Team,m=>m.MapFrom(mm=>mm.Team.School.Name + " - " + mm.Team.Sport.Name));
            });
        }

		public async Task<List<UserRecord>> BuildGridAsync(bool showDeleted, bool showInactive, bool showPending)
		{
			//var query = showDeleted ? _repo.Context.Users : _repo.Users.AsQueryable().Include(m => m.Atheletes);
			var query = _repo.Context.Users.AsQueryable().Include(m => m.Atheletes);
            if (showPending)
            {
                query = query.Where(m => m.Atheletes.Count != 0 && m.Atheletes.All(i => !i.Verified));
                return (await query.ProjectTo<UserRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
            }
            if (showInactive && showDeleted)
			{
               
				query = query.Where(m => !m.IsActive || m.DateDeletedUtc != null);
				return (await query.ProjectTo<UserRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();

			}
			if (showInactive && !showDeleted)
			{
				query = query.Where(m => !m.IsActive);
				return (await query.ProjectTo<UserRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();

			}

			if (!showInactive && showDeleted)
			{
				query = query.Where(m => m.DateDeletedUtc != null);
				return (await query.ProjectTo<UserRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
			}

			query = query.Where(m => m.DateDeletedUtc == null && m.IsActive);
			return (await query.ProjectTo<UserRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
		}

        public async Task<List<AtheleteRecord>> AthleteYearsAsync(string userId) {
            var query = _repo.Atheletes.Where(m => m.ApplicationUserId == userId);
            return (await query.ProjectTo<AtheleteRecord>(MapAtheleteGrid()).ToListAsync()).SpecifyDateTimeKind();
        }

        public async Task FlipVerificationAsync(string id) {
            var dbItem = await _repo.Atheletes.GetByIdAsync(id);
            if (dbItem == null) return;
            dbItem.Verified = !dbItem.Verified;
            await _repo.SaveAsync();
        }

        public async Task<UserViewModel> BuildSingleAsync(string id) {
            return (await _repo.Users.GetByIdQueryable(id).ProjectTo<UserViewModel>(MapQueries()).FirstOrDefaultAsync())?.SpecifyDateTimeKind();
        }

        public async Task<UserViewModel> AddAsync(UserViewModel model) {
            //assume validated
            var user = new ApplicationUser() {
                Id = Guid.NewGuid().ToString(),
                DateCreatedUtc = DateTime.UtcNow,
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsActive = true,
                ProfileBlob = model.ProfileBlob,
                ProfileContainer = model.ProfileContainer,
                ProfileUrl = model.ProfileUrl,
            };

            await _repo.UserManager.CreateAsync(user, model.Password);
            await _repo.UserManager.AddToRoleAsync(user.Id, AppRoles.SystemAdmin);
            return await BuildSingleAsync(user.Id);
        }

        public async Task<UserViewModel> UpdateAsync(UserViewModel model) {
            var user = await _repo.UserManager.FindByIdAsync(model.Id);

            if (user == null) return null;
			if (model.ContentSourceId != null)
			{
				_repo.UserManager.AddToRole(model.Id, "ContentSource");
			}
			user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.IsActive = model.IsActive;
            user.ProfileBlob = model.ProfileBlob;
            user.ProfileContainer = model.ProfileContainer;
            user.ProfileUrl = model.ProfileUrl;

			var contentSource = await _repo.ContentSources.Where(m => m.Id == model.ContentSourceId).FirstOrDefaultAsync();
			user.ContentSource = contentSource;
            await _repo.UserManager.UpdateAsync(user);
            return await BuildSingleAsync(user.Id);
        }

        public async Task DeleteAsync(string id) {
            await _repo.Users.DeleteAndSaveAsync(id);
			var posts = await _repo.Posts.Where(m => m.CreatedById == id && m.TeamId == null && m.SchoolId == null).ToListAsync();
			foreach (var post in posts)
			{
				if(post.DateDeletedUtc == null)
				{
				    await _repo.Posts.DeleteAndSaveAsync(post.Id);
				}		
			}
			await _repo.SaveAsync();
		}
    }
}
