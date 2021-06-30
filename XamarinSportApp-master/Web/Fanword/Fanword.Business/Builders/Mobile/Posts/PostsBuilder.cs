using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ExtensionClasses.AutoMapper.Mappers;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Business.Builders.Mobile.PostShares;
using Fanword.Data.Entities;
using Fanword.Data.Repository;
using Newtonsoft.Json;
using Post = Fanword.Poco.Models.Post;
using PostImage = Fanword.Poco.Models.PostImage;
using PostLink = Fanword.Poco.Models.PostLink;
using PostVideo = Fanword.Poco.Models.PostVideo;

namespace Fanword.Business.Builders.Mobile.Posts
{
    public class PostsBuilder : BaseBuilder
    {
        public PostsBuilder(ApplicationRepository repo)
        {
            _repo = repo;
        }

        public async Task<Post> BuildAsync(string postId)
        {
            var post = await _repo.Posts.Where(m => m.Id == postId).Select(m => new Post()
            {
                Id = m.Id,
                Content = m.Content,
                ContentSourceId = m.ContentSourceId,
                TeamId = m.TeamId,
                SchoolId = m.SchoolId,
                Images = m.PostImages.Select(j => new PostImage()
                {
                    Id = j.Id,
                    Blob = j.Blob,
                    Container = j.Container,
                    Url = j.Url,
                    ImageAspectRatio = j.ImageAspectRatio
                }).ToList(),
                Videos = m.PostVideos.Select(j => new PostVideo()
                {
                    Id = j.Id,
                    Blob = j.Blob,
                    Container = j.Container,
                    Url = j.Url,
                    ImageUrl = j.ImageUrl,
                    ImageBlob = j.ImageBlob,
                    ImageContainer = j.ImageContainer,
                    ImageAspectRatio = j.ImageAspectRatio
                }).ToList(),
                Links = m.PostLinks.Select(j => new PostLink()
                {
                    Id = j.Id,
                    Content = j.Content,
                    ImageUrl = j.ImageUrl,
                    LinkUrl = j.LinkUrl,
                    Title = j.Title,
                    ImageAspectRatio = j.ImageAspectRatio
                }).ToList(),
                Schools = m.Schools.Select(j => j.Id).ToList(),
                Teams = m.Teams.Select(j => j.Id).ToList(),
                Sports = m.Sports.Select(j => j.Id).ToList(),
                Events = m.Events.Select(j => j.Id).ToList(),
            }).FirstOrDefaultAsync();

            return post;
        }

        public async Task SaveAsync(Post post, string userId)
        {
            var existingDbPost = _repo.Posts.GetById(post.Id, m => m.Include(j => j.PostLinks).Include(j => j.PostImages).Include(j => j.PostVideos).Include(j => j.Schools).Include(j => j.Sports).Include(j => j.Teams).Include(j => j.Events));
            if (existingDbPost == null)
            {
                existingDbPost = new Data.Entities.Post();
                existingDbPost.Id = Guid.NewGuid().ToString();
                existingDbPost.CreatedById = userId;
            }
            else
            {
                foreach (var link in existingDbPost.PostLinks.ToList())
                {
                    _repo.PostLinks.Delete(link.Id);
                }
                foreach (var image in existingDbPost.PostImages.ToList())
                {
                    _repo.PostImages.Delete(image.Id);
                }
                foreach (var video in existingDbPost.PostVideos.ToList())
                {
                    _repo.PostVideos.Delete(video.Id);
                }
                await _repo.SaveAsync();
            }
            
            // Update post
            existingDbPost.Content = post.Content;
            existingDbPost.ContentSourceId = post.ContentSourceId;
            existingDbPost.TeamId = post.TeamId;
            existingDbPost.SchoolId = post.SchoolId;
            existingDbPost.SharedFromPostId = post.SharedPostId;

            existingDbPost.PostImages = post.Images.Map<List<Data.Entities.PostImage>>(expression =>
                expression.CreateMap<PostImage, Data.Entities.PostImage>()
                    .ForMember(m => m.PostId, m => m.UseValue(existingDbPost.Id))
                    .ForMember(m => m.Id, m => m.MapFrom(mm => string.IsNullOrEmpty(mm.Id) ? Guid.NewGuid().ToString() : mm.Id))
                    .ForMember(m => m.DateCreatedUtc, m => m.UseValue(DateTime.UtcNow))
            );
            existingDbPost.PostVideos = post.Videos.Map<List<Data.Entities.PostVideo>>(expression =>
                expression.CreateMap<PostVideo, Data.Entities.PostVideo>()
                    .ForMember(m => m.PostId, m => m.UseValue(existingDbPost.Id))
                    .ForMember(m => m.Id, m => m.MapFrom(mm => string.IsNullOrEmpty(mm.Id) ? Guid.NewGuid().ToString() : mm.Id))
                    .ForMember(m => m.DateCreatedUtc, m => m.UseValue(DateTime.UtcNow))
            );
            existingDbPost.PostLinks = post.Links.Map<List<Data.Entities.PostLink>>(expression =>
                expression.CreateMap<PostLink, Data.Entities.PostLink>()
                    .ForMember(m => m.PostId, m => m.UseValue(existingDbPost.Id))
                    .ForMember(m => m.Id, m => m.MapFrom(mm => string.IsNullOrEmpty(mm.Id) ? Guid.NewGuid().ToString() : mm.Id))
                    .ForMember(m => m.DateCreatedUtc, m => m.UseValue(DateTime.UtcNow))
            );

            existingDbPost.Schools = _repo.Schools.Where(m => post.Schools.Contains(m.Id)).ToList();
            existingDbPost.Teams = _repo.Teams.Where(m => post.Teams.Contains(m.Id)).ToList();
            existingDbPost.Sports = _repo.Sports.Where(m => post.Sports.Contains(m.Id)).ToList();
            existingDbPost.Events = _repo.Events.Where(m => post.Events.Contains(m.Id)).ToList();
   
            await _repo.Posts.AddOrUpdateAndSaveAsync(existingDbPost);

            if (string.IsNullOrEmpty(post.Id) && post.IsShared)
            {
                await new PostSharesBuilder(_repo).SaveShareAsync(existingDbPost.Id, existingDbPost.CreatedById);
            }
        }

        public async Task CloneAsync(string postId, string userId)
        {
            var post = await BuildAsync(postId);

            // Save that the post was shared
            await new PostSharesBuilder(_repo).SaveShareAsync(post.Id, userId);

            foreach (var image in post.Images)
            {
                image.Id = null;
            }
            foreach (var link in post.Links)
            {
                link.Id = null;
            }
            foreach (var video in post.Videos)
            {
                video.Id = null;
            }
            post.SharedPostId = postId;
            post.Id = null;
            post.ContentSourceId = null;
            post.TeamId = null;
            post.SchoolId = null;

            await SaveAsync(post, userId);

        }
    }
}

