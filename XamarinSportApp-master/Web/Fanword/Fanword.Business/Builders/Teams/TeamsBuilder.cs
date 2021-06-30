using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExtensionClasses;
using Fanword.Business.Builders.Sports;
using Fanword.Business.ViewModels.Sports;
using Fanword.Business.ViewModels.Teams;
using Fanword.Data.Entities;
using Fanword.Data.Repository;
using FileUploads.Azure;
using FileUploads.Core.Models;
using Notifications.Service;

namespace Fanword.Business.Builders.Teams
{
    public class TeamsBuilder
    {
        private ApplicationRepository _repo { get; set; }

        public TeamsBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        private MapperConfiguration MapGridQueries()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<Team, TeamRecord>()
                    .ForMember(m => m.Name, m => m.MapFrom(mm => mm.School.Name + " - " + mm.Sport.Name))
                    .ForMember(m => m.IsSportActive, m => m.MapFrom(mm => mm.Sport.IsActive))
                    .ForMember(m => m.IsSchoolActive, m => m.MapFrom(mm => mm.School.IsActive))
                    .ForMember(m => m.SchoolName, m => m.MapFrom(mm => mm.School.Name))
                    .ForMember(m => m.SportName, m => m.MapFrom(mm => mm.Sport.Name));
            });
        }

        private MapperConfiguration MapQueries()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<Team, TeamViewModel>();
            });
        }

        private MapperConfiguration MapAddUpdate(bool isUpdate)
        {
            return new MapperConfiguration(config =>
            {
                if (isUpdate)
                {
                    config.CreateMap<TeamViewModel, Team>()
                        .ForMember(m => m.Id, m => m.Ignore());
                }
                else
                {
                    config.CreateMap<TeamViewModel, Team>()
                        .ForMember(m => m.Id, m => m.MapFrom(mm => Guid.NewGuid().ToString()));
                }
            });
        }

        public async Task<List<TeamRecord>> BuildGridAsync(bool showInactive)
        {
            if (showInactive == true)
            {
                return (await _repo.Teams.Where(m => m.Sport.DateDeletedUtc == null && m.School.DateDeletedUtc == null && !m.IsActive).ProjectTo<TeamRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
            }
            else
            {

                return (await _repo.Teams.Where(m => m.Sport.DateDeletedUtc == null && m.School.DateDeletedUtc == null && m.IsActive).ProjectTo<TeamRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
            }
        }

        public async Task<TeamViewModel> BuildSingleAsync(string id)
        {
            return (await _repo.Teams.GetByIdQueryable(id).ProjectTo<TeamViewModel>(MapQueries()).FirstOrDefaultAsync())?.SpecifyDateTimeKind();
        }


        public async Task AddAsync(TeamViewModel model)
        {
            await _repo.Teams.AddOrUpdateAndSaveAsync(MapAddUpdate(false).CreateMapper().Map<Team>(model));
        }

        public async Task UpdateAsync(TeamViewModel model)
        {
            var dbSport = await _repo.Teams.GetByIdAsync(model.Id);
            if (model.ProfileBlob != dbSport.ProfileBlob)
            {
                //delete current one
                new AzureStorage(dbSport.ProfileContainer, true).DeleteFile(dbSport.ProfileBlob);
            }
            if (model.IsActive == false)
            {
                var feeds = await _repo.RssFeeds.Where(m => m.TeamId == model.Id && m.DateDeletedUtc == null).ToListAsync();
                foreach (var feed in feeds)
                {
                    feed.IsActive = false;
                }
            }
            await _repo.SaveAsync();
            await _repo.Teams.AddOrUpdateAndSaveAsync(MapAddUpdate(true).CreateMapper().Map(model, dbSport));
        }

        /// <summary>
        /// Delete the team, associated athletes, team rankings, rssfeeds, team admins, news notifications, posts, and post tags
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string id)
        {
            var team = await _repo.Teams.Where(m => m.Id == id).FirstOrDefaultAsync();
            team.DateDeletedUtc = DateTime.UtcNow;
            
            var athletes = await _repo.Atheletes.Where(m => m.TeamId == id).ToListAsync();
            foreach (var athlete in athletes)
            {
                await _repo.Atheletes.DeleteAndSaveAsync(athlete);
            }
            var ranking = await _repo.RankingTeams.Where(m => m.TeamId == id).FirstOrDefaultAsync();
            if (ranking != null)
            {
                await _repo.RankingTeams.DeleteAndSaveAsync(ranking);
            }

            var feeds = await _repo.RssFeeds.Where(m => m.TeamId == id).ToListAsync();
            foreach (var feed in feeds)
            {
                feed.DateDeletedUtc = DateTime.UtcNow;
                feed.RssFeedStatus = Data.Enums.RssFeedStatus.Denied;
            }
            await _repo.SaveAsync();

            var teamAdmins = await _repo.TeamAdmins.Where(m => m.TeamId == id).ToListAsync();

            foreach (var admin in teamAdmins)
            {
                var userId = admin.UserId;
                var user = _repo.Users.GetById(userId);
                if(user.TeamAdmins.Where(m => m.DateDeletedUtc == null).Count() == 1)
                {
                    await _repo.UserManager.RemoveFromRoleAsync(userId, "TeamAdmin");
                }
                admin.DateDeletedUtc = DateTime.UtcNow;
            }

            var newsNotifications = _repo.NewsNotifications.Where(m => m.TeamId == id);
            foreach (var newsNotification in newsNotifications)
            {
                await _repo.NewsNotifications.DeleteAndSaveAsync(newsNotification);
            }
            
            await _repo.SaveAsync();
            var posts = await _repo.Posts.Where(m => (m.TeamId == id || m.Feed.TeamId == id || m.Teams.Any(i => i.Id == id)) && m.DateDeletedUtc == null).Include(m => m.Teams).Include(m => m.Feed).ToListAsync();
            foreach (var post in posts)
            {
                if (post.TeamId == id)
                {
                    post.DateDeletedUtc = DateTime.UtcNow;
                }
                var teamTag = post.Teams.FirstOrDefault(m => m.Id == id);
                if (teamTag != null)
                {
                    post.Teams.Remove(teamTag);
                }
            }

            await _repo.SaveAsync();
        }

        public async Task AddFromSportAndSchoolAsync(AddFromSportAndSchoolViewModel model)
        {
            var school = await _repo.Schools.GetByIdAsync(model.SchoolId);
            if (school == null) return;
            var stream = await new AzureStorage(school.ProfileContainer, true).DownloadToStreamAsync(school.ProfileBlob);
            foreach (var sportId in model.SportIds)
            {
                stream.Position = 0;
                var storageItem = await new AzureStorage("teamprofiles").UploadToAzureAsync(stream, $"file_{Guid.NewGuid().ToString()}{Path.GetExtension(school.ProfileBlob)}", MemoryFile.GetContentType(school.ProfileBlob), school.ProfileBlob);
                await AddAsync(new TeamViewModel()
                {
                    SchoolId = model.SchoolId,
                    IsActive = true,
                    Nickname = school.Nickname,
                    FacebookUrl = school.FacebookUrl,
                    TwitterUrl = school.TwitterUrl,
                    InstagramUrl = school.InstagramUrl,
                    PrimaryColor = school.PrimaryColor,
                    SecondaryColor = school.SecondaryColor,
                    SportId = sportId,
                    WebsiteUrl = school.WebsiteUrl,
                    ProfileBlob = storageItem.Blob,
                    ProfileContainer = storageItem.Container,
                    ProfilePublicUrl = storageItem.Url,
                });
            }
        }
    }
}
