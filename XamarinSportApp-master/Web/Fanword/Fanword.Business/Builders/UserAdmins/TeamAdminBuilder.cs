using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExtensionClasses;
using Fanword.Business.ViewModels.UserAdmins;
using Fanword.Data.Entities;
using Fanword.Data.Enums;
using Fanword.Data.IdentityConfig.Managers;
using Fanword.Data.IdentityConfig.Roles;
using Fanword.Data.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.Builders.UserAdmins
{
	public class TeamAdminBuilder
	{
		
		private ApplicationRepository _repo { get; set; }
		public TeamAdminBuilder(ApplicationRepository repo)
		{
			_repo = repo ?? ApplicationRepository.CreateWithoutOwin();
		}

		private MapperConfiguration MapGridQueries()
		{
			return new MapperConfiguration(config => {
				config.CreateMap<TeamAdmin, TeamAdminRecord>()
				.ForMember(m => m.Name, m => m.MapFrom(mm => mm.User.FirstName + " " + mm.User.LastName))
				.ForMember(m => m.TeamName, m => m.MapFrom(mm => mm.Team.School.Name + " - " + mm.Team.Sport.Name))
				.ForMember(m => m.Status, m => m.MapFrom(mm => mm.AdminStatus)); 
			});
		}

		private MapperConfiguration MapQueries()
		{
			return new MapperConfiguration(config =>
			{
				config.CreateMap<TeamAdmin, TeamAdminViewModel>();
			});
		}

		private MapperConfiguration MapAdd()
		{
			return new MapperConfiguration(config => {
				config.CreateMap<TeamAdminViewModel, TeamAdmin>();
			});
		}

		private MapperConfiguration MapUpdate()
		{
			return new MapperConfiguration(config =>
			{
				config.CreateMap<TeamAdminViewModel, TeamAdmin>();
			});
		}

		public async Task<List<TeamAdminRecord>> BuildGridAsync(bool showPending)
		{
            if (showPending)
            {
                return (await _repo.TeamAdmins.Where(m => m.AdminStatus == AdminStatus.Pending).AsQueryable().ProjectTo<TeamAdminRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
            }
            return (await _repo.TeamAdmins.Where(m => m.AdminStatus != AdminStatus.Pending).AsQueryable().ProjectTo<TeamAdminRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
		}

		public async Task<TeamAdminViewModel> BuildSingleAsync(string id)
		{
			return (await _repo.TeamAdmins.GetByIdQueryable(id).ProjectTo<TeamAdminViewModel>(MapQueries()).FirstOrDefaultAsync())?.SpecifyDateTimeKind();
		}

		public async Task UpdateAsync(TeamAdminViewModel model)
		{
			var record = MapUpdate().CreateMapper().Map<TeamAdmin>(model);
			if (!_repo.RoleManager.RoleExists("TeamAdmin"))
			{
				var role = new ApplicationRole();
				role.Name = "TeamAdmin";
				role.DateCreatedUtc = DateTime.UtcNow;
				await _repo.RoleManager.CreateAsync(role);
			}
			if (model.AdminStatus == AdminStatus.Approved && (await _repo.UserManager.IsInRoleAsync(model.UserId, "TeamAdmin") == false))
			{
				await _repo.UserManager.AddToRoleAsync(model.UserId, "TeamAdmin");
			}
			if ((model.AdminStatus == AdminStatus.Denied || model.AdminStatus == AdminStatus.Pending) && (await _repo.UserManager.IsInRoleAsync(model.UserId, "TeamAdmin")))
			{
				await _repo.UserManager.RemoveFromRoleAsync(model.UserId, "TeamAdmin");
			}
			await _repo.TeamAdmins.AddOrUpdateAndSaveAsync(record);
		}

		public async Task AddAsync(TeamAdminViewModel model)
		{
			var record = MapAdd().CreateMapper().Map<TeamAdmin>(model);
			if (!_repo.RoleManager.RoleExists("TeamAdmin")) {
				var role = new ApplicationRole();
				role.Name = "TeamAdmin";
				role.DateCreatedUtc = DateTime.UtcNow;
				await _repo.RoleManager.CreateAsync(role);
			}
			if (model.AdminStatus == AdminStatus.Approved && (await _repo.UserManager.IsInRoleAsync(model.UserId, "TeamAdmin") == false))
			{
				await _repo.UserManager.AddToRoleAsync(model.UserId, "TeamAdmin");
			}
			if ((model.AdminStatus == AdminStatus.Denied || model.AdminStatus == AdminStatus.Pending) && (await _repo.UserManager.IsInRoleAsync(model.UserId, "TeamAdmin")))
			{
				await _repo.UserManager.RemoveFromRoleAsync(model.UserId, "TeamAdmin");
			}
			await _repo.TeamAdmins.AddOrUpdateAndSaveAsync(record);
		}
	}
}
