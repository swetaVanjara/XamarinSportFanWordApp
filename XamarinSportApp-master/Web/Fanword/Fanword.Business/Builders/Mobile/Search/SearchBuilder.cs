using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.Repository;
using Fanword.Poco.Models;

namespace Fanword.Business.Builders.Mobile.Search
{
    public class SearchBuilder : BaseBuilder
    {
        public SearchBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        public async Task<GlobalSearch> BuildAsync(string userId, string filter)
        {
            var search = new GlobalSearch();
            search.SearchText = filter;
            ConcurrentBag<GlobalSearchItem> bag = new ConcurrentBag<GlobalSearchItem>();
            filter = (filter ?? "").ToLower();

            var t1 = Task.Run(() =>
            {
                AddRange(bag, GetTeams(userId, filter).ToList());
            });
            var t2 = Task.Run(() =>
            {
                AddRange(bag, GetSchools(userId, filter).ToList());
            });
            var t3 = Task.Run(() =>
            {
                AddRange(bag, GetSports(userId, filter).ToList());
            });
            var t4 = Task.Run(() =>
            {
                AddRange(bag, GetContentSources(userId, filter).ToList());
            });
            var t5 = Task.Run(() =>
            {
                AddRange(bag, GetUsers(userId, filter).ToList());
            });

            Task.WaitAll(t1, t2, t3, t4, t5);

            search.Results = bag.ToList();
            if (string.IsNullOrEmpty(filter))
            {
                search.Results = search.Results.OrderByDescending(m => m.Followers).Take(10).ToList();
            }
            return search;
        }

        public async Task<GlobalSearch> BuildAsync(string userId, string filter, FeedType type)
        {
            var search = new GlobalSearch();
            search.SearchText = filter;
            ConcurrentBag<GlobalSearchItem> bag = new ConcurrentBag<GlobalSearchItem>();
            filter = (filter ?? "").ToLower();
            if (type == FeedType.Team)
            {
                AddRange(bag, GetTeams(userId,filter).ToList());
            }
            if (type == FeedType.School)
            {
                AddRange(bag, GetSchools(userId, filter).ToList());
            }
            if (type == FeedType.Sport)
            {
                AddRange(bag, GetSports(userId, filter).ToList());
            }
            if (type == FeedType.ContentSource)
            {
                AddRange(bag, GetContentSources(userId, filter).ToList());
            }
            if (type == FeedType.User)
            {
                AddRange(bag, GetUsers(userId, filter).ToList());
            }
            search.Results = bag.ToList();

            return search;
        }

        IQueryable<GlobalSearchItem> GetTeams(string userId, string filter)
        {
            return ApplicationRepository.CreateWithoutOwin().Teams.Where(m => m.IsActive && (m.Sport.DateDeletedUtc == null && m.School.DateDeletedUtc == null) && (string.IsNullOrEmpty(filter) || m.School.Name.ToLower().Contains(filter) || m.Sport.Name.ToLower().Contains(filter))).Select(m => new GlobalSearchItem()
            {
                Id = m.Id,
                ProfileUrl = m.ProfilePublicUrl,
                Followers = m.Followers.Count(),
                Title = m.School.Name,
                Subtitle = m.Sport.Name,
                Type = FeedType.Team,
                IsFollowing = m.Followers.Select(j => j.Id).Contains(userId)
            });
        }
        IQueryable<GlobalSearchItem> GetSchools(string userId, string filter)
        {
            return ApplicationRepository.CreateWithoutOwin().Schools.Where(m => m.IsActive && (string.IsNullOrEmpty(filter) || m.Name.ToLower().Contains(filter))).Select(m => new GlobalSearchItem()
            {
                Id = m.Id,
                ProfileUrl = m.ProfilePublicUrl,
                Followers = m.Followers.Count(),
                Title = m.Name,
                Type = FeedType.School,
                IsFollowing = m.Followers.Select(j => j.Id).Contains(userId)
            });
        }

        IQueryable<GlobalSearchItem> GetSports(string userId, string filter)
        {
            return ApplicationRepository.CreateWithoutOwin().Sports.Where(m => m.IsActive && (string.IsNullOrEmpty(filter) || m.Name.ToLower().Contains(filter))).Select(m => new GlobalSearchItem()
            {
                Id = m.Id,
                ProfileUrl = m.IconPublicUrl,
                Followers = m.Followers.Count(),
                Title = m.Name,
                Type = FeedType.Sport,
                IsFollowing = m.Followers.Select(j => j.Id).Contains(userId)
            });
        }
        IQueryable<GlobalSearchItem> GetContentSources(string userId, string filter)
        {
            return ApplicationRepository.CreateWithoutOwin().ContentSources.Where(m => m.IsApproved && (string.IsNullOrEmpty(filter) || m.ContentSourceName.ToLower().Contains(filter))).Select(m => new GlobalSearchItem()
            {
                Id = m.Id,
                ProfileUrl = m.LogoUrl,
                Followers = m.Followers.Count(),
                Title = m.ContentSourceName,
                Type = FeedType.ContentSource,
                IsFollowing = m.Followers.Select(j => j.Id).Contains(userId)
            });
        }

        IQueryable<GlobalSearchItem> GetUsers(string userId, string filter)
        {
            return ApplicationRepository.CreateWithoutOwin().Users.Where(m => m.IsActive && (string.IsNullOrEmpty(filter) || (m.FirstName + " " + m.LastName).ToLower().Contains(filter))).Select(m => new GlobalSearchItem()
            {
                Id = m.Id,
                ProfileUrl = m.ProfileUrl,
                Followers = m.Followers.Count(),
                Title = m.FirstName + " " + m.LastName,
                Type = FeedType.User,
                IsFollowing = m.Followers.Select(j => j.Id).Contains(userId)
            });
        }

        void AddRange(ConcurrentBag<GlobalSearchItem> bag, List<GlobalSearchItem> items)
        {
            foreach (var item in items)
            {
                bag.Add(item);
            }
        }
    }
}
