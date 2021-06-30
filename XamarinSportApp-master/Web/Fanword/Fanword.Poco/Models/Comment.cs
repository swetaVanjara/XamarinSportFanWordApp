using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Poco.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string ParentCommentId { get; set; }
        public string CreatedById { get; set; }
        public string PostId { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public string Username { get; set; }
        public string ProfileUrl { get; set; }
        public int LikeCount { get; set; }
        public int ReplyCount { get; set; }
        public bool IsLiked { get; set; }
        public string RepliedToUsername { get; set; }

    }
}
