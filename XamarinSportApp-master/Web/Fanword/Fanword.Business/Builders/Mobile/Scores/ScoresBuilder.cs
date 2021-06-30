using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.Enums;
using Fanword.Data.Repository;
using Fanword.Poco.Models;
using Newtonsoft.Json;

namespace Fanword.Business.Builders.Mobile.Scores
{
    public class ScoresBuilder : BaseBuilder
    {
        public ScoresBuilder(ApplicationRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ScoreModel>> BuildAsync(string userId, ScoresFilterModel filter)
        {
            var sportFollows = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Sports).Select(m => m.Id);
            var schoolFollows = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Schools).Select(m => m.Id);
            var teamFollows = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Teams).Select(m => m.Id);
            
            var query1 = _repo.Events.AsQueryable();

            if (filter.FollowFilter.MySchools || filter.FollowFilter.MySports || filter.FollowFilter.MyTeams)
            {
                query1 = query1.Where(m =>
                    (filter.FollowFilter.MySports && sportFollows.Contains(m.SportId)) ||
                    (filter.FollowFilter.MyTeams && teamFollows.Any(j => m.EventTeams.Any(k => k.TeamId == j))) ||
                    (filter.FollowFilter.MySchools && schoolFollows.Any(j => m.EventTeams.Any(k => k.Team.SchoolId == j))));
            }
            var utcFromLocalMidnight = filter.Today;
            var utcFromLocalMidnightPlus24Hours = filter.Today.AddHours(24);

            if (filter.DateFilter == DateFilter.Past)
            {
                query1 = query1.OrderByDescending(m => m.DateOfEventUtc).Where(m => m.DateOfEventUtc < utcFromLocalMidnight);
            }
            else if (filter.DateFilter == DateFilter.Today)
            {
                query1 = query1.OrderBy(m => m.DateOfEventUtc).Where(m => m.DateOfEventUtc >= utcFromLocalMidnight && m.DateOfEventUtc < utcFromLocalMidnightPlus24Hours);
            }
            else if (filter.DateFilter == DateFilter.Upcoming)
            {
                query1 = query1.OrderBy(m => m.DateOfEventUtc).Where(m => m.DateOfEventUtc >= utcFromLocalMidnightPlus24Hours);
            }

            if (!string.IsNullOrEmpty(filter.TeamId))
            {
                query1 = query1.Where(m => m.EventTeams.Any(j => j.TeamId == filter.TeamId));
            }
            if (!string.IsNullOrEmpty(filter.SchoolId))
            {
                query1 = query1.Where(m => m.EventTeams.Any(j => j.Team.SchoolId == filter.SchoolId));
            }
            if (!string.IsNullOrEmpty(filter.SportId))
            {
                query1 = query1.Where(m => m.EventTeams.Any(j => j.Team.SportId == filter.SportId));
            }

            var query2 = query1.Select(m => new ScoreModel()
            {
                SportName = m.Sport.Name,
                SportProfileUrl = m.Sport.IconPublicUrl,
                EventDate = m.DateOfEventUtc,
                EventId = m.Id,
                EventName = m.Name,
                PostCount = m.Posts.Count,
                Team1Score = m.EventTeams.OrderBy(j => j.DateCreatedUtc).Select(j => j.ScorePointsOrPlace).FirstOrDefault(),
                Team2Score = m.EventTeams.OrderByDescending(j => j.DateCreatedUtc).Select(j => j.ScorePointsOrPlace).FirstOrDefault(),
                Team1Name = m.EventTeams.OrderBy(j => j.DateCreatedUtc).Select(j => j.Team.School.Name).FirstOrDefault(),
                Team2Name = m.EventTeams.OrderByDescending(j => j.DateCreatedUtc).Select(j => j.Team.School.Name).FirstOrDefault(),
                Team1Url = m.EventTeams.OrderBy(j => j.DateCreatedUtc).Select(j => j.Team.ProfilePublicUrl).FirstOrDefault(),
                Team2Url = m.EventTeams.OrderByDescending(j => j.DateCreatedUtc).Select(j => j.Team.ProfilePublicUrl).FirstOrDefault(),
                TeamCount = m.EventTeams.Count,
                TicketUrl = m.PurchaseTicketsUrl,
                TimezoneId = m.TimezoneId,
            });

            var items = await query2.ToListAsync();

            // Can't do this inside a queryable
            foreach (var scoreModel in items)
            {
                var date = TimeZoneInfo.ConvertTimeFromUtc(scoreModel.EventDate, TimeZoneInfo.FindSystemTimeZoneById(scoreModel.TimezoneId));
                scoreModel.IsTbd = date.Hour == 0;
            }

            return items;
        }
    }
}
