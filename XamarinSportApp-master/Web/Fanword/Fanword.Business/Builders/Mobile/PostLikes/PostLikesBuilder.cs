using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.Context;
using Fanword.Data.Entities;
using Fanword.Data.Repository;
using Fanword.Poco.Models;
using Notifications.Data.Enum;
using Notifications.Service;
using Fanword.Business.CustomNotification;

using PostLike = Fanword.Poco.Models.PostLike;

namespace Fanword.Business.Builders.Mobile.Likes
{
    public class PostLikesBuilder : BaseBuilder
    {
        public PostLikesBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        public async Task<List<PostLike>> BuildAsync(string postId)
        {
            MapperConfiguration config = new MapperConfiguration(j =>
                j.CreateMap<Data.Entities.PostLike, PostLike>()
                    .ForMember(m => m.Username, m => m.MapFrom(mm => mm.CreatedBy.FirstName + " " + mm.CreatedBy.LastName))
                    .ForMember(m => m.ProfileUrl, m => m.MapFrom(mm => mm.CreatedBy.ProfileUrl)));


            return await _repo.PostLikes.Where(m => m.PostId == postId).OrderByDescending(m => m.DateCreatedUtc).ProjectTo<PostLike>(config).ToListAsync();
        }

        public async Task UnlikePostAsync(string postId, string userId)
        {
            var like = await _repo.PostLikes.FirstOrDefaultAsync(m => m.PostId == postId && m.CreatedById == userId);
            if (like != null)
            {
                await _repo.PostLikes.DeleteAndSaveAsync(like);
            }
        }

        public async Task LikePostAsync(string postId, string userId)
        {
            var like = await _repo.PostLikes.FirstOrDefaultAsync(m => m.PostId == postId && m.CreatedById == userId);

            var CreatedPostUserData = await _repo.Posts.FirstOrDefaultAsync(m => m.Id == postId);

            if (like == null)
            {
                like = new Data.Entities.PostLike();
                like.PostId = postId;
                like.CreatedById = userId;
               
               
                await _repo.PostLikes.AddOrUpdateAndSaveAsync(like);
                var me = await _repo.Users.FirstOrDefaultAsync(m => m.Id == userId);

                var post = _repo.Posts.FirstOrDefault(m => m.Id == postId, posts => posts.Include(j => j.CreatedBy));
                
                if (string.IsNullOrEmpty(post.ContentSourceId + post.TeamId + post.SchoolId + post.FeedId))
                {
                    var metaData = new Dictionary<string, string>();
                    metaData.Add(MetaDataKeys.FromId, userId);
                    metaData.Add(MetaDataKeys.UserNotificationType, UserNotificationType.Like.ToString());
                    metaData.Add(MetaDataKeys.PostId, postId);
                    metaData.Add(MetaDataKeys.UserFullName, post.CreatedBy.FirstName + " " + post.CreatedBy.LastName);
                    metaData.Add(MetaDataKeys.PostUserID, CreatedPostUserData.CreatedById);


                    PushMsgNotificationModel pushMsgNotificationModel = new PushMsgNotificationModel();
                    pushMsgNotificationModel.title = me.FirstName + " " + me.LastName + " liked your post";
                    pushMsgNotificationModel.type = NotificationType.User;
                    pushMsgNotificationModel.CreatedById = post.CreatedById;
                    pushMsgNotificationModel.metaData = metaData;
                    pushMsgNotificationModel.content = "liked your post";

                    new PushMsgNotification().PushMessageAsync(pushMsgNotificationModel);
                    new NotificationService(me.FirstName + " " + me.LastName + " liked your post", NotificationType.User).AddUser(post.CreatedById).AddMetaData(metaData).AddContent("liked your post").Send();
                }
            }
        }
    }
}
