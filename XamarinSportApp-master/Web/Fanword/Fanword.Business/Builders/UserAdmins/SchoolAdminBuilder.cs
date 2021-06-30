using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExtensionClasses;
using Fanword.Business.ViewModels.UserAdmins;
using Fanword.Data.Entities;
using Fanword.Data.Enums;
using Fanword.Data.IdentityConfig.Roles;
using Fanword.Data.Repository;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.Builders.UserAdmins
{
	public class SchoolAdminBuilder
	{
		private ApplicationRepository _repo { get; set; }

		public SchoolAdminBuilder(ApplicationRepository repo)
		{
			_repo = repo ?? ApplicationRepository.CreateWithoutOwin();
		}

		private MapperConfiguration MapGridQueries()
		{
			return new MapperConfiguration(config => {
				config.CreateMap<SchoolAdmin, SchoolAdminRecord>()
				.ForMember(m => m.Name, m => m.MapFrom(mm => mm.User.FirstName + " " + mm.User.LastName))
				.ForMember(m => m.SchoolName, m => m.MapFrom(mm => mm.School.Name))
				.ForMember(m => m.Status, m => m.MapFrom(mm => mm.AdminStatus));
			});
		}
		private MapperConfiguration MapQueries()
		{
			return new MapperConfiguration(config =>
			{
				config.CreateMap<SchoolAdmin, SchoolAdminViewModel>();
			});
		}
		private MapperConfiguration MapAdd()
		{
			return new MapperConfiguration(config => {
				config.CreateMap<SchoolAdminViewModel, SchoolAdmin>();
			});
		}

		private MapperConfiguration MapUpdate()
		{
			return new MapperConfiguration(config =>
			{
				config.CreateMap<SchoolAdminViewModel, SchoolAdmin>();
				
			});
		}

        public async Task<List<SchoolAdminRecord>> BuildGridAsync(bool showPending)
        {
            if (showPending)
            {
                return (await _repo.SchoolAdmins.Where(m => m.AdminStatus == AdminStatus.Pending).AsQueryable().ProjectTo<SchoolAdminRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
            }
            else { 
                return (await _repo.SchoolAdmins.Where(m => m.AdminStatus != AdminStatus.Pending).AsQueryable().ProjectTo<SchoolAdminRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
            }
        }

		public async Task<SchoolAdminViewModel> BuildSingleAsync(string id)
		{
			return (await _repo.SchoolAdmins.GetByIdQueryable(id).ProjectTo<SchoolAdminViewModel>(MapQueries()).FirstOrDefaultAsync())?.SpecifyDateTimeKind();
		}

		public async Task UpdateAsync(SchoolAdminViewModel model)
		{
			var record = MapUpdate().CreateMapper().Map<SchoolAdmin>(model);
			if (!_repo.RoleManager.RoleExists("SchoolAdmin"))
			{
				var role = new ApplicationRole();
				role.DateCreatedUtc = DateTime.UtcNow;
				role.Name = "SchoolAdmin";
				await _repo.RoleManager.CreateAsync(role);
			}
			if ((model.AdminStatus == AdminStatus.Approved) && (await _repo.UserManager.IsInRoleAsync(model.UserId, "SchoolAdmin") == false))
			{
				await _repo.UserManager.AddToRoleAsync(model.UserId, "SchoolAdmin");
			}
			if ((model.AdminStatus == AdminStatus.Denied || model.AdminStatus == AdminStatus.Pending) && (await _repo.UserManager.IsInRoleAsync(model.UserId, "SchoolAdmin")))
			{
				await _repo.UserManager.RemoveFromRoleAsync(model.UserId, "SchoolAdmin");
			}
			await _repo.SchoolAdmins.AddOrUpdateAndSaveAsync(record);
		}
		public async Task AddAsync(SchoolAdminViewModel model)
		{
			var record = MapAdd().CreateMapper().Map<SchoolAdmin>(model);
			if (!_repo.RoleManager.RoleExists("SchoolAdmin"))
			{
				var role = new ApplicationRole();
				role.DateCreatedUtc = DateTime.UtcNow;
				role.Name = "SchoolAdmin";
				await _repo.RoleManager.CreateAsync(role);
			}
			if (model.AdminStatus == AdminStatus.Approved && (await _repo.UserManager.IsInRoleAsync(model.UserId, "SchoolAdmin") == false))
			{
				await _repo.UserManager.AddToRoleAsync(model.UserId, "SchoolAdmin");
			}
			if ((model.AdminStatus == AdminStatus.Denied || model.AdminStatus == AdminStatus.Pending) && (await _repo.UserManager.IsInRoleAsync(model.UserId, "SchoolAdmin")))
			{
				await _repo.UserManager.RemoveFromRoleAsync(model.UserId, "SchoolAdmin");
			}
			await _repo.SchoolAdmins.AddOrUpdateAndSaveAsync(record);
		}
	}
}
