using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExtensionClasses.AutoMapper.Mappers;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Business.Builders.Mobile.Teams;
using Fanword.Data.Context;
using Fanword.Data.Enums;
using Fanword.Data.IdentityConfig.User;
using Fanword.Data.Repository;
using Fanword.Poco.Models;
namespace Fanword.Business.Builders.Mobile
{
    public class UsersBuilder : BaseBuilder
    {
        public UsersBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        public async Task<User> SaveUserAync(User user, string userId)
        {
            var dbUser = await _repo.UserManager.FindByIdAsync(userId);
            user.Map(dbUser);
            await _repo.UserManager.UpdateAsync(dbUser);
            return GetUser(userId);
        }

        public User GetUser(string userId)
        {
            MapperConfiguration config = new MapperConfiguration(k =>
                k.CreateMap<ApplicationUser, User>()
                    .ForMember(m => m.ContentSourceName, m => m.MapFrom(mm => mm.ContentSource.ContentSourceName))
                    .ForMember(m => m.ContentSourceUrl, m => m.MapFrom(mm => mm.ContentSource.LogoUrl))
                    .ForMember(m => m.ContentSourceApproved, m => m.MapFrom(mm => mm.ContentSource.IsApproved == null ? false : mm.ContentSource.IsApproved))
                    .ForMember(m => m.AthleteTeamId, m => m.MapFrom(mm => mm.Atheletes.Select(j => j.TeamId).FirstOrDefault()))
                    .ForMember(m => m.AthleteProfileUrl, m => m.MapFrom(mm => mm.Atheletes.Select(j => j.Team.ProfilePublicUrl).FirstOrDefault()))
                    .ForMember(m => m.AthleteSchool, m => m.MapFrom(mm => mm.Atheletes.Select(j => j.Team.School.Name).FirstOrDefault()))
                    .ForMember(m => m.AthleteSport, m => m.MapFrom(mm => mm.Atheletes.Select(j => j.Team.Sport.Name).FirstOrDefault()))
                    .ForMember(m => m.AthleteStartDateUtc, m => m.MapFrom(mm => mm.Atheletes.Select(j => j.StartUtc).FirstOrDefault()))
                    .ForMember(m => m.AthleteEndDateUtc, m => m.MapFrom(mm => mm.Atheletes.Select(j => j.EndUtc).FirstOrDefault()))
                    .ForMember(m => m.AthleteVerified, m => m.MapFrom(mm => mm.Atheletes.Select(j => j.Verified).FirstOrDefault() == null ? false : mm.Atheletes.Select(j => j.Verified).FirstOrDefault()))
                    .ForMember(m => m.AdminTeams, m => m.MapFrom(mm => mm.TeamAdmins.Where(j => j.Team.IsActive && j.DateDeletedUtc == null && j.AdminStatus == AdminStatus.Approved).Select(j => new Poco.Models.TeamProfile() { Id = j.TeamId, SchoolName = j.Team.School.Name, SportName = j.Team.Sport.Name, ProfileUrl = j.Team.ProfilePublicUrl})))
                    .ForMember(m => m.AdminSchools, m => m.MapFrom(mm => mm.SchoolAdmins.Where(j => j.School.IsActive && j.DateDeletedUtc == null && j.AdminStatus == AdminStatus.Approved).Select(j => new Poco.Models.SchoolProfile() { Id = j.SchoolId, Name = j.School.Name, ProfileUrl = j.School.ProfilePublicUrl }))));

            var repo = new ApplicationRepository(new ApplicationDbContext());
            var user = repo.Users.Where(m => m.Id == userId).ProjectTo<User>(config).FirstOrDefault();
            return user;
        }
        public User GetActiveUser(string userId)
        {
            MapperConfiguration config = new MapperConfiguration(k =>
                k.CreateMap<ApplicationUser, User>()
                    .ForMember(m => m.IsActive, m => m.MapFrom(mm => mm.Users.Where(j => j.IsActive && j.DateDeletedUtc == null))));

            var repo = new ApplicationRepository(new ApplicationDbContext());
            var user = repo.Users.Where(m => m.Id == userId).ProjectTo<User>(config).FirstOrDefault();
            return user;
        }
    }
}
