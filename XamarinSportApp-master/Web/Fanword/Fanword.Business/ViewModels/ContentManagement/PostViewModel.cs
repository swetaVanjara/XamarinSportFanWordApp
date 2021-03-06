using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.ContentManagement {
    public class PostViewModel {
        public string Id { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public string ContentSourceUrl => PostImage?.Url ?? PostVideo?.Url ?? PostLink?.ImageUrl ?? "";
        public string PostSource => PostImage != null ? "Image" : PostLink != null ? "Link" : PostVideo != null ? "Video" : "Unknown";
        public string Content { get; set; }
        public string CreatedByName { get; set; }
        public bool RemoveContentSource { get; set; }

        public PostImageViewModel PostImage { get; set; }
        public PostLinkViewModel PostLink { get; set; }
        public PostVideoViewModel PostVideo { get; set; }
    }
}
