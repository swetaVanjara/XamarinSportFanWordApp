using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Poco.Models
{
    public class FeedItem
    {
        public string TwitterUrl { get; set; }
        public string FacebookUrl { get; set; }
        public float ImageAspectRatio { get; set; }
        public string SportId { get; set; }
        public string SchoolId { get; set; }
        public string TeamId { get; set; }
        public string ContentSourceId { get; set; }
        public string AdvertisementUrl { get; set; }
        public bool IsCommented { get; set; }
        public bool IsLiked { get; set; }
        public string LinkImage { get; set; }
        public string LinkTitle { get; set; }
        public string LinkUrl { get; set; }
        public string ImageUrl { get; set; }
        public string VideoImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public int ShareCount { get; set; }
        public int TagCount { get; set; }
        public int CommentCount { get; set; }
        public int LikeCount { get; set; }
        public string Username { get; set; }
        public string ProfileUrl { get; set; }
        public string Content { get; set; }
        public string CreatedById { get; set; }
        public DateTime DateLastModifiedUtc { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public string Id { get; set; }
        public string InstagramUrl { get; set; }
        public bool IsSharePost { get; set; }
        public string SharedUsername { get; set; }

    }
}
