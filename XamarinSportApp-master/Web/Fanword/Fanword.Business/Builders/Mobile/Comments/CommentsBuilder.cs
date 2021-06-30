using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExtensionClasses.AutoMapper.Mappers;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.Entities;
using Fanword.Data.IdentityConfig.User;
using Fanword.Data.Repository;
using Fanword.Poco.Models;
using Notifications.Data.Enum;
using Notifications.Service;
using Comment = Fanword.Poco.Models.Comment;
using Fanword.Business.CustomNotification;

namespace Fanword.Business.Builders.Mobile.Comments
{
    public class CommentsBuilder : BaseBuilder
    {
        public MapperConfiguration MapObjects(string userId)
        {
            MapperConfiguration config = new MapperConfiguration(j =>
            j.CreateMap<Data.Entities.Comment, Comment>()
                .ForMember(m => m.Username, m => m.MapFrom(mm => mm.CreatedBy.FirstName + " " + mm.CreatedBy.LastName))
                .ForMember(m => m.ProfileUrl, m => m.MapFrom(mm => mm.CreatedBy.ProfileUrl))
                .ForMember(m => m.LikeCount, m => m.MapFrom(mm => mm.Likes.Count))
                .ForMember(m => m.ReplyCount, m => m.MapFrom(mm => _repo.Context.Comments.Count(k => k.ParentCommentId == mm.Id)))
                .ForMember(m => m.RepliedToUsername, m => m.MapFrom(mm => mm.ParentComment.CreatedBy.FirstName + " " + mm.ParentComment.CreatedBy.LastName))
                .ForMember(m => m.IsLiked, m => m.MapFrom(mm => mm.Likes.Any(k => k.CreatedById == userId))));
            return config;
        }
        public CommentsBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        public async Task<List<Comment>> BuildAsync(string postId, string userId)
        {
            var config = MapObjects(userId);
            return await _repo.Comments.Where(m => m.PostId == postId).OrderBy(m => m.DateCreatedUtc).ProjectTo<Comment>(config).ToListAsync();
        }

        public async Task SaveCommentAsync(Comment comment, string userId)
        {
            var dbComment = comment.Map<Data.Entities.Comment>();
            dbComment.CreatedById = userId;
            var me = await _repo.Users.FirstOrDefaultAsync(m => m.Id == userId);
            await _repo.Comments.AddOrUpdateAndSaveAsync(dbComment);
            
            var post = _repo.Posts.FirstOrDefault(m => m.Id == dbComment.PostId, posts => posts.Include(j => j.CreatedBy));

            if (string.IsNullOrEmpty(post.ContentSourceId + post.TeamId + post.SchoolId + post.FeedId))
            {
                var metaData = new Dictionary<string, string>();
                metaData.Add(MetaDataKeys.FromId, userId);
                metaData.Add(MetaDataKeys.UserNotificationType, UserNotificationType.Comment.ToString());
                metaData.Add(MetaDataKeys.PostId, dbComment.PostId);
                metaData.Add(MetaDataKeys.UserFullName, post.CreatedBy.FirstName + " " + post.CreatedBy.LastName);

                PushMsgNotificationModel pushMsgNotificationModel = new PushMsgNotificationModel();
                pushMsgNotificationModel.title = me.FirstName + " " + me.LastName + " commented on your post";
                pushMsgNotificationModel.type = NotificationType.User;
                pushMsgNotificationModel.CreatedById = post.CreatedById;
                pushMsgNotificationModel.metaData = metaData;
                pushMsgNotificationModel.content = "commented on your post";

                new PushMsgNotification().PushMessageAsync(pushMsgNotificationModel);

                new NotificationService(me.FirstName + " " + me.LastName + " commented on your post", NotificationType.User).AddUser(post.CreatedById).AddMetaData(metaData).AddContent("commented on your post").Send();

                if (!string.IsNullOrEmpty(dbComment.ParentCommentId))
                {
                    var parentComment = await _repo.Comments.FirstOrDefaultAsync(m => m.Id == dbComment.ParentCommentId);

                    var metaDataReply = new Dictionary<string, string>();
                    metaDataReply.Add(MetaDataKeys.FromId, userId);
                    metaDataReply.Add(MetaDataKeys.UserNotificationType, UserNotificationType.Comment.ToString());
                    metaDataReply.Add(MetaDataKeys.PostId, dbComment.PostId);
                    metaDataReply.Add(MetaDataKeys.CommentId, dbComment.Id);
                    metaDataReply.Add(MetaDataKeys.UserFullName, post.CreatedBy.FirstName + " " + post.CreatedBy.LastName);


                    PushMsgNotificationModel parentPushMsgNotificationModel = new PushMsgNotificationModel();
                    parentPushMsgNotificationModel.title = me.FirstName + " " + me.LastName + " replied to your comment";
                    parentPushMsgNotificationModel.type = NotificationType.User;
                    parentPushMsgNotificationModel.CreatedById = parentComment.CreatedById;
                    parentPushMsgNotificationModel.metaData = metaDataReply;
                    parentPushMsgNotificationModel.content = "replied to your comment";

                    new PushMsgNotification().PushMessageAsync(pushMsgNotificationModel);
                    new NotificationService(me.FirstName + " " + me.LastName + " replied to your comment", NotificationType.User).AddUser(parentComment.CreatedById).AddMetaData(metaDataReply).AddContent("replied to your comment").Send();
                }
            }
        }
    }
}
