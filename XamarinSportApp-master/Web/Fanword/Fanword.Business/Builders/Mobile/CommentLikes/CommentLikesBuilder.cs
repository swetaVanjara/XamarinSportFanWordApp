using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.Repository;
using Fanword.Poco.Models;
using Notifications.Data.Enum;
using Notifications.Service;
using Fanword.Business.CustomNotification;

namespace Fanword.Business.Builders.Mobile.CommentLikes
{
    public class CommentLikesBuilder : BaseBuilder
    {
        public CommentLikesBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        public async Task UnlikeCommentAsync(string commentId, string userId)
        {
            var comment = await _repo.CommentLikes.FirstOrDefaultAsync(m => m.CommentId == commentId && m.CreatedById == userId);
            if (comment != null)
            {
                await _repo.CommentLikes.DeleteAndSaveAsync(comment);
            }
        }

        public async Task LikeCommentAsync(string commentId, string userId)
        {
            var commentLike = await _repo.CommentLikes.FirstOrDefaultAsync(m => m.CommentId == commentId && m.CreatedById == userId);
            if (commentLike == null)
            {
                commentLike = new Data.Entities.CommentLike();
                commentLike.CommentId = commentId;
                commentLike.CreatedById = userId;
                await _repo.CommentLikes.AddOrUpdateAndSaveAsync(commentLike);

                var comment = await _repo.Comments.FirstOrDefaultAsync(m => m.Id == commentId);
                var me = await _repo.Users.FirstOrDefaultAsync(m => m.Id == userId);


                var metaData = new Dictionary<string, string>();
                metaData.Add(MetaDataKeys.FromId, userId);
                metaData.Add(MetaDataKeys.UserNotificationType, UserNotificationType.CommentLike.ToString());
                metaData.Add(MetaDataKeys.PostId, comment.PostId);
                metaData.Add(MetaDataKeys.CommentId, comment.Id);
                metaData.Add(MetaDataKeys.UserFullName, me.FirstName + " " + me.LastName);

                PushMsgNotificationModel pushMsgNotificationModel = new PushMsgNotificationModel();
                pushMsgNotificationModel.title = me.FirstName + " " + me.LastName + " liked your comment";
                pushMsgNotificationModel.type = NotificationType.User;
                pushMsgNotificationModel.CreatedById = comment.CreatedById;
                pushMsgNotificationModel.metaData = metaData;
                pushMsgNotificationModel.content = "liked your comment";

                new PushMsgNotification().PushMessageAsync(pushMsgNotificationModel);
                new NotificationService(me.FirstName + " " + me.LastName + " liked your comment", NotificationType.User).AddUser(comment.CreatedById).AddMetaData(metaData).AddContent("liked your comment").Send();
            }
        }
    }
}
