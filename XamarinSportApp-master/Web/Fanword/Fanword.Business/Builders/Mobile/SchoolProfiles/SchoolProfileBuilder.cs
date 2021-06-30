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
using Fanword.Data.Entities;
using Fanword.Data.Enums;
using Fanword.Data.Repository;
using Fanword.Poco.Models;

namespace Fanword.Business.Builders.Mobile.SchoolProfiles
{
    public class SchoolProfileBuilder : BaseBuilder
    {
        public SchoolProfileBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        public async Task<SchoolProfile> BuildAsync(string userId, string teamId)
        {
            var followedSchools = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Schools.Select(j => j.Id));
            MapperConfiguration config = new MapperConfiguration(k =>
                k.CreateMap<Data.Entities.School, SchoolProfile>()
                    .ForMember(m => m.Followers, m => m.MapFrom(mm => mm.Followers.Count))
                    .ForMember(m => m.ProfileUrl, m => m.MapFrom(mm => mm.ProfilePublicUrl))
                    .ForMember(m => m.Posts, m => m.MapFrom(mm => mm.PostsBySchool.Count(j => j.DateDeletedUtc == null)))
                    .ForMember(m => m.IsFollowing, m => m.MapFrom(mm => followedSchools.Contains(mm.Id)))
                    .ForMember(m => m.IsProfileAdmin, m => m.MapFrom(mm => mm.Admins.Any(j => j.UserId == userId && j.AdminStatus == AdminStatus.Approved))));

            return await _repo.Schools.Where(m => m.Id == teamId).ProjectTo<SchoolProfile>(config).FirstOrDefaultAsync();
        }

        public async Task RequestAdminAsync(string userId, string schoolId)
        {
            var existingAdmin = await _repo.SchoolAdmins.FirstOrDefaultAsync(m => m.UserId == userId && m.SchoolId == schoolId);
            if (existingAdmin == null)
            {
                existingAdmin = new SchoolAdmin() { UserId = userId, SchoolId = schoolId, AdminStatus = AdminStatus.Pending };
                await _repo.SchoolAdmins.AddOrUpdateAndSaveAsync(existingAdmin);
            }
        }
    }
}
