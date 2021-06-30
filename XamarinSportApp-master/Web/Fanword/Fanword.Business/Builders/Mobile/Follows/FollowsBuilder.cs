using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.Repository;
using Fanword.Poco.Models;
using Notifications.Data.Enum;
using Notifications.Service;
using Fanword.Business.CustomNotification;

namespace Fanword.Business.Builders.Mobile.Follows
{
    public class FollowsBuilder : BaseBuilder
    {
        public FollowsBuilder(ApplicationRepository repo)
        {
            _repo = repo;
        }

        public async Task FollowTeamAsync(string userId, string teamId)
        {
            var me = await _repo.Users.FirstOrDefaultAsync(m => m.Id == userId, query => query.Include(m => m.Teams));
            var team = await _repo.Teams.FirstOrDefaultAsync(m => m.Id == teamId);
            if (me.Teams.All(m => m.Id != team.Id))
            {
                me.Teams.Add(team);
                await _repo.Users.AddOrUpdateAndSaveAsync(me);
            }
        }

        public async Task UnfollowTeamAsync(string userId, string teamId)
        {
            var me = await _repo.Users.FirstOrDefaultAsync(m => m.Id == userId, query => query.Include(m => m.Teams));
            var team = await _repo.Teams.FirstOrDefaultAsync(m => m.Id == teamId);
            if (me.Teams.Any(m => m.Id == team.Id))
            {
                me.Teams.Remove(team);
                await _repo.Users.AddOrUpdateAndSaveAsync(me);
            }
        }

        public async Task FollowUserAsync(string userId, string followedUserId)
        {
            var me = await _repo.Users.FirstOrDefaultAsync(m => m.Id == userId, query => query.Include(m => m.Users));
            var user = await _repo.Users.FirstOrDefaultAsync(m => m.Id == followedUserId);
            if (me.Users.All(m => m.Id != user.Id))
            {
                me.Users.Add(user);
                await _repo.Users.AddOrUpdateAndSaveAsync(me);

                var metaData = new Dictionary<string, string>();
                metaData.Add(MetaDataKeys.FromId, userId);
                metaData.Add(MetaDataKeys.UserNotificationType, UserNotificationType.Follow.ToString());
                metaData.Add(MetaDataKeys.UserFullName, me.FirstName + " " + me.LastName);


                PushMsgNotificationModel pushMsgNotificationModel = new PushMsgNotificationModel();
                pushMsgNotificationModel.title = me.FirstName + " " + me.LastName + " is following you";
                pushMsgNotificationModel.type = NotificationType.User;
                pushMsgNotificationModel.CreatedById = followedUserId;
                pushMsgNotificationModel.metaData = metaData;
                pushMsgNotificationModel.content = "is following you";

               new PushMsgNotification().PushMessageAsync(pushMsgNotificationModel);
                new NotificationService(me.FirstName + " " + me.LastName + " is following you", NotificationType.User).AddUser(followedUserId).AddMetaData(metaData).AddContent("is following you").Send();
            }
        }

        public async Task UnfollowUserAsync(string userId, string followedUserId)
        {
            var me = await _repo.Users.FirstOrDefaultAsync(m => m.Id == userId, query => query.Include(m => m.Users));
            var user = await _repo.Users.FirstOrDefaultAsync(m => m.Id == followedUserId);
            if (me.Users.Any(m => m.Id == user.Id))
            {
                me.Users.Remove(user);
                await _repo.Users.AddOrUpdateAndSaveAsync(me);
            }
        }


        public async Task FollowContentSourceAsync(string userId, string contentSourceId)
        {
            var me = await _repo.Users.FirstOrDefaultAsync(m => m.Id == userId, query => query.Include(m => m.ContentSources));
            var source = await _repo.ContentSources.FirstOrDefaultAsync(m => m.Id == contentSourceId);
            if (me.ContentSources.All(m => m.Id != source.Id))
            {
                me.ContentSources.Add(source);
                await _repo.Users.AddOrUpdateAndSaveAsync(me);
            }
        }

        public async Task UnfollowContentSourceAsync(string userId, string contentSourceId)
        {
            var me = await _repo.Users.FirstOrDefaultAsync(m => m.Id == userId, query => query.Include(m => m.ContentSources));
            var source = await _repo.ContentSources.FirstOrDefaultAsync(m => m.Id == contentSourceId);
            if (me.ContentSources.Any(m => m.Id == source.Id))
            {
                me.ContentSources.Remove(source);
                await _repo.Users.AddOrUpdateAndSaveAsync(me);
            }
        }

        public async Task FollowSportAsync(string userId, string sportId)
        {
            var me = await _repo.Users.FirstOrDefaultAsync(m => m.Id == userId, query => query.Include(m => m.Sports));
            var sport = await _repo.Sports.FirstOrDefaultAsync(m => m.Id == sportId);
            if (me.Sports.All(m => m.Id != sport.Id))
            {
                me.Sports.Add(sport);
                await _repo.Users.AddOrUpdateAndSaveAsync(me);
            }
        }

        public async Task UnfollowSportAsync(string userId, string sportId)
        {
            var me = await _repo.Users.FirstOrDefaultAsync(m => m.Id == userId, query => query.Include(m => m.Sports));
            var sport = await _repo.Sports.FirstOrDefaultAsync(m => m.Id == sportId);
            if (me.Sports.Any(m => m.Id == sport.Id))
            {
                me.Sports.Remove(sport);
                await _repo.Users.AddOrUpdateAndSaveAsync(me);
            }
        }

        public async Task FollowSchoolAsync(string userId, string schoolId)
        {
            var me = await _repo.Users.FirstOrDefaultAsync(m => m.Id == userId, query => query.Include(m => m.Schools));
            var school = await _repo.Schools.FirstOrDefaultAsync(m => m.Id == schoolId);
            if (me.Schools.All(m => m.Id != school.Id))
            {
                me.Schools.Add(school);
                await _repo.Users.AddOrUpdateAndSaveAsync(me);
            }
        }

        public async Task UnfollowSchoolAsync(string userId, string schoolId)
        {
            var me = await _repo.Users.FirstOrDefaultAsync(m => m.Id == userId, query => query.Include(m => m.Schools));
            var school = await _repo.Schools.FirstOrDefaultAsync(m => m.Id == schoolId);
            if (me.Schools.Any(m => m.Id == school.Id))
            {
                me.Schools.Remove(school);
                await _repo.Users.AddOrUpdateAndSaveAsync(me);
            }
        }
    }
}
