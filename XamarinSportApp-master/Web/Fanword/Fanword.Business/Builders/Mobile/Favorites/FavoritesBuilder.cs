using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.Repository;
using Fanword.Poco.Models;

namespace Fanword.Business.Builders.Mobile.Favorites
{
    public class FavoritesBuilder : BaseBuilder
    {
        public FavoritesBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        public async Task<Poco.Models.Favorites> BuildAsync(string userId)
        {
            var favorites = new Poco.Models.Favorites();

            List<Task> tasks = new List<Task>();
            var t1 = Task.Run(() =>
            {
                favorites.Teams = GetTeams(userId).ToList();
            });
            var t2 = Task.Run(() =>
            {
                favorites.Schools = GetSchools(userId).ToList();
            });
            var t3 = Task.Run(() =>
            {
                favorites.Sports = GetSports(userId).ToList();
            });
            var t4 = Task.Run(() =>
            {
                favorites.ContentSources = GetContentSources(userId).ToList();
            });
            var t5 = Task.Run(() =>
            {
                favorites.Following = GetUsers(userId).ToList();
            });
            var t6 = Task.Run(() =>
            {
                favorites.Followers = GetFollowers(userId).ToList();
            });

            Task.WaitAll(t1,t2,t3,t4,t5,t6);
            
            return favorites;
        }

        IQueryable<FavoriteItem> GetTeams(string userId)
        {
            return ApplicationRepository.CreateWithoutOwin().Users.Where(m => m.Id == userId).SelectMany(m => m.Teams).Where(m => m.IsActive && m.DateDeletedUtc == null).Select(m => new FavoriteItem()
            {
                Id = m.Id,
                ProfileUrl = m.ProfilePublicUrl,
                Title = m.School.Name,
                Subtitle = m.Sport.Name,
                Type = FeedType.Team,
                IsFollowing = m.Followers.Select(j => j.Id).Contains(userId)
            });
        }
        IQueryable<FavoriteItem> GetSchools(string userId)
        {
            return ApplicationRepository.CreateWithoutOwin().Users.Where(m => m.Id == userId).SelectMany(m => m.Schools).Where(m => m.IsActive && m.DateDeletedUtc == null).Select(m => new FavoriteItem()
            {
                Id = m.Id,
                ProfileUrl = m.ProfilePublicUrl,
                Title = m.Name,
                Type = FeedType.School,
                IsFollowing = m.Followers.Select(j => j.Id).Contains(userId)
            });
        }

        IQueryable<FavoriteItem> GetSports(string userId)
        {
            return ApplicationRepository.CreateWithoutOwin().Users.Where(m => m.Id == userId).SelectMany(m => m.Sports).Where(m => m.IsActive && m.DateDeletedUtc == null).Select(m => new FavoriteItem()
            {
                Id = m.Id,
                ProfileUrl = m.IconPublicUrl,
                Title = m.Name,
                Type = FeedType.Sport,
                IsFollowing = m.Followers.Select(j => j.Id).Contains(userId)
            });
        }
        IQueryable<FavoriteItem> GetContentSources(string userId)
        {
            return ApplicationRepository.CreateWithoutOwin().Users.Where(m => m.Id == userId).SelectMany(m => m.ContentSources).Where(m => m.IsApproved).Select(m => new FavoriteItem()
            {
                Id = m.Id,
                ProfileUrl = m.LogoUrl,
                Title = m.ContentSourceName,
                Type = FeedType.ContentSource,
                IsFollowing = m.Followers.Select(j => j.Id).Contains(userId)
            });
        }

        IQueryable<FavoriteItem> GetUsers(string userId)
        {
            // what i'm following
            return ApplicationRepository.CreateWithoutOwin().Users.Where(m => m.Followers.Where(j => j.IsActive).Select(j => j.Id).Contains(userId)).Select(m => new FavoriteItem()
            {
                Id = m.Id,
                ProfileUrl = m.ProfileUrl,
                Title = m.FirstName + " " + m.LastName,
                Type = FeedType.User,
                IsFollowing = m.Followers.Select(j => j.Id).Contains(userId)
            });
        }

        IQueryable<FavoriteItem> GetFollowers(string userId)
        {
            return ApplicationRepository.CreateWithoutOwin().Users.Where(m => m.Id == userId).SelectMany(m => m.Followers.Where(j => j.IsActive)).Select(m => new FavoriteItem()
            {
                Id = m.Id,
                ProfileUrl = m.ProfileUrl,
                Title = m.FirstName + " " + m.LastName,
                Type = FeedType.User,
                IsFollowing = m.Followers.Select(j => j.Id).Contains(userId)
            });
        }
    }
}
