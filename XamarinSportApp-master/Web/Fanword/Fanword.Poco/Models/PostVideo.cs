using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Poco.Models
{
    public class PostVideo
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Container { get; set; }
        public string Blob { get; set; }
        public string ImageUrl { get; set; }
        public string ImageContainer { get; set; }
        public string ImageBlob { get; set; }
        public float ImageAspectRatio { get; set; }
    }
}
