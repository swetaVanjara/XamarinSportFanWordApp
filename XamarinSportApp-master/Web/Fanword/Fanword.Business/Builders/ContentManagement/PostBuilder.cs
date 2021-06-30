using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExtensionClasses;
using Fanword.Business.ViewModels.ContentManagement;
using Fanword.Data.Entities;
using Fanword.Data.Repository;
using FileUploads.Azure;

namespace Fanword.Business.Builders.ContentManagement {
    public class PostBuilder {
        private ApplicationRepository _repo { get; set; }

        public PostBuilder(ApplicationRepository repo) {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        private MapperConfiguration MapGridQueries() {
            return new MapperConfiguration(config => {

                config.CreateMap<PostVideo, PostVideoViewModel>();
                config.CreateMap<PostImage, PostImageViewModel>();
                config.CreateMap<PostLink, PostLinkViewModel>();


                config.CreateMap<Post, PostRecord>()
                    .ForMember(m => m.PostSource, m => m.Ignore())
                    .ForMember(m => m.ContentSourceUrl, m => m.Ignore())
                    .ForMember(m => m.PostVideo, m => m.MapFrom(mm => mm.PostVideos.FirstOrDefault()))
                    .ForMember(m => m.PostImage, m => m.MapFrom(mm => mm.PostImages.FirstOrDefault()))
                    .ForMember(m => m.PostLink, m => m.MapFrom(mm => mm.PostLinks.FirstOrDefault()))
                    .ForMember(m => m.CreatedByName, m => m.MapFrom(mm => mm.CreatedBy != null ? mm.CreatedBy.FirstName + " " + mm.CreatedBy.LastName : "System"));
            });
        }

        private MapperConfiguration MapQueries() {
            return new MapperConfiguration(config => {

                config.CreateMap<PostVideo, PostVideoViewModel>();
                config.CreateMap<PostImage, PostImageViewModel>();
                config.CreateMap<PostLink, PostLinkViewModel>();


                config.CreateMap<Post, PostViewModel>()
                    .ForMember(m => m.PostSource, m => m.Ignore())
                    .ForMember(m => m.ContentSourceUrl, m => m.Ignore())
                    .ForMember(m => m.CreatedByName, m => m.MapFrom(mm => mm.CreatedBy != null ? mm.CreatedBy.FirstName + " " + mm.CreatedBy.LastName : "System"))
                    .ForMember(m => m.PostVideo, m => m.MapFrom(mm => mm.PostVideos.FirstOrDefault()))
                    .ForMember(m => m.PostImage, m => m.MapFrom(mm => mm.PostImages.FirstOrDefault()))
                    .ForMember(m => m.PostLink, m => m.MapFrom(mm => mm.PostLinks.FirstOrDefault()));
            });
        }

        public async Task<List<PostRecord>> BuildGridAsync() {
            return (await _repo.Posts.AsQueryable().ProjectTo<PostRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
        }

        public async Task<PostViewModel> BuildSingleAsync(string id) {
            return (await _repo.Posts.GetByIdQueryable(id).ProjectTo<PostViewModel>(MapQueries()).FirstOrDefaultAsync()).SpecifyDateTimeKind();
        }


        public async Task UpdateAsync(PostViewModel model) {
            var dbPost = await _repo.Posts.GetByIdAsync(model.Id);
            if (model.RemoveContentSource) {
                if (_repo.PostImages.AsQueryable().Any(i=>i.PostId==model.Id)) {
                    if (!String.IsNullOrEmpty(model.PostImage.Container)) { //null from rss feeds
                        new AzureStorage(model.PostImage.Container).DeleteFile(model.PostImage.Blob);
                    }
                    await _repo.PostImages.DeleteAndSaveAsync(_repo.PostImages.FirstOrDefault(i=>i.PostId==model.Id)?.Id);
                }

                if (_repo.PostVideos.AsQueryable().Any(i => i.PostId == model.Id)) {
                    if (!String.IsNullOrEmpty(model.PostVideo.Container)) { //null from rss feeds
                        new AzureStorage(model.PostVideo.Container).DeleteFile(model.PostVideo.Blob);
                    }
                    await _repo.PostVideos.DeleteAndSaveAsync(_repo.PostVideos.FirstOrDefault(i => i.PostId == model.Id)?.Id);
                }

                if (_repo.PostLinks.AsQueryable().Any(i => i.PostId == model.Id)) {
                   await _repo.PostLinks.DeleteAndSaveAsync(_repo.PostLinks.FirstOrDefault(i => i.PostId == model.Id)?.Id);
                }
            }

            dbPost.Content = model.Content;
            await _repo.SaveAsync();
            //update link title/content
            var dbLink =await  _repo.PostLinks.GetByIdAsync(model.PostLink?.Id);
            if (dbLink != null) {
                dbLink.Title = model.PostLink?.Title;
                dbLink.Content = model.PostLink?.Content;
                await _repo.SaveAsync();
            }
        }



    }
}
