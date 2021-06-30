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

namespace Fanword.Business.Builders.Mobile.PostShares
{
    public class PostSharesBuilder : BaseBuilder
    {
        public PostSharesBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        public async Task<List<PostShare>> BuildAsync(string postId)
        {
            var post = _repo.Posts.Where(m => m.Id == postId);
            return await post.SelectMany(m => m.Shares).Select(m => new PostShare() { UserId = m.CreatedById, ProfileUrl = m.CreatedBy.ProfileUrl, FirstName = m.CreatedBy.FirstName, LastName = m.CreatedBy.LastName }).ToListAsync();
        }

        public async Task SaveShareAsync(string postId, string userId)
        {
            var share = new Data.Entities.PostShare();
            share.CreatedById = userId;
            share.PostId = postId;
            var existing = _repo.PostShares.FirstOrDefault(m => m.PostId == postId && m.CreatedById == userId);

            if(existing != null)
                return;
            
            await _repo.PostShares.AddOrUpdateAndSaveAsync(share);

            var me = await _repo.Users.FirstOrDefaultAsync(m => m.Id == userId);
            var createdById = await _repo.Posts.Where(m => m.Id == postId).Select(m => m.CreatedById).FirstOrDefaultAsync();

            var metaData = new Dictionary<string, string>();
            metaData.Add(MetaDataKeys.FromId, userId);
            metaData.Add(MetaDataKeys.UserNotificationType, UserNotificationType.Share.ToString());
            metaData.Add(MetaDataKeys.PostId, postId);
            metaData.Add(MetaDataKeys.UserFullName, me.FirstName + " " + me.LastName);


            PushMsgNotificationModel pushMsgNotificationModel = new PushMsgNotificationModel();
            pushMsgNotificationModel.title = me.FirstName + " " + me.LastName + " shared your post";
            pushMsgNotificationModel.type = NotificationType.User;
            pushMsgNotificationModel.CreatedById = createdById;
            pushMsgNotificationModel.metaData = metaData;
            pushMsgNotificationModel.content = "shared your post";

            new PushMsgNotification().PushMessageAsync(pushMsgNotificationModel);

            new NotificationService(me.FirstName + " " + me.LastName + " shared your post", NotificationType.User).AddUser(createdById).AddMetaData(metaData).AddContent("shared your post").Send();
        }
    }
}
