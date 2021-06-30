using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExtensionClasses;
using Fanword.Business.ViewModels.Likes;
using Fanword.Data.Repository;

namespace Fanword.Business.Builders.Likes {
    public class LikeBuilder {
        private ApplicationRepository _repo { get; set; }

        public LikeBuilder(ApplicationRepository repo) {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        private MapperConfiguration MapGridQueries() {
            return new MapperConfiguration(config => {
                config.CreateMap<Data.Entities.PostLike, LikeRecord>()
                    .ForMember(m => m.LikedItem, m => m.UseValue("Post"))
                    .ForMember(m => m.CreatedByName, m => m.MapFrom(mm => mm.CreatedBy.FirstName + " " + mm.CreatedBy.LastName));
                config.CreateMap<Data.Entities.CommentLike, LikeRecord>()
                    .ForMember(m => m.LikedItem, m => m.UseValue("Comment"))
                    .ForMember(m => m.CreatedByName, m => m.MapFrom(mm => mm.CreatedBy.FirstName + " " + mm.CreatedBy.LastName));
            });
        }

        public async Task<List<LikeRecord>> BuildGridAsync() {
            var commentQuery = await _repo.CommentLikes.AsQueryable().ProjectTo<LikeRecord>(MapGridQueries()).ToListAsync();
            var postQuery = await _repo.PostLikes.AsQueryable().ProjectTo<LikeRecord>(MapGridQueries()).ToListAsync();
            commentQuery.AddRange(postQuery);
            return commentQuery.OrderByDescending(i=>i.DateCreatedUtc).ToList().SpecifyDateTimeKind();
        }
    }
}
