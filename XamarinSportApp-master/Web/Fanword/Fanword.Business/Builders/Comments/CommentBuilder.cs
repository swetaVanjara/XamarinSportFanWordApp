using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExtensionClasses;
using Fanword.Business.ViewModels.Comments;
using Fanword.Data.Repository;

namespace Fanword.Business.Builders.Comments {
    public class CommentBuilder {
        private ApplicationRepository _repo { get; set; }

        public CommentBuilder(ApplicationRepository repo) {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        private MapperConfiguration MapGridQueries() {
            return new MapperConfiguration(config => {
                config.CreateMap<Data.Entities.Comment, CommentRecord>()
                    .ForMember(m=>m.CreatedByName,m=>m.MapFrom(mm=>mm.CreatedBy.FirstName + " " + mm.CreatedBy.LastName));
            });
        }

        public async Task<List<CommentRecord>> BuildGridAsync() {
            return (await _repo.Comments.AsQueryable().OrderByDescending(i=>i.DateCreatedUtc).ProjectTo<CommentRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
        }
    }
}
