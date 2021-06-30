using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Poco.Models
{
    public class PostImage
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Container { get; set; }
        public string Blob { get; set; }
        public float ImageAspectRatio { get; set; }
    }
}
